using AutoMapper;
using Ecom.API.Helper;
using Ecom.Core.DTO;
using Ecom.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.API.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(IUnitOfWork work, IMapper mapper) : base(work, mapper)
        {
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {

            string result = await work.Auth.RegisterAsync(registerDTO);
            if (result != "Done")
            {
                return BadRequest(new ResponseAPI(400, result));
            }
            else
            {
                return Ok(new ResponseAPI(200, result));
            }
           
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var result = await work.Auth.LoginAsync(loginDTO);
            if (result.StartsWith("please"))
            {
                return BadRequest(new ResponseAPI(400, result));
            }
            Response.Cookies.Append("token", result, new CookieOptions
            {
                HttpOnly = true,
                Domain= "localhost", // Set your domain here
                Expires = DateTime.Now.AddDays(1),
                IsEssential = true, // Make the cookie essential
                SameSite = SameSiteMode.Strict,
                Secure = true // Set to true if using HTTPS
            });
            return Ok(new ResponseAPI(200));

        }
        [HttpPost("active-account")]
        public async Task<IActionResult> ActiveAccount(ActiveteAccountDTO accountDTO)
        {
            var result = await work.Auth.ActiveAccount(accountDTO);
            if (result)
            {
                return Ok(new ResponseAPI(200, "Your account is activated successfully"));
            }
            else
            {
                return BadRequest(new ResponseAPI(400, "Your account is not activated"));
            }
        }

        [HttpPost("send-email-for-forget-password")]
        public async Task<IActionResult> SendEmailForForgetPassword(string email)
        {
            var result = await work.Auth.SendEmailForForgetPassword(email);
            if (result)
            {
                return Ok(new ResponseAPI(200, "Email sent successfully"));
            }
            else
            {
                return BadRequest(new ResponseAPI(400, "Email not found"));
            }
        }


    }
}
