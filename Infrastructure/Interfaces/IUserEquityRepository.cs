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
        DateTime fromDate,
        DateTime toDate,
        int equityType,
        int page,
        int pageSize,
        string searchText,
        string sortOrder,
        string sortField,
        bool isActive,
        bool isDeleted
    );

        Task<DbResponse<int>> InsertUpdateDeleteUserEquity( UserEquity equity, OperationType operationType, int id = 0 );

    }
}
