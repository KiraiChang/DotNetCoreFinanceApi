using System;

namespace FinanceApi.Models.Filter
{
    /// <summary>
    /// Exchange Filter
    /// </summary>
    public class ExchangeFilter
    {
        /// <summary>
        /// Filter by date
        /// </summary>
        public DateTime? Date { get; set; }
    }
}