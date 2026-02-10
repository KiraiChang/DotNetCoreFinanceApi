using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceApi.Services.Grabs.Models
{
    /// <summary>
    /// 台灣證交所股票Api返還結構
    /// </summary>
    [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "*", Justification = "According for api result")]
    public class TWSEStock
    {
        /// <summary>
        /// 股票資訊
        /// </summary>
        public List<List<string>> data { get; set; } = new List<List<string>>();
    }
}
