using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CommonModels
{
    public class DbResponse<T>
    {
        public bool Success
        {
            get; set;
        }
        public T Data
        {
            get; set;
        }
        public string Message
        {
            get; set;
        }
        public List<string> Errors
        {
            get; set;
        }
        public DateTime? TimeStamp
        {
            get; set;
        } = DateTime.Now;

        public static DbResponse<T> SuccessDbResponse( T data, string message = "" )
        {
            return new DbResponse<T> { Success = true, Data = data, Message = message };
        }

        public static DbResponse<T> FailureDbResponse( T data, List<string> errors, string message = "")
        {
            return new DbResponse<T> { Success = false, Data = data, Errors = errors, Message = message };
        }
    }
}
