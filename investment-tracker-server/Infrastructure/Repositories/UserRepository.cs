using Core.CommonModels;
using Core.Entities;
using Core.ViewModels;
using Dapper;
using GenericServices.Interfaces;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Enums.Enum;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILoggingService _loggingService;

        public UserRepository( IDbConnectionFactory connectionFactory, ILoggingService loggingService )
        {
            _connectionFactory = connectionFactory;
            _loggingService = loggingService;
        }
        public async Task<DbResponse<List<UserViewModel>>> GetUsers( int userId, bool isActive = true, bool isDeleted = false, int page = 0, int pageSize = 10, string searchText = "", string sortOrder = "DESC", string sortField = "CreatedDate" )
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection(); // return new SqlConnection(_connectionString)

                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);
                parameters.Add("@IsActive", isActive);
                parameters.Add("@IsDeleted", isDeleted);
                parameters.Add("@Page", page);
                parameters.Add("@PageSize", pageSize);
                parameters.Add("@SearchText", searchText ?? "");
                parameters.Add("@SortOrder", sortOrder ?? "ASC");
                parameters.Add("@SortField", sortField ?? "CreatedDate");

                var data = await connection.QueryAsync<UserViewModel>(
                    "GetUsers",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return DbResponse<List<UserViewModel>>.SuccessDbResponse(
                   data.ToList(),
                    "Users fetched successfully"
                );
            }
            catch(Exception ex)
            {
                _loggingService.LogAsync("Failed to fetch users", Core.Enums.Enum.LogLevel.Error, "UserRepository.GetUsers", ex, null);

                return DbResponse<List<UserViewModel>>.FailureDbResponse(
                    new List<UserViewModel>(),
                    new List<string> { "Failed to fetch users ." },
                    "Exception occurred while fetching users"
                );
            }
        }

        public async Task<DbResponse<UserViewModel>> GetUserByEmail( string email, bool isActive = true, bool isDeleted = false )
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection(); // return new SqlConnection(_connectionString)

                var parameters = new DynamicParameters();
                parameters.Add("@Email", email);
                parameters.Add("@IsActive", isActive);
                parameters.Add("@IsDeleted", isDeleted);

                var data = await connection.QueryFirstOrDefaultAsync<UserViewModel>(
                    "GetUserByEmail",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                if(data == null)
                {
                    return DbResponse<UserViewModel>.FailureDbResponse(
                   data,
                    new List<string> { $"User not found by email: {email}" },
                    $"User not found by email: {email}"
                    );
                }

                return DbResponse<UserViewModel>.SuccessDbResponse(
                   data,
                    "User fetched successfully by email"
                );
            }
            catch(Exception ex)
            {
                _loggingService.LogAsync("Exception occurred while fetching user by email", Core.Enums.Enum.LogLevel.Error, "UserRepository.GetUserByEmail", ex, new Dictionary<string, object> { { "Email", email } });

                return DbResponse<UserViewModel>.FailureDbResponse(
                    new UserViewModel(),
                    new List<string> { "Failed to fetch user by email" },
                    "Exception occurred while fetching user by email"
                );
            }
        }

        public async Task<DbResponse<int>> InsertUpdateDeleteUser( User user, OperationType operationType, int userId = 0 )
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var parameters = new DynamicParameters();
                parameters.Add("@OperationType", (int)operationType);
                parameters.Add("@UserId", userId);

                parameters.Add("@Name", user?.Name);
                parameters.Add("@Email", user?.Email);
                parameters.Add("@Password", user?.Password);
                parameters.Add("@PasswordHash", user?.PasswordHash);
                parameters.Add("@PhoneNumber", user?.PhoneNumber);
                parameters.Add("@LastLogin", user?.LastLogin);
                parameters.Add("@IsActive", user?.IsActive ?? true);

                // SP returns UserId for all operations
                var resultUserId = await connection.ExecuteScalarAsync<int>(
                    "InsertUpdateDeleteUser",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return DbResponse<int>.SuccessDbResponse(
                    resultUserId,
                    $"User {operationType} operation completed successfully"
                );
            }
            catch(Exception ex)
            {
                await _loggingService.LogAsync(
                    "Exception occurred while performing Insert/Update/Delete user operation",
                    Core.Enums.Enum.LogLevel.Error,
                    "UserRepository.InsertUpdateDeleteUser",
                    ex,
                    new Dictionary<string, object>
                    {
                        { "OperationType", operationType.ToString() },
                        { "UserId", userId },
                        { "Email", user?.Email }
                     });

                return DbResponse<int>.FailureDbResponse(
                    0,
                    new List<string> { "Failed to perform user operation" },
                    "Exception occurred while processing user data"
                );
            }
        }


    }
}
