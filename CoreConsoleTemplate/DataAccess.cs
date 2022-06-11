using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreConsoleTemplate
{
    public class DataAccess : IDataAccess
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DataAccess> _logger;
        public DataAccess(IConfiguration configuration, ILogger<DataAccess> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public IEnumerable<T> Read<T>(string sqlQuery)
        {
            IEnumerable<T> result=null;
            try
            {
                using (IDbConnection con = new SqlConnection(GetConnectionString()))
                {
                    result = con.Query<T>(sqlQuery);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return result;
        }
        public IEnumerable<T> ReadWithParamater<T>(string sqlQuery, DynamicParameters param)
        {
            IEnumerable<T> result = null;
            try
            {
                using (IDbConnection con = new SqlConnection(GetConnectionString()))
                {
                    result = con.Query<T>(sqlQuery, param);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return result;
        }

        public bool Create(string sqlQuery, DynamicParameters param)
        {
            int result = -1;
            try
            {
                using (IDbConnection con = new SqlConnection(GetConnectionString()))
                {
                    result = con.Execute(sqlQuery, param);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return result > 0;
        }

        public bool Update(string sqlQuery, DynamicParameters param)
        {
            int result = -1;
            try
            {
                using (IDbConnection con = new SqlConnection(GetConnectionString()))
                {
                    result = con.Execute(sqlQuery, param);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return result > 0;
        }
        public bool Delete(string sqlQuery, DynamicParameters param)
        {
            int result = -1;
            try
            {
                using (IDbConnection con = new SqlConnection(GetConnectionString()))
                {
                    result = con.Execute(sqlQuery, param);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return result > 0;
        }
        public async Task<IEnumerable<T>> ReadAsync<T>(string sqlQuery)
        {
            IEnumerable<T> result = null;
            try
            {
                using (IDbConnection con = new SqlConnection(GetConnectionString()))
                {
                    result = await con.QueryAsync<T>(sqlQuery);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return result;
        }
        public async Task<IEnumerable<T>> ReadWithParamaterAsync<T>(string sqlQuery, DynamicParameters param)
        {
            IEnumerable<T> result = null;
            try
            {
                using (IDbConnection con = new SqlConnection(GetConnectionString()))
                {
                    result = await con.QueryAsync<T>(sqlQuery, param);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return result;
        }
        public async Task<bool> CreateAsync<T>(string sqlQuery, T data) where T:class
        {
            int result = -1;
            try
            {
                using (IDbConnection con = new SqlConnection(GetConnectionString()))
                {
                    result = await con.ExecuteAsync(sqlQuery, data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return result > 0;
        }

        public async Task<bool> UpdateAsync<T>(string sqlQuery, T data) where T : class
        {
            int result = -1;
            try
            {
                using (IDbConnection con = new SqlConnection(GetConnectionString()))
                {
                    result = await con.ExecuteAsync(sqlQuery, data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return result > 0;
        }
        public async Task<bool> DeleteAsync<T>(string sqlQuery, T data) where T : class
        {
            int result = -1;
            try
            {
                using (IDbConnection con = new SqlConnection(GetConnectionString()))
                {
                    result = await con.ExecuteAsync(sqlQuery, data);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return result > 0;
        }

        private string GetConnectionString()
        {
            return _configuration.GetConnectionString("");
        }
    }
}
