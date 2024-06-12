using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebFormsApp.Service.Abstract;

namespace WebFormsApp.Service.Concrete
{
    public class SQLManager : ISQLService
    {
        private readonly IDBContextEntity _context;

        public SQLManager(IDBContextEntity context)
        {
            _context = context;
        }

        public async Task<List<T>> ExecuteSqlQuery<T>(string sqlQuery)
        {
            try
            {
                var result = await _context.Database.SqlQuery<T>(sqlQuery).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"SQL query execution failed: {ex.Message}");
            }
        }

        public async Task<T> ExecuteStoredProcedure<T>(string procedureName, params object[] parameters)
        {
            try
            {
                var result = await _context.Database.SqlQuery<T>($"EXEC {procedureName}", parameters).FirstOrDefaultAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"Stored procedure execution failed: {ex.Message}");
            }
        }
    }
}
