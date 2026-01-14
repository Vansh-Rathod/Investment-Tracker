using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.CommonFunctions
{
    public class CommonFunctions
    {
        public static string GenerateOtpCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString(); // returns a 6-digit string
        }
    }
}
