using FinanceApi.Models.Attributes;
using System;

namespace FinanceApi.Models.Entity
{
    /// <summary>
    /// 股票的Model
    /// </summary>
    [UniqueKey(nameof(StockId), nameof(Date))]
    public class Stock
    {
        /// <summary>
        /// 股票Id
        /// </summary>
        public string StockId { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 開盤價
        /// </summary>
        public decimal OpenPrice { get; set; }

        /// <summary>
        /// 最高價
        /// </summary>
        public decimal MaxPrice { get; set; }

        /// <summary>
        /// 最低價
        /// </summary>
        public decimal MinPrice { get; set; }

        /// <summary>
        /// 收盤價
        /// </summary>
        public decimal ClosePrice { get; set; }

        /// <summary>
        /// 漲跌價差
        /// </summary>
        public decimal Decline { get; set; }

        /// <summary>
        /// 成交股數
        /// </summary>
        public long Volume { get; set; }

        /// <summary>
        /// 成交金額
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// 成交筆數
        /// </summary>
        public long Count { get; set; }
    }
}