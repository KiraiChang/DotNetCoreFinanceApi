using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.Dashboard;

namespace WebApi.Authorization
{
    /// <summary>
    /// No need check Authorization
    /// </summary>
    public class NeedlessDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        /// <inheritdoc cref="IDashboardAuthorizationFilter.Authorize"/>
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}