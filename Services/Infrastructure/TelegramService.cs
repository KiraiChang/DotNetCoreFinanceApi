using FinanceApi.Interfaces.Services.Infrastructure;
using FinanceApi.Models.Services.Telegram;
using FinanceApi.Models.Settings;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Threading.Tasks;

namespace FinanceApi.Services.Infrastructure
{
    /// <summary>
    /// telegram service
    /// </summary>
    public class TelegramService : ITetegramService
    {
        /// <summary>
        /// setting of telegram
        /// </summary>
        private readonly IOptionsMonitor<TelegramSetting> _settings = null;

        /// <summary>
        /// acot.
        /// </summary>
        /// <param name="settings">setting of telegram</param>
        public TelegramService(IOptionsMonitor<TelegramSetting> settings)
        {
            _settings = settings;
        }

        /// <inheritdoc/>
        public async Task SendMessage(string message)
        {
            using (var client = new RestClient(await GetUrl(_settings.CurrentValue.Token, "sendMessage")))
            {
                var request = new RestRequest()
                {
                    Method = Method.Post,
                };
                request.AddParameter("chat_id", _settings.CurrentValue.ChatId);
                request.AddParameter("text", message);
                var result = await client.ExecuteAsync<TelegramResult>(request);
            }
        }

        /// <summary>
        /// get telegram url
        /// </summary>
        /// <param name="token">bot token</param>
        /// <param name="method">method</param>
        /// <returns>telegram url</returns>
        private async Task<string> GetUrl(string token, string method)
        {
            return string.Format("https://api.telegram.org/bot{0}/{1}", token, method);
        }
    }
}
