namespace FinanceApi.Models.Enums.Stocks
{
    /// <summary>
    /// StockProps Index
    /// </summary>
    public enum StockProps
    {
        /// <summary>
        /// 日期
        /// </summary>
        Date = 0,

        /// <summary>
        /// 成交股數
        /// </summary>
        Volume = 1,

        /// <summary>
        /// 成交金額
        /// </summary>
        Amount = 2,

        /// <summary>
        /// 開盤價
        /// </summary>
        OpenPrice = 3,

        /// <summary>
        /// 最高價
        /// </summary>
        MaxPrice = 4,

        /// <summary>
        /// 最低價
        /// </summary>
        MinPrice = 5,

        /// <summary>
        /// 收盤價
        /// </summary>
        ClosePrice = 6,

        /// <summary>
        /// 漲跌價差
        /// </summary>
        Decline = 7,

        /// <summary>
        /// 成交筆數
        /// </summary>
        Count = 8,
    }
}