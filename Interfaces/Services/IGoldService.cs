﻿using FinanceApi.Models.Entity;
using FinanceApi.Models.Filter;
using FinanceApi.Models.Services;
using System.Collections.Generic;

namespace FinanceApi.Interfaces.Services
{
    /// <summary>
    /// gold service
    /// </summary>
    public interface IGoldService
    {
        /// <summary>
        /// Get gold list
        /// </summary>
        /// <param name="filter">filter condition</param>
        /// <returns>service result of gold list</returns>
        ServiceResult<IList<Gold>> GetList(GoldFilter filter);

        /// <summary>
        /// Insert into db
        /// </summary>
        /// <param name="values">list of gold</param>
        /// <returns>service result of effect count</returns>
        ServiceResult<int> Insert(IList<Gold> values);
    }
}