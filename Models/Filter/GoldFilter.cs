using System;

namespace FinanceApi.Models.Filter
{
    /// <summary>
    /// Exchange Filter
    /// </summary>
    public class GoldFilter
    {

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