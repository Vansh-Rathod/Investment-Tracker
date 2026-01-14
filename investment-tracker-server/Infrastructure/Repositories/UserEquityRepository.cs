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
    public class UserEquityRepository : IUserEquityRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly ILoggingService _loggingService;

        public UserEquityRepository( IDbConnectionFactory connectionFactory, ILoggingService loggingService )
        {
            _connectionFactory = connectionFactory;
            _loggingService = loggingService;
        }

        public async Task<DbResponse<List<UserEquityViewModel>>> GetUserEquities( int userId, int equityId = 0, int equityType = 0, int page = 1, int pageSize = 10, string searchText = "", string sortOrder = "DESC", string sortField = "CreatedDate", bool isActive = true, bool isDeleted = false, DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);
                parameters.Add("@EquityId", equityId);
                parameters.Add("@Page", page);
                parameters.Add("@PageSize", pageSize);
                parameters.Add("@SearchText", searchText);
                parameters.Add("@SortOrder", sortOrder);
                parameters.Add("@SortField", sortField);
                parameters.Add("@IsActive", isActive);
                parameters.Add("@IsDeleted", isDeleted);
                parameters.Add("@EquityType", equityType);
                parameters.Add("@FromDate", fromDate);
                parameters.Add("@ToDate", toDate);

                var data = await connection.QueryAsync<UserEquityViewModel>(
                    "GetUserEquities",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                return DbResponse<List<UserEquityViewModel>>.SuccessDbResponse(
                    data.ToList(),
                   $"User equities fetched successfully"
               );
            }
            catch(Exception ex )
            {
                _loggingService.LogAsync("Failed to fetch user equities", Core.Enums.Enum.LogLevel.Error, "UserEquityRepository.GetUserEquities", ex, null);

                return DbResponse<List<UserEquityViewModel>>.FailureDbResponse(
                    new List<UserEquityViewModel>(),
                    new List<string> { "Failed to fetch user equities." },
                    "Exception occurred while fetching user equities"
                );
            }
            
        }

        public async Task<DbResponse<int>> InsertUpdateDeleteUserEquity(UserEquity equity, OperationType operationType, int id = 0 )
        {
            try
            {
                using var connection = _connectionFactory.CreateConnection();

                var parameters = new DynamicParameters();
                parameters.Add("@OperationType", (int)operationType);
                parameters.Add("@Id", id);
                parameters.Add("@UserId", equity.UserId);

                parameters.Add("@EquityName", equity.EquityName);
                parameters.Add("@EquityShortForm", equity.EquityShortForm);
                parameters.Add("@EquityType", equity.EquityType);
                parameters.Add("@PurchasePrice", equity.PurchasePrice);
                parameters.Add("@Quantity", equity.Quantity);
                parameters.Add("@InvestedAmount", equity.InvestedAmount);
                parameters.Add("@CurrentPrice", equity.CurrentPrice);
                parameters.Add("@InvestmentDate", equity.InvestmentDate);
                parameters.Add("@OrderId", equity.OrderId);
                parameters.Add("@CompanyName", equity.CompanyName);
                parameters.Add("@IsActive", equity.IsActive);

                var equityId = await connection.ExecuteScalarAsync<int>(
                    "InsertUpdateDeleteUserEquity",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                if(equityId > 0)
                {
                    return DbResponse<int>.SuccessDbResponse(
                    equityId,
                    $"User equity {operationType} operation successful"
                );
                }

                return DbResponse<int>.FailureDbResponse(
                    equityId,
                    new List<string> { "Failed to perform user equity operation" },
                    $"Failed to {operationType} User equity"
                );
            }
            catch(Exception ex)
            {
                await _loggingService.LogAsync(
                    "Exception occurred while performing user equity operation",
                    Core.Enums.Enum.LogLevel.Error,
                    "UserEquityRepository.InsertUpdateDeleteUserEquity",
                    ex,
                    new Dictionary<string, object>
                    {
                { "OperationType", operationType.ToString() },
                { "UserId", equity?.UserId },
                { "EquityId", id }
                    });

                return DbResponse<int>.FailureDbResponse(
                    -1,
                    new List<string> { "Failed to perform user equity operation" },
                    "Exception occurred while processing equity request"
                );
            }
        }


    }
}
