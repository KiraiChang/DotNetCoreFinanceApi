using System;
using System.Collections.Generic;

namespace FinanceApi.Models.Entity
{
    /// <summary>
    /// Exchange Entity
    /// </summary>
    public class Exchange
    {
        /// <summary>
        /// Date of record
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Data of Exchange
        /// </summary>
        public IList<ExchangeItem> Data { get; set; }
    }
}