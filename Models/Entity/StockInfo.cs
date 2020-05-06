using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApi.Models.Entity
{
    /// <summary>
    /// stock base information
    /// </summary>
    public class StockInfo
    {
        /// <summary>
        /// Id of stock
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// international code of stock
        /// </summary>
        public string IISINCoded { get; set; }

        /// <summary>
        /// Market of stock
        /// </summary>
        public string Market { get; set; }

        /// <summary>
        /// Industry of stock
        /// </summary>
        public string Industry { get; set; }

        /// <summary>
        /// PublicDate of stock
        /// </summary>
        public DateTime PublicDate { get; set; }
    }
}