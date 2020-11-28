using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceApi.Controllers.Api;
using FinanceApi.Models.Services;

namespace WebApi.Models
{
    public class ApiResult : ApiResult<bool>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResult" /> class.
        /// </summary>
        public ApiResult(ServiceResult result) : base(result)
        {
        }

        public ApiResult() : base()
        {
        }
    }
}