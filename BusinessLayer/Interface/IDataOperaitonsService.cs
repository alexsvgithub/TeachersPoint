using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersPoint.Core.RequestDto;

namespace TeachersPoint.BusinessLayer.Interface
{
    public interface IDataOperaitonsService
    {
        public string AddStudentRecord(StudentDetails student);
    }
}
