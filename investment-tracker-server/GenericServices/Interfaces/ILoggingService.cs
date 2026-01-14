using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Enums.Enum;

namespace GenericServices.Interfaces
{
    public interface ILoggingService
    {
        Task LogAsync( string message,
                 LogLevel level = LogLevel.Info,
                 string source = "Application",
                 Exception? exception = null,
                 Dictionary<string, object>? additionalData = null );
    }
}
