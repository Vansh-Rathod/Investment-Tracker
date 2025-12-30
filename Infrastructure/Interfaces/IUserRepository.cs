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
    public interface IUserRepository
    {
        Task<DbResponse<List<UserViewModel>>> GetUsers(int userId, bool isActive = true, bool isDeleted = false, int page = 0, int pageSize = 10, string searchText = "", string sortOrder = "DESC", string sortField = "CreatedDate");

        Task<DbResponse<UserViewModel>> GetUserByEmail( string email, bool isActive = true, bool isDeleted = false );

        Task<DbResponse<int>> InsertUpdateDeleteUser( User user, OperationType operationType, int userId = 0 );
    }
}
