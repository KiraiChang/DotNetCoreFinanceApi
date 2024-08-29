using FinanceApi.Interfaces.Services;
using FinanceApi.Interfaces.Services.Grabs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace WebApi.Schedules
{
    /// <summary>
    /// schedule of gold grab
    /// </summary>
    public class GoldGrabSchedule
    {
        /// <summary>
        /// grab service
        /// </summary>
        private readonly IGoldGrabService _grabService = null;

        /// <summary>
        /// Gold service
        /// </summary>
        private readonly IGoldService _service = null;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<GoldGrabSchedule> _logger = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoldGrabSchedule" /> class.
        /// </summary>
        /// <param name="grabService">grab service</param>
        /// <param name="service">Gold service</param>
        /// <param name="logger">logger of Gold grab schedule</param>
        public GoldGrabSchedule(IGoldGrabService grabService, IGoldService service, ILogger<GoldGrabSchedule> logger)
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
            Grab(DateTime.Now.AddDays(-14), DateTime.Now);
        }

        /// <summary>
        /// grab stock
        /// </summary>
        /// <param name="begin">begin date</param>
        /// <param name="end">end date</param>
        public void Grab(DateTime begin, DateTime end)
        {
            var result = _grabService.GetList(new FinanceApi.Models.Filter.GoldFilter()
            {
                BeginDate = begin.Date,
                EndDate = end.Date
            });

            if (result.IsSuccess && result.InnerResult.Count > 0)
            {
                var insertResult = _service.Insert(result.InnerResult);
                _logger.LogInformation($"BeginDate:{begin.Date}, EndDate:{end.Date}, InsertCount:{insertResult}");
            }
        }

        /// <summary>
        /// grab stock
        /// </summary>
        /// <param name="begin">begin date</param>
        /// <param name="end">end date</param>
        public void GrabAll()
        {
            for (var date = new DateTime(2023, 01, 01); date < DateTime.Now; date = date.AddMonths(1))
            {
                var filter = new FinanceApi.Models.Filter.GoldFilter()
                {
                    BeginDate = date.AddDays(-14),
                    EndDate = date
                };
                var result = _grabService.GetList(filter);

                if (result.IsSuccess && result.InnerResult.Count > 0)
                {
                    var insertResult = _service.Insert(result.InnerResult);
                    _logger.LogInformation($"BeginDate:{filter.BeginDate.Value.Date}, EndDate:{filter.EndDate.Value.Date}, InsertCount:{insertResult}");
                }
                Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }
    }
}