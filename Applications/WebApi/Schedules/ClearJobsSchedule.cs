using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.MemoryStorage.Dto;
using Hangfire.States;

namespace WebApi.Schedules
{
    /// <summary>
    /// clear jobs schedule
    /// </summary>
    public class ClearJobsSchedule
    {
        /// <summary>
        /// succeeded aggregated and counter key
        /// </summary>
        private static string SucceededStateKey { get; } = "stats:succeeded";

        /// <summary>
        /// clear succeeded jobs
        /// </summary>
        public void ClearSucceededJobs()
        {
            var storage = JobStorage.Current as MemoryStorage;
            if (storage != null)
            {
                var jobs = storage.Data.GetEnumeration<JobDto>().Where(x => x.StateName == SucceededState.StateName).Cast<IIdentifiedData<string>>();
                if (jobs.Count() > 0)
                {
                    storage.Data.Delete(jobs);
                }

                var acc = storage.Data.GetEnumeration<AggregatedCounterDto>().Where(x => x.Key.Contains(SucceededStateKey)).Cast<IIdentifiedData<string>>();
                if (acc.Count() > 0)
                {
                    storage.Data.Delete(acc);
                }

                var counter = storage.Data.GetEnumeration<CounterDto>().FirstOrDefault(x => x.Key.Equals(SucceededStateKey, StringComparison.OrdinalIgnoreCase));
                if (counter != null)
                {
                    counter.Value = storage.Data.GetEnumeration<JobDto>().Where(x => x.StateName == SucceededState.StateName).Count();
                }
            }
        }
    }
}