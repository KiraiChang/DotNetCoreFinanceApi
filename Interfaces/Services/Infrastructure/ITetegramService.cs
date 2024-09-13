using System.Threading.Tasks;

namespace FinanceApi.Interfaces.Services.Infrastructure
{
    /// <summary>
    /// interface of telegram service
    /// </summary>
    public interface ITetegramService
    {
        /// <summary>
        /// send message
        /// </summary>
        /// <param name="message">message</param>
        /// <returns>async task</returns>
        Task SendMessage(string message);
    }
}
