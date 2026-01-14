using Core.CommonModels;
using Core.Entities;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Enums.Enum;

namespace Infrastructure.Interfaces
{
    public interface IUserEquityRepository
    {
        Task<DbResponse<List<UserEquityViewModel>>> GetUserEquities(
        int userId,
        int equityId = 0,
        int equityType = 0,
        int page = 1,
        int pageSize = 20,
        string searchText = "",
        string sortOrder = "DESC",
        string sortField = "CreatedDate",
        bool isActive = true,
        bool isDeleted = false,
        DateTime? fromDate = null,
        DateTime? toDate = null
    );

        Task<DbResponse<int>> InsertUpdateDeleteUserEquity( UserEquity equity, OperationType operationType, int id = 0 );

    }
}
