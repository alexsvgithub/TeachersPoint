using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeachersPoint.Core.RequestDto
{
    public class MongoRequestDto
    {
        public object _id {  get; set; }
        public string ids  => _id.ToString();
        public string name { get; set; }
        public string term { get; set; }
        public string std { get; set; }
        public string div { get; set; }
        public int rollNo { get; set; }
        public int year { get; set; }
        public Marksforsubject[] MarksForSubjects { get; set; }
    }

    public class Marksforsubject
    {
        public string SubjectName { get; set; }
        public List<string> reviewForSubject { get; set; }
    }

}
