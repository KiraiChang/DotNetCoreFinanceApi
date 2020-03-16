using System;
using System.ComponentModel;

namespace FinanceApi.Cores.Extensions
{
    /// <summary>
    /// Enum Extension
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// Get enum Description
        /// </summary>
        /// <typeparam name="T">type of enum</typeparam>
        /// <param name="source"> enum source</param>
        /// <returns>description</returns>
        public static string ExtGetDescription<T>(this T source) where T : Enum
        {
            var fi = source.GetType().GetField(source.ToString());

            var attrs = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attrs.Length > 0)
            {
                return attrs[0].Description;
            }

            return source.ToString();
        }
    }
}