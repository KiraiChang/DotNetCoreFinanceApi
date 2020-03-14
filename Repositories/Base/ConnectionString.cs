using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApi.Repositories.Base
{
    /// <summary>
    /// Connection Setting
    /// </summary>
    public class ConnectionSetting
    {
        /// <summary>
        /// Connection String
        /// </summary>
        public string Connection { get; set; }

        /// <summary>
        /// Connection Type
        /// </summary>
        public string Type { get; set; }
    }
}