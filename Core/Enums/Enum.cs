using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Enums
{
    public class Enum
    {
        public enum LogLevel
        {
            Trace = 1,
            Debug = 2,
            Info = 3,
            Warning = 4,
            Error = 5,
            Critical = 6,
            Success = 7
        }
        public enum EquityType
        {
            [Description("Stocks")]
            Stocks = 1,

            [Description("Mutual Funds")]
            MutualFunds = 2,

            [Description("Exchange Traded Funds")]
            ETFs = 3
        }

        public enum OperationType
        {
            [Description("Insert")]
            INSERT = 1,

            [Description("Update")]
            UPDATE = 2,

            [Description("Delete")]
            DELETE = 3
        }
    }
}
