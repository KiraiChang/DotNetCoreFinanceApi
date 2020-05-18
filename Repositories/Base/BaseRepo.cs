using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Dapper;
using Microsoft.Extensions.Options;

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
        public IList<T> GetList<T1>(T1 filter)
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
                    sql = string.Concat(sql,
                        " WHERE ",
                        string.Join(" AND ", conditions.Where(x => x.GetValue(filter, null) != null)
                                                        .Select(x => x.Name.Contains("Begin") ? $"{x.Name.Replace("Begin", string.Empty)}>=@{x.Name}"
                                                                        : x.Name.Contains("End") ? $"{x.Name.Replace("End", string.Empty)}<=@{x.Name}"
                                                                        : $"{x.Name}=@{x.Name}")));
                    result = conn.Query<T>(sql, filter) as IList<T>;
                }
                else
                {
                    result = conn.Query<T>(sql) as IList<T>;
                }
            }

            return result;
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
        public int Insert(IList<T> values)
        {
            var t = typeof(T);
            var props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var sql = string.Concat("INSERT IGNORE INTO ",
                                    t.Name,
                                    "(",
                                    string.Join(", ", props.Select(x => x.Name)),
                                    ") ",
                                    "VALUES(",
                                    string.Join(", ", props.Select(x => $"@{x.Name}")),
                                    ")");
            var result = 0;
            var provider = DbProviderFactories.GetFactory(_setting.CurrentValue.Type);
            using (var conn = provider.CreateConnection())
            {
                conn.ConnectionString = _setting.CurrentValue.Connection;
                conn.Open();
                AddTypeHandler();
                result = conn.Execute(sql, values);
            }

            return result;
        }
    }
}