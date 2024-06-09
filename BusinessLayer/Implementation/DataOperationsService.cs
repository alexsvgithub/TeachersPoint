using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeachersPoint.BusinessLayer.Interface;
using TeachersPoint.Core.RequestDto;
using TeachersPoint.DataAccessLayer.Implementation;
using TeachersPoint.DataAccessLayer.Interface;

namespace TeachersPoint.BusinessLayer.Implementation
{
    public class DataOperationsService : IDataOperaitonsService
    {
        private ISqlQueryResolver _sqlQueryResolver;
        public DataOperationsService(ISqlQueryResolver sqlQueryResolver)
        {
            _sqlQueryResolver = sqlQueryResolver;
        }
        public string AddStudentRecord(StudentDetails student)
        {
            var query = $@"
                          INSERT INTO mastertableTest (year_of_study, rollno, div, standard, term, name)
                          SELECT '{student.year_Of_Study}', '{student.rollNo}', '{student.div}', '{student.standard}', '{student.term}', '{student.name}'
                        ";

            var ifEmailExist = _sqlQueryResolver.ResolveSqlQuery(query);
            if(JsonConvert.SerializeObject(ifEmailExist) != "[]") // if added successfully
            {
                return "Student Already Present in Databse";
            }
            return "Student Record Added in Database";
        }

        public string AddStudentRecord
    }
}
