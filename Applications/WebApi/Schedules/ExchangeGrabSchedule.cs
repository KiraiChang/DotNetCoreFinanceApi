using System;
using FinanceApi.Interfaces.Services;
using FinanceApi.Interfaces.Services.Grabs;
using Microsoft.Extensions.Logging;

namespace WebApi.Schedules
{
    /// <summary>
    /// schedule of exchange grab
    /// </summary>
    public class ExchangeGrabSchedule
    {
        /// <summary>
        /// grab service
        /// </summary>
        private readonly IExchangeGrabService _grabService = null;

        /// <summary>
        /// exchange service
        /// </summary>
        private readonly IExchangeService _service = null;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<ExchangeGrabSchedule> _logger = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeGrabSchedule" /> class.
        /// </summary>
        /// <param name="grabService">grab service</param>
        /// <param name="service">exchange service</param>
        /// <param name="logger">logger of exchange grab schedule</param>
        public ExchangeGrabSchedule(IExchangeGrabService grabService, IExchangeService service, ILogger<ExchangeGrabSchedule> logger)
        {
            _service = service;
            _grabService = grabService;
            _logger = logger;
        }

        /// <summary>
        /// grab stock
        /// </summary>
        public void Grab()
        {
            Grab(DateTime.Now);
        }

        /// <summary>
        /// grab stock
        /// </summary>
        /// <param name="date">date</param>
        public void Grab(DateTime date)
        {
            var result = _grabService.GetList(new FinanceApi.Models.Filter.ExchangeFilter()
            {
                Date = date.Date,
            });

            if (result.IsSuccess && result.InnerResult.Count > 0)
            {
                var insertResult = _service.Insert(result.InnerResult);
                _logger.LogInformation($"Date:{date.Date}, InsertCount:{insertResult}");
            }
        }
    }
}