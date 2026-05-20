using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    /// <summary>
    /// check health controller
    /// </summary>
    public class HealthController : Controller
    {
        /// <summary>
        /// check health for system
        /// </summary>
        /// <returns></returns>
        [HttpGet("health")]
        public string Index()
        {
            return "OK!";
        }
    }
}
