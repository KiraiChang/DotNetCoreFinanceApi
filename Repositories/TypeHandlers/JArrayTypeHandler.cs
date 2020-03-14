using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Dapper;
using Newtonsoft.Json.Linq;

namespace FinanceApi.Repositories.TypeHandlers
{
    /// <summary>
    /// JArray Type Handler
    /// </summary>
    public class JArrayTypeHandler : SqlMapper.TypeHandler<JArray>
    {
        /// <summary>
        /// Parser db value to JArray
        /// </summary>
        /// <param name="value">db value</param>
        /// <returns>JArray</returns>
        public override JArray Parse(object value)
        {
            return JArray.Parse(value.ToString());
        }

        /// <summary>
        /// Set JArray to db parameter
        /// </summary>
        /// <param name="parameter">db parameter</param>
        /// <param name="value">JArray</param>
        public override void SetValue(IDbDataParameter parameter, JArray value)
        {
            parameter.Value = value.ToString();
        }
    }
}