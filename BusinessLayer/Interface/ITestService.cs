using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersPoint.Core.RequestDto;

namespace TeachersPoint.BusinessLayer.Interface
{
    public interface ITestService
    {
        public string ThisIsTestServiceMethodCalling(string s);

        public int RegisterNewUser(UserDto user);

        public string LogIn(UserDto user);
    }
}
