using FinanceApi.Interfaces.Repositories;
using FinanceApi.Interfaces.Services;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Filter;
using FinanceApi.Models.Services;
using System;
using System.Collections.Generic;

namespace FinanceApi.Services
{
    /// <summary>
    /// Implement IGoldService
    /// </summary>
    /// <seealso cref="FinanceApi.Interfaces.Services.IGoldService" />
    public class GoldService : IGoldService
    {
        /// <summary>
        /// Gold Repository
        /// </summary>
        private IGoldRepo _repo = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoldService" /> class.
        /// </summary>
        /// <param name="repo">stock repository</param>
        public GoldService(IGoldRepo repo)
        {
            _repo = repo;
        }

        /// <inheritdoc />
        public ServiceResult<IList<Gold>> GetList(GoldFilter filter)
        {
            var result = new ServiceResult<IList<Gold>>();
            try
            {
                result.InnerResult = _repo.GetList(filter);
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.InnerException = ex;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        /// <inheritdoc />
        public ServiceResult<int> Insert(IList<Gold> values)
        {
            var result = new ServiceResult<int>();
            try
            {
                result.InnerResult = _repo.Insert(values);
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.InnerException = ex;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
    }
}