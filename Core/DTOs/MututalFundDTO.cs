using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    // DTO for Inserting / Updating Mutual Fund
    public class MututalFundDTO
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
        public string AssetManagementCompanyName
        {
            get; set;
        }
        public decimal NetAssetValue
        {
            get; set;
        }
        public decimal Units
        {
            get; set;
        }
        public decimal CurrentNetAssetValue
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
