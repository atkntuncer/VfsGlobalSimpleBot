using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreConsoleTemplate
{
    public interface IDataAccess
    {
        bool Create(string sqlQuery, DynamicParameters param);
        Task<bool> CreateAsync<T>(string sqlQuery, T data) where T:class;
        bool Delete(string sqlQuery, DynamicParameters param);
        Task<bool> DeleteAsync<T>(string sqlQuery, T data) where T : class;
        IEnumerable<T> Read<T>(string sqlQuery);
        Task<IEnumerable<T>> ReadAsync<T>(string sqlQuery);
        IEnumerable<T> ReadWithParamater<T>(string sqlQuery, DynamicParameters param);
        Task<IEnumerable<T>> ReadWithParamaterAsync<T>(string sqlQuery, DynamicParameters param);
        bool Update(string sqlQuery, DynamicParameters param);
        Task<bool> UpdateAsync<T>(string sqlQuery, T data) where T : class;
    }
}