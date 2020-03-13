using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Dapper;

namespace FinanceApi.Repositories.Base
{
    /// <summary>
    /// Base Repository
    /// </summary>
    /// <typeparam name="T">type of repository</typeparam>
    public class BaseRepo<T> where T : class
    {
        /// <summary>
        /// database connection
        /// </summary>
        private readonly IDbConnection _db = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepo{T}" /> class.
        /// </summary>
        /// <param name="db">db connection</param>
        public BaseRepo(IDbConnection db)
        {
            _db = db;
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

            if (filter != null && conditions.Any(x => x.GetValue(filter, null) != null))
            {
                sql = string.Concat(sql,
                    " WHERE ",
                    string.Join(" AND ", conditions.Where(x => x.GetValue(filter, null) != null).Select(x => $"{x.Name}=@{x.Name}")));
                return _db.Query<T>(sql, filter) as IList<T>;
            }

            return _db.Query<T>(sql) as IList<T>;
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
            return _db.Execute(sql, values);
        }
    }
}