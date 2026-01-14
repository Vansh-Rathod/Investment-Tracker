using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class UserEquityViewModel
    {
        public int Id
        {
            get; set;
        }
        public int UserId
        {
            get; set;
        }
        public string UserName
        {
            get; set;
        }
        public string UserEmail
        {
            get; set;
        }
        public string EquityName
        {
            get; set;
        }
        public string CompanyName
        {
            get; set;
        }
        public string? EquityShortForm
        {
            get; set;
        }
        public int EquityType
        {
            get; set;
        }
        public decimal PurchasePrice
        {
            get; set;
        }
        public decimal Quantity
        {
            get; set;
        }
        public decimal InvestedAmount
        {
            get; set;
        }
        public decimal CurrentPrice
        {
            get; set;
        }
        public DateTime InvestmentDate
        {
            get; set;
        }
        public string? OrderId
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
