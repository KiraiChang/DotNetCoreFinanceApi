using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceApi.Controllers.Api;
using FinanceApi.Models.Services;

namespace WebApi.Models
{
    public class ApiResult<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResult" /> class.
        /// <param name="result">Result</param>
        /// </summary>
        public ApiResult(ServiceResult<T> result)
        {
            InnerResult = result.InnerResult;
            IsSuccess = result.IsSuccess;
            ErrorCode = result.ErrorCode;
            ErrorMessage = result.ErrorMessage;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResult" /> class.
        /// </summary>
        public ApiResult()
        {
        }

        /// <summary>
        /// Filter result
        /// </summary>
        public T InnerResult { get; set; }

        /// <summary>
        /// Statue
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Error code
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// Error Message
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}