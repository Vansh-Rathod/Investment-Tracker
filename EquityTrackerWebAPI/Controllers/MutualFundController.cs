using Core.CommonModels;
using Core.DTOs;
using Core.Entities;
using Core.ViewModels;
using GenericServices.Interfaces;
using Infrastructure.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Core.Enums.Enum;

namespace EquityTrackerWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MutualFundController : ControllerBase
    {
        private readonly IUserEquityRepository _userEquityRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILoggingService _loggingService;

        public MutualFundController( IUserEquityRepository userEquityRepository, IUserRepository userRepository, ILoggingService loggingService )
        {
            _userEquityRepository = userEquityRepository;
            _userRepository = userRepository;
            _loggingService = loggingService;
        }

        /// <summary>
        /// Get all mutual fund investments with pagination, search & sorting.
        /// </summary>
        [HttpGet("GetMutualFunds")]
        public async Task<APIResponse<List<UserEquityViewModel>>> GetMutualFunds(
            int userId = 0,
            int page = 1,
            int pageSize = 10,
            string searchText = "",
            string sortOrder = "DESC",
            string sortField = "CreatedDate",
            bool isActive = true,
            bool isDeleted = false,
            string fromDate = null,
            string toDate = null )
        {
            var response = new APIResponse<List<UserEquityViewModel>>();
            try
            {
                if(userId == 0)
                {
                    return APIResponse<List<UserEquityViewModel>>.FailureResponse(
                       new List<string> { "Validation Failed" },
                       "Please provide user id greater than 0"
                    );
                }

                var userResult = await _userRepository.GetUsers(
                    userId,
                    true,
                    false,
                    1,
                    10,
                    "",
                    "DESC",
                    "CreatedDate"
                );

                if(!userResult.Success || !userResult.Data.Any())
                {
                    return APIResponse<List<UserEquityViewModel>>.FailureResponse(
                       new List<string> { "User Not found" },
                       $"User not found by Id: {userId}"
                   );
                }

                // Default to last 1 day (24 hours)
                DateTime defaultFrom = DateTime.Now.AddDays(-1);
                DateTime defaultTo = DateTime.Now;

                // Convert strings → DateTime
                DateTime fromDt = string.IsNullOrWhiteSpace(fromDate)
                    ? defaultFrom
                    : DateTime.Parse(fromDate);

                DateTime toDt = string.IsNullOrWhiteSpace(toDate)
                    ? defaultTo
                    : DateTime.Parse(toDate);

                var result = await _userEquityRepository.GetUserEquities(
                    userId,
                    fromDt,
                    toDt,
                    (int)Core.Enums.Enum.EquityType.MutualFunds,
                    page,
                    pageSize,
                    searchText,
                    sortOrder,
                    sortField,
                    isActive,
                    isDeleted
                );

                if(!result.Success)
                {
                    return APIResponse<List<UserEquityViewModel>>.FailureResponse(
                       new List<string> { "Something went wrong while fetching mututal funds" },
                       $"Failed to fetch mutual funds for user Id: {userId}"
                   );
                }

                return APIResponse<List<UserEquityViewModel>>.SuccessResponse(
                    result.Data,
                    "Mututal Funds fetched successfully"
                 );
            }
            catch(Exception ex)
            {
                _loggingService.LogAsync("An error occurred while fetching mutual funds", Core.Enums.Enum.LogLevel.Error, "MutualFundController.GetMutualFunds", ex, null);

                return APIResponse<List<UserEquityViewModel>>.FailureResponse(
                       new List<string> { "Internal Server Error" },
                       "An error occurred while fetching mutual funds. Please try again later."
                   );
            }
        }

        [HttpPost("InserUpdateMututalFund")]
        public async Task<APIResponse<int>> InserUpdateMututalFund( [FromBody] MututalFundDTO model )
        {
            try
            {
                var userIdClaim = User.FindFirst("userId")?.Value;
                if(string.IsNullOrWhiteSpace(userIdClaim))
                {
                    return APIResponse<int>.FailureResponse(
                           new List<string> { "Token is invalid" },
                           "Token is Invalid or Forbidden. Cannot find User Id"
                       );
                }

                if(!int.TryParse(userIdClaim, out int userId))
                {
                    return APIResponse<int>.FailureResponse(
                        new List<string> { "Invalid User Id in token" },
                        "User Id claim is not a valid integer"
                    );
                }

                int equityId = model?.Id ?? 0;

                OperationType operationType = equityId > 0 ? OperationType.UPDATE : OperationType.INSERT;

                UserEquity stockEquity = new UserEquity
                {
                    Id = equityId, // 0 -> Create User Mutual Fund Equity, >0 -> Update User Mutual Fund Equity
                    UserId = userId,
                    EquityName = model?.Name,
                    EquityShortForm = model?.ShortForm,
                    EquityType = (int)EquityType.MutualFunds,
                    PurchasePrice = model?.PurchasePrice ?? 0,
                    Quantity = model?.Quantity ?? 0,
                    InvestedAmount = (model?.PurchasePrice * model?.Quantity) ?? 0,
                    CurrentPrice = model?.CurrentPrice ?? 0,
                    InvestmentDate = model?.InvestmentDate ?? DateTime.Now,
                    OrderId = model?.OrderId,
                    IsActive = true,
                    CompanyName = model?.CompanyName
                };

                var result = await _userEquityRepository.InsertUpdateDeleteUserEquity(
                    stockEquity,
                    operationType,
                    equityId
                );

                if(!result.Success && (result.Data <= 0 || result.Data == null))
                {
                    return APIResponse<int>.FailureResponse(
                           new List<string> { "Failed to add/update mutual fund" },
                           "An error occurred while adding/updating mutual fund. Please try again later."
                       );
                }

                return APIResponse<int>.SuccessResponse(
                        result.Data,
                        "Mutual Fund Inserted/Updated Successfully"
                     );
            }
            catch(Exception ex)
            {
                _loggingService.LogAsync("Exception occurred while Insert/Update Mutual Fund", Core.Enums.Enum.LogLevel.Critical, "MutualFundController.InserUpdateMututalFund", ex, new Dictionary<string, object> { { "MutualFundDTO", model } });

                return APIResponse<int>.FailureResponse(
                    new List<string> { "Internal Server Error" },
                    "An error occurred while inserting/updating Mutual Fund. Please try again later."
                );
            }
        }

        [HttpPost("DeleteMutualFund")]
        public async Task<APIResponse<int>> DeleteMutualFund( int equityId )
        {
            try
            {
                var userIdClaim = User.FindFirst("userId")?.Value;
                if(string.IsNullOrWhiteSpace(userIdClaim))
                {
                    return APIResponse<int>.FailureResponse(
                           new List<string> { "Token is invalid" },
                           "Token is Invalid or Forbidden. Cannot find User Id"
                       );
                }

                if(!int.TryParse(userIdClaim, out int userId))
                {
                    return APIResponse<int>.FailureResponse(
                        new List<string> { "Invalid User Id in token" },
                        "User Id claim is not a valid integer"
                    );
                }

                UserEquity stockEquity = new UserEquity
                {
                    Id = equityId,
                    UserId = userId
                };

                var result = await _userEquityRepository.InsertUpdateDeleteUserEquity(
                    stockEquity,
                    OperationType.DELETE,
                    equityId
                );

                if(!result.Success && (result.Data <= 0 || result.Data == null))
                {
                    return APIResponse<int>.FailureResponse(
                           new List<string> { "Failed to delete mutual fund" },
                           "An error occurred while deleting mutual fund. Please try again later."
                       );
                }

                return APIResponse<int>.SuccessResponse(
                        result.Data,
                        "Mutual Fund Deleted Successfully"
                     );
            }
            catch(Exception ex)
            {
                _loggingService.LogAsync("Exception occurred while Deleting Mutual Fund", Core.Enums.Enum.LogLevel.Critical, "MutualFundController.DeleteMutualFund", ex, new Dictionary<string, object> { { "EquityId", equityId } });

                return APIResponse<int>.FailureResponse(
                    new List<string> { "Internal Server Error" },
                    "An error occurred while deleting Mutual Fund. Please try again later."
                );
            }
        }

    }
}
