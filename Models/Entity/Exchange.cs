using FinanceApi.Models.Attributes;
using System;
using System.Collections.Generic;

namespace FinanceApi.Models.Entity
{
    /// <summary>
    /// Exchange Entity
    /// </summary>
    [UniqueKey(nameof(Date))]
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