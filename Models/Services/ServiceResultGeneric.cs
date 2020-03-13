using System;

namespace FinanceApi.Models.Services
{
    /// <summary>
    /// Filter result structure
    /// </summary>
    /// <typeparam name="T">type of result</typeparam>
    public class ServiceResult<T>
    {
        /// <summary>
        /// Filter result
        /// </summary>
        public T InnerResult { get; set; }

        /// <summary>
        /// Statue
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Exception
        /// </summary>
        public Exception InnerException { get; set; }

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