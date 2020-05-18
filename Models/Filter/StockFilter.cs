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
        /// begin date
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// end date
        /// </summary>
        public DateTime? EndDate { get; set; }
    }
}