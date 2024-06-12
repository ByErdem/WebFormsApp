using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebFormsApp.Service.Abstract
{
    public interface ISQLService
    {
        Task<List<T>> ExecuteSqlQuery<T>(string sqlQuery);
        Task<T> ExecuteStoredProcedure<T>(string procedureName, params object[] parameters);
    }
}
