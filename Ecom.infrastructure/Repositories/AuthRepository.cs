using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecom.Core.DTO;
using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using Ecom.Core.Services;
using Ecom.Core.Sharing;
using Microsoft.AspNetCore.Identity;

namespace Ecom.infrastructure.Repositories
{
    public class AuthRepository : IAuth
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IEmailService emailService;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IGenerateToken generatetoken;
        public AuthRepository(UserManager<AppUser> userManager, IEmailService emailService, SignInManager<AppUser> signInManager, IGenerateToken generatetoken)
        {
            this.userManager = userManager;
            this.emailService = emailService;
            this.signInManager = signInManager;
            this.generatetoken = generatetoken;
        }
        public async Task<string> RegisterAsync(RegisterDTO registerDTO)
        {
            if (registerDTO == null)
            {
                return null;
            }
            if (await userManager.FindByNameAsync(registerDTO.UserName) is not null)
            {
                return "This UserName is already registered";
            }
            if (await userManager.FindByEmailAsync(registerDTO.Email) is not null)
            {
                return "This email is already registered";
            }
            AppUser user = new AppUser()
            {
                Email = registerDTO.Email,
                UserName = registerDTO.UserName,
                DisplayName = registerDTO.DisplayName,
            };
            var result = await userManager.CreateAsync(user, registerDTO.Password);
            if (result.Succeeded is not true)
            {
                return result.Errors.ToList()[0].Description;
            }
            string token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            await SendEmail(user.Email, token, "active", "ActiveEmail", "Confirm your email, Click on the button to activate your account.");

            return "Done";
        }


        public async Task SendEmail(string email, string code, string component, string subject, string message)
        {
            var result = new EmailDTO(email, "helaldoaa11@gmail.com", subject, EmailStringBody.send(email, code, component, message));
            await emailService.SendEmail(result);
        }


        public async Task<string> LoginAsync(LoginDTO login)
        {
            if (login == null)
            {
                return null;

            }
            var findUser = await userManager.FindByEmailAsync(login.Email);
            if (!findUser.EmailConfirmed)
            {
                string token = await userManager.GenerateEmailConfirmationTokenAsync(findUser);
                await SendEmail(findUser.Email, token, "active", "ActiveEmail", "Confirm your email, Click on the button to activate your account.");
                return "Please confirm your email";

            }
            var result = await signInManager.CheckPasswordSignInAsync(findUser, login.Password, true);
            if (result.Succeeded)
            {
                return generatetoken.GetAndCreateToken(findUser);
            }

            return "Email or Password is incorrect";


        }

        public async Task<bool> SendEmailForForgetPassword(string email)
        {
            var findUser = await userManager.FindByEmailAsync(email);
            if (findUser == null)
            {
                return false;
            }
            var token = await userManager.GeneratePasswordResetTokenAsync(findUser);
            await SendEmail(findUser.Email, token, "reset", "ResetPassword", "Click on the button to reset your password.");
            return true;
        }

        public async Task<string> ResetPassword(ResetPasswordDTO resetPassword)
        {
            var findUser = await userManager.FindByEmailAsync(resetPassword.Email);
            if (findUser == null)
            {
                return null;
            }
            var result = await userManager.ResetPasswordAsync(findUser, resetPassword.Token, resetPassword.Password);
            if (result.Succeeded)
            {
                return "Password reset successfully";
            }
            return result.Errors.ToList()[0].Description;
        }

        public async Task<bool> ActiveAccount( ActiveteAccountDTO accountDTO)
        {
            var findUser = await userManager.FindByEmailAsync(accountDTO.Email);
            if (findUser == null)
            {
                return false;
            }
            var result = await userManager.ConfirmEmailAsync(findUser, accountDTO.Token);
            if (result.Succeeded)
            
                 return true;
            var token = await userManager.GenerateEmailConfirmationTokenAsync(findUser);
            await SendEmail(findUser.Email, token, "active", "ActiveEmail", "Confirm your email, Click on the button to activate your account.");
            return false;


        }


    }
}
