using System.Web;

namespace FinanceApi.Cores.Extensions
{
    /// <summary>
    /// String Extension
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Get url encode string
        /// </summary>
        /// <param name="source">not encoding</param>
        /// <returns>url encoding string</returns>
        public static string ExtUrlEncode(this string source)
        {
            return HttpUtility.UrlEncode(source);
        }
    }
}
