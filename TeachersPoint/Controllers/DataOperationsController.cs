
using Microsoft.AspNetCore.Mvc;
using TeachersPoint.BusinessLayer.Interface;
using TeachersPoint.Core.RequestDto;

namespace TeachersPoint.WebApisService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataOperationsController : ControllerBase
    {
        private IDataOperaitonsService _dataOperaitonsService;
        public DataOperationsController(IDataOperaitonsService dataOperaitonsService)
        {
            _dataOperaitonsService = dataOperaitonsService;
        }

        [HttpPost]
        [Route("AddStudentRecord")]
        public string AddStudentRecord(StudentDetails student)
        {
            return _dataOperaitonsService.AddStudentRecord(student);
        }

    }
}
