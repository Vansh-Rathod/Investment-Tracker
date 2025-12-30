using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CommonModels
{
    public class APIResponse<T>
    {
        public bool Success { get; set; } = true;
        public string? Message
        {
            get; set;
        }
        public T? Data
        {
            get; set;
        }
        public List<string>? Errors
        {
            get; set;
        }
        public DateTime? TimeStamp
        {
            get; set;
        } = DateTime.Now;

        public static APIResponse<T> SuccessResponse( T data, string message = "" )
        {
            return new APIResponse<T> { Success = true, Data = data, Message = message };
        }

        public static APIResponse<T> FailureResponse( List<string> errors, string message = "" )
        {
            return new APIResponse<T> { Success = false, Errors = errors, Message = message };
        }
    }
}
