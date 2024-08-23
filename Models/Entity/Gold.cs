using System;

namespace FinanceApi.Models.Entity
{
    /// <summary>
    /// gold info
    /// </summary>
    public class Gold
    {
        /// <summary>
        /// date
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// currency
        /// </summary>
        public int Currency { get; set; }

        /// <summary>
        /// unit
        /// </summary>
        public int Unit { get; set; }

        /// <summary>
        /// bid
        /// </summary>
        public decimal Bid { get; set; }

        /// <summary>
        /// ask
        /// </summary>
        public decimal Ask { get; set; }
    }
}
