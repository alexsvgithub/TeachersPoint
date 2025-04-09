using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Business.Interface
{
    public interface IGetAllExpenseService
    {
        Task<JObject> GetAllExpenseByUserId(string userId);
    }
}
