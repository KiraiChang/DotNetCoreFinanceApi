namespace FinanceApi.Models.Services.Telegram
{
    /// <summary>
    /// Tg result model
    /// </summary>
    public class TelegramResult
    {
        /// <summary>
        /// is ok
        /// </summary>
        public bool ok { get; set; }

        /// <summary>
        /// result data
        /// </summary>
        public Result result { get; set; }
    }

    /// <summary>
    /// result data
    /// </summary>
    public class Result
    {
        /// <summary>
        /// message
        /// </summary>
        public int message_id { get; set; }

        /// <summary>
        /// direction
        /// </summary>
        public From from { get; set; }

        /// <summary>
        /// chat
        /// </summary>
        public Chat chat { get; set; }

        /// <summary>
        /// data
        /// </summary>
        public int date { get; set; }

        /// <summary>
        /// text
        /// </summary>
        public string text { get; set; }
    }

    /// <summary>
    /// direction
    /// </summary>
    public class From
    {
        /// <summary>
        /// id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// is bot
        /// </summary>
        public bool is_bot { get; set; }

        /// <summary>
        /// first name
        /// </summary>
        public string first_name { get; set; }

        /// <summary>
        /// user name
        /// </summary>
        public string username { get; set; }
    }

    /// <summary>
    /// chat
    /// </summary>
    public class Chat
    {
        /// <summary>
        /// id
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// title
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// type
        /// </summary>
        public string type { get; set; }
    }

}
