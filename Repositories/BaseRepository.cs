using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using LoginApp.Models;
using Dapper;

namespace LoginApp.Repositories
{
    public class BaseRepository
    {
        private static string _connectionString;
        private IDbConnection db => new SqlConnection(_connectionString);

        public BaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
      
        protected async Task<int> Add<T>(object param = null) where T : BaseModel
        {
            string storedProcuedure = typeof(T).Name.ToString() + "Add";

            int id = 0;
            using (db)
            {
                id = await db.ExecuteScalarAsync<int>(storedProcuedure, param, commandType: CommandType.StoredProcedure);
            }
            return id;
        }

        protected async Task<T> QueryFirstOrDefaultAsync<T>(string storedProcuedure, object param = null)
        {
            T item = default(T);
            using (db)
            {
                item = await db.QueryFirstOrDefaultAsync<T>(storedProcuedure, param, commandType: CommandType.StoredProcedure);
            }
            return item;
        }

    }
}
