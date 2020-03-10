using System;

namespace FinanceApi.Models.Filter
{
    /// <summary>
    /// filter of stock
    /// </summary>
    public class StockFilter
    {
        /// <summary>
        /// serial id
        /// </summary>
        public string StockId { get; set; }

        /// <summary>
        /// date
        /// </summary>
        public DateTime? Date { get; set; }
    }
}