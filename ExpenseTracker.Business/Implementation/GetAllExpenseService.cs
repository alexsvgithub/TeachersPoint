using ExpenseTracker.Business.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExpenseTracker.Business.Implementation
{
    public class GetAllExpenseService : IGetAllExpenseService
    {

        public GetAllExpenseService()
        {

        }


        public async Task<JObject> GetAllExpenseByUserId(string userId)
        {
            var a = @"{'test':'123'}";
            await Task.Delay(1000);
            return JObject.Parse(a.ToString());
        }
    }
}
