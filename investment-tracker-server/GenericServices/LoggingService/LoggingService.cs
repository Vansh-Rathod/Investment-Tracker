using GenericServices.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GenericServices.LoggingService
{
    public class LoggingService : ILoggingService
    {
        private readonly IConfiguration _configuration;

        public LoggingService( IConfiguration configuration )
        {
            _configuration = configuration;
        }

        public async Task LogAsync( string message, Core.Enums.Enum.LogLevel level = Core.Enums.Enum.LogLevel.Info, string source = "Application", Exception? exception = null, Dictionary<string, object>? additionalData = null )
        {
            try
            {
                // 1. Get log folder path from appsettings.json
                string folderPath = _configuration["LoggerSettings:LogFolderPath"];

                // 2. Ensure the folder exists
                if(!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // 3. Build daily log file name
                string filePath = Path.Combine(folderPath, $"log_{DateTime.Now:dd-MM-yyyy}.txt");

                // 4. Build log object
                var logEntry = new
                {
                    Id = Guid.NewGuid(),
                    Timestamp = DateTime.Now.ToString("o"), // ISO 8601 format
                    Level = level.ToString(),
                    Source = source,
                    Message = message,
                    Exception = exception?.ToString(),
                    StackTrace = exception?.StackTrace,
                    AdditionalData = additionalData

                    // Additional Data can have IPAddress, Device(IPhone, Samsung, etc.)
                };

                // 5. Serialize to pretty JSON
                string logContent = JsonConvert.SerializeObject(logEntry, Newtonsoft.Json.Formatting.Indented);

                // 6. Append log entry to file
                await File.AppendAllTextAsync(filePath, logContent + Environment.NewLine + new string('-', 80) + Environment.NewLine);
            }
            catch(Exception logEx)
            {
                // Fallback in case logging itself fails
                Console.WriteLine("Logging failed: " + logEx.Message);
            }
        }
    }
}
