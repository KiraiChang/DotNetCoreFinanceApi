using System;

namespace FinanceApi.Models.Attributes
{
    /// <summary>
    /// Unique Key
    /// </summary>
    public class UniqueKeyAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        public UniqueKeyAttribute(params string[] keys)
        {
            Keys = keys;
        }

        /// <summary>
        /// key name
        /// </summary>
        public string[] Keys { get; set; }
    }
}