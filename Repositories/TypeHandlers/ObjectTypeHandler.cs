using System.Data;
using Dapper;
using Newtonsoft.Json;

namespace FinanceApi.Repositories.TypeHandlers
{
    /// <summary>
    /// Handler Object
    /// </summary>
    /// <typeparam name="T">Type of object</typeparam>
    public class ObjectTypeHandler<T> : SqlMapper.TypeHandler<T>
    {
        /// <summary>
        /// Parse Object from db value
        /// </summary>
        /// <param name="value">db value</param>
        /// <returns>instance</returns>
        public override T Parse(object value)
        {
            return JsonConvert.DeserializeObject<T>(value.ToString());
        }

        /// <summary>
        /// set Object to db parameter
        /// </summary>
        /// <param name="parameter">db parameter</param>
        /// <param name="value">value</param>
        public override void SetValue(IDbDataParameter parameter, T value)
        {
            parameter.Value = JsonConvert.SerializeObject(value);
        }
    }
}