using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecom.Core.DTO;

namespace Ecom.Core.Interfaces
{
    public interface IAuth
    {
        Task<string>  RegisterAsync(RegisterDTO registerDTO);
        Task<string> LoginAsync(LoginDTO loginDTO);
        Task<string> ResetPassword(ResetPasswordDTO resetPasswordDTO);
        //Task SendEmail(string email, string code, string component, string subject, string message);
        Task<bool> SendEmailForForgetPassword(string email);
        Task<bool> ActiveAccount(ActiveteAccountDTO accountDTO);


    }
}
