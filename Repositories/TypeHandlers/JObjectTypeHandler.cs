using System.Data;
using Dapper;
using Newtonsoft.Json.Linq;

namespace FinanceApi.Repositories.TypeHandlers
{
    /// <summary>
    /// Handler JObject
    /// </summary>
    public class JObjectTypeHandler : SqlMapper.TypeHandler<JObject>
    {
        /// <summary>
        /// Parse JObject from db value
        /// </summary>
        /// <param name="value">db value</param>
        /// <returns>instance</returns>
        public override JObject Parse(object value)
        {
            return JObject.Parse(value.ToString());
        }

        /// <summary>
        /// set JObject to db parameter
        /// </summary>
        /// <param name="parameter">db parameter</param>
        /// <param name="value">value</param>
        public override void SetValue(IDbDataParameter parameter, JObject value)
        {
            parameter.Value = value.ToString();
        }
    }
}