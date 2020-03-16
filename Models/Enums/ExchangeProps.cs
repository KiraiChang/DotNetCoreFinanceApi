using System.ComponentModel;

namespace FinanceApi.Models.Enums
{
    /// <summary>
    /// ExchangeProps Index
    /// </summary>
    public enum ExchangeProps
    {
        /// <summary>
        /// 日期
        /// </summary>
        [Description("Date")]
        Date = 0,

        /// <summary>
        /// 美元／新台幣
        /// </summary>
        [Description("USD/TWD")]
        UsdTwd = 1,

        /// <summary>
        /// 人民幣／新台幣
        /// </summary>
        [Description("RMB/TWD")]
        RmbTwd = 2,

        /// <summary>
        /// 歐元／美元
        /// </summary>
        [Description("EUR/USD")]
        EurUsd = 3,

        /// <summary>
        /// 美元／日幣
        /// </summary>
        [Description("USD/JPY")]
        UsdJpy = 4,

        /// <summary>
        /// 英鎊／美元
        /// </summary>
        [Description("GBP/USD")]
        GbpUsd = 5,

        /// <summary>
        /// 澳幣／美元
        /// </summary>
        [Description("AUD/USD")]
        AudUsd = 6,

        /// <summary>
        /// 美元／港幣
        /// </summary>
        [Description("USD/HKD")]
        UsdHkd = 7,

        /// <summary>
        /// 美元／人民幣
        /// </summary>
        [Description("USD/RMB")]
        UsdRmb = 8,

        /// <summary>
        /// 美元／南非幣
        /// </summary>
        [Description("USD/ZAR")]
        UsdZar = 9,

        /// <summary>
        /// 紐幣／美元
        /// </summary>
        [Description("NZD/USD")]
        NzdUsd = 10,
    }
}