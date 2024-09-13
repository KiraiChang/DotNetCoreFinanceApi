namespace FinanceApi.Models.Settings
{
    /// <summary>
    /// Telegram Setting
    /// </summary>
    public class TelegramSetting
    {
        /// <summary>
        /// bot token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// chat id
        /// </summary>
        public string ChatId { get; set; }
    }
}
