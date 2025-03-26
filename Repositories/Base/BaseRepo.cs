using Dapper;
using FinanceApi.Models.Attributes;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace FinanceApi.Repositories.Base
{
    /// <summary>
    /// Base Repository
    /// </summary>
    /// <typeparam name="T">type of repository</typeparam>
    public class BaseRepo<T> where T : class
    {
        /// <summary>
        /// database connection setting
        /// </summary>
        private readonly IOptionsMonitor<ConnectionSetting> _setting = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepo{T}" /> class.
        /// </summary>
        /// <param name="setting">db connection setting</param>
        public BaseRepo(IOptionsMonitor<ConnectionSetting> setting)
        {
            _setting = setting;
        }

        /// <summary>
        /// Get result from db
        /// </summary>
        /// <typeparam name="T1">type of condition</typeparam>
        /// <param name="filter">filter condition</param>
        /// <returns>result of type T</returns>
        public async Task<IList<T>> GetList<T1>(T1 filter)
        {
            var t = typeof(T);
            var props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var conditions = typeof(T1).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var sql = string.Concat("SELECT ",
                                    string.Join(", ", props.Select(x => x.Name)),
                                    " FROM ",
                                    t.Name);
            var result = null as IList<T>;
            var provider = DbProviderFactories.GetFactory(_setting.CurrentValue.Type);
            using (var conn = provider.CreateConnection())
            {
                conn.ConnectionString = _setting.CurrentValue.Connection;
                conn.Open();
                AddTypeHandler();
                if (filter != null && conditions.Any(x => x.GetValue(filter, null) != null))
                {
                    var whereItem = conditions.Where(x => x.GetValue(filter, null) != null)
                                .Select(x => x.Name.Contains("Begin") ? $"{x.Name.Replace("Begin", string.Empty)}>=@{x.Name}"
                                                                        : x.Name.Contains("End") ? $"{x.Name.Replace("End", string.Empty)}<=@{x.Name}"
                                                                        : $"{x.Name}=@{x.Name}");
                    var whereCondition = string.Join(" AND ", whereItem);
                    sql = string.Concat(sql, " WHERE ", whereCondition);
                    result = (await conn.QueryAsync<T>(sql, filter)).ToList();
                }
                else
                {
                    result = (await conn.QueryAsync<T>(sql)).ToList();
                }
            }

            return await Task.FromResult(result);
        }

        /// <summary>
        /// Implement for SqlMapper.AddTypeHandler
        /// </summary>
        protected virtual void AddTypeHandler()
        {
        }

        /// <summary>
        /// Insert value to db
        /// </summary>
        /// <param name="values">list of value</param>
        /// <returns>effect count</returns>
        public async Task<int> Insert(IList<T> values)
        {
            var result = 0;
            if (values.Count() > 0)
            {
                var t = typeof(T);
                var attribute = t.GetCustomAttribute<UniqueKeyAttribute>();
                var props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var sql = string.Empty;
                if (attribute != null)
                {
                    sql = string.Concat("INSERT INTO ",
                        t.Name,
                        "(",
                        string.Join(", ", props.Select(x => x.Name)),
                        ") ",
                        "VALUES(",
                        string.Join(", ", props.Select(x => $"@{x.Name}")),
                        ") ON DUPLICATE KEY UPDATE ",
                        string.Join(", ", props.Where(x => !attribute.Keys.Contains(x.Name))
                                                .Select(x => $"{x.Name} = VALUES({x.Name})")));
                }
                else
                {
                    sql = string.Concat("INSERT IGNORE INTO ",
                                        t.Name,
                                        "(",
                                        string.Join(", ", props.Select(x => x.Name)),
                                        ") ",
                                        "VALUES(",
                                        string.Join(", ", props.Select(x => $"@{x.Name}")),
                                        ")");
                }

                var provider = DbProviderFactories.GetFactory(_setting.CurrentValue.Type);
                using (var conn = provider.CreateConnection())
                {
                    conn.ConnectionString = _setting.CurrentValue.Connection;
                    conn.Open();
                    AddTypeHandler();
                    result = await conn.ExecuteAsync(sql, values);
                }
            }

            return result;
        }
    }
}