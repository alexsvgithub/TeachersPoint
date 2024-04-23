using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersPoint.Core.RequestDto;

namespace TeachersPoint.BusinessLayer.Interface
{
    public interface IUserAuthorization
    {
        public JsonResult RegisterNewUser(UserDto user);

        public string LogIn(UserDto user);
    }
}
