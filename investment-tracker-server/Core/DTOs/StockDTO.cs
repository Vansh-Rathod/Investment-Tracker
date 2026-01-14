using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class StockDTO
    {
        public int? Id
        {
            get; set;
        }
        public string Name
        {
            get; set;
        }
        public string ShortForm
        {
            get; set;
        }
        public string CompanyName
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
        public bool IsActive
        {
            get; set;
        }
    }
}
