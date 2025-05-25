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
    }
}
