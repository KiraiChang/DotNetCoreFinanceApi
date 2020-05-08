using System;

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
        /// CFICode
        /// </summary>
        public string CFICode { get; set; }

        /// <summary>
        /// Name of Stock
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// international code of stock
        /// </summary>
        public string ISINCode { get; set; }

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

        /// <summary>
        /// Memo
        /// </summary>
        public string Memo { get; set; }
    }
}