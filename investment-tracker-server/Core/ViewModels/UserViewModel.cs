using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class UserViewModel
    {
        public int Id
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        public string Email
        {
            get; set;
        }
        public string Password
        {
            get; set;
        }
        public string PasswordHash
        {
            get; set;
        }
        public string? PhoneNumber
        {
            get; set;
        }
        public DateTime? LastLogin
        {
            get; set;
        }
        public DateTime CreatedDate
        {
            get; set;
        }
        public DateTime? ModifiedDate
        {
            get; set;
        }
        public bool IsActive
        {
            get; set;
        }
        public bool IsDeleted
        {
            get; set;
        }
        public int TotalRecords
        {
            get; set;
        }
    }
}
