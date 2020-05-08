using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApi.Models.Enums
{
    /// <summary>
    /// 股票資訊的屬性
    /// </summary>
    public enum StockInfoProps
    {
        /// <summary>
        /// Id and Name
        /// </summary>
        IdAndName = 0,

        /// <summary>
        /// International Code
        /// </summary>
        ISINCode = 1,

        /// <summary>
        /// Public Date
        /// </summary>
        PublicDate = 2,

        /// <summary>
        /// Marketing
        /// </summary>
        Market = 3,

        /// <summary>
        /// Industry
        /// </summary>
        Industry = 4,

        /// <summary>
        /// CFICode
        /// </summary>
        CFICode = 5,

        /// <summary>
        /// Memo
        /// </summary>
        Memo = 6,
    }
}