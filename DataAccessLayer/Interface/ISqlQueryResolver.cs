using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace TeachersPoint.DataAccessLayer.Interface
{
    public interface ISqlQueryResolver
    {
        public DataTable ResolveSqlQuery(string query);

    }
}