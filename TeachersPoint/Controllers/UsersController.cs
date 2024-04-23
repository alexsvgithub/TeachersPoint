using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TeachersPoint.BusinessLayer.Interface;
using TeachersPoint.Core.RequestDto;

namespace TeachersPoint.WebApisService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserAuthorization _userAuthorization;
        public UsersController(IUserAuthorization userAuthorization)
        {
            _userAuthorization = userAuthorization; 
        }

        [HttpPost]
        [Route("RegisterUser")]
        public JsonResult RegisterUser(UserDto user)
        {
            return _userAuthorization.RegisterNewUser(user);
        }

        [HttpPost]
        [Route("LogIn")]
        public string LogIn(UserDto user)
        {
            return _userAuthorization.LogIn(user);
        }
    }
}
