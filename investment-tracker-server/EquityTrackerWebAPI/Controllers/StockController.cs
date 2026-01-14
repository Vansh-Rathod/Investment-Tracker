using Core.CommonModels;
using Core.DTOs;
using Core.Entities;
using Core.ViewModels;
using GenericServices.Interfaces;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Reflection;
using static Core.Enums.Enum;

namespace EquityTrackerWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StockController : ControllerBase
    {
        private readonly IUserEquityRepository _userEquityRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILoggingService _loggingService;

        public StockController( IUserEquityRepository userEquityRepository, IUserRepository userRepository, ILoggingService loggingService )
        {
            _userEquityRepository = userEquityRepository;
            _userRepository = userRepository;
            _loggingService = loggingService;
        }

        /// <summary>
        /// Get all stock investments with pagination, search & sorting.
        /// </summary>
        [HttpGet("GetStocks")]
        public async Task<APIResponse<List<UserEquityViewModel>>> GetStocks(
            int userId = 0,
            int equityId = 0,
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
                    equityId,
                    (int)Core.Enums.Enum.EquityType.Stocks,
                    page,
                    pageSize,
                    searchText,
                    sortOrder,
                    sortField,
                    isActive,
                    isDeleted,
                    fromDt,
                    toDt
                );

                if(!result.Success)
                {
                    return APIResponse<List<UserEquityViewModel>>.FailureResponse(
                       new List<string> { "Something went wrong while fetching stocks" },
                       $"Failed to fetch stocks for user Id: {userId}"
                   );
                }

                return APIResponse<List<UserEquityViewModel>>.SuccessResponse(
                    result.Data,
                    "Stocks fetched successfully"
                 );
            }
            catch(Exception ex)
            {
                _loggingService.LogAsync("An error occurred while fetching stocks", Core.Enums.Enum.LogLevel.Critical, "StockController.GetStocks", ex, null);

                return APIResponse<List<UserEquityViewModel>>.FailureResponse(
                       new List<string> { "Internal Server Error" },
                       "An error occurred while fetching stocks. Please try again later."
                   );
            }
        }

        [HttpPost("InserUpdateStock")]
        public async Task<APIResponse<int>> InserUpdateStock( [FromBody] StockDTO model )
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

                // If user id is 0 then dont allow to create user equity
                if (userId == 0)
                {
                    return APIResponse<int>.FailureResponse(
                           new List<string> { "Invalid User Id" },
                           $"UserId: {userId} is not valid for creating/updating a stock"
                       );
                }

                int equityId = model?.Id ?? 0;


                if (equityId > 0)
                {
                    var userEquityResponse = await _userEquityRepository.GetUserEquities(userId, equityId, (int)EquityType.Stocks);

                    if (userEquityResponse.Data == null || !userEquityResponse.Data.Any())
                    {
                        return APIResponse<int>.FailureResponse(
                           new List<string> { "Stock not found" },
                           $"Stock not found by Id: {equityId}"
                       );
                    }
                }

                OperationType operationType = equityId > 0 ? OperationType.UPDATE : OperationType.INSERT;

                UserEquity stockEquity = new UserEquity
                {
                    Id = equityId, // 0 -> Create User Stock Equity, >0 -> Update User Stock Equity
                    UserId = userId,
                    EquityName = model?.Name,
                    EquityShortForm = model?.ShortForm,
                    EquityType = (int)EquityType.Stocks,
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
                           new List<string> { "Failed to add/update stock" },
                           "An error occurred while adding/updating stock. Please try again later."
                       );
                }

                return APIResponse<int>.SuccessResponse(
                        result.Data,
                        "Stocks Inserted/Updated Successfully"
                     );
            }
            catch(Exception ex)
            {
                _loggingService.LogAsync("Exception occurred while Insert/Update Stock", Core.Enums.Enum.LogLevel.Critical, "StockController.InserUpdateStock", ex, new Dictionary<string, object> { { "StockDTO", model } });

                return APIResponse<int>.FailureResponse(
                    new List<string> { "Internal Server Error" },
                    "An error occurred while inserting/updating stock. Please try again later."
                );
            }
        }

        [HttpPost("DeleteStock")]
        public async Task<APIResponse<int>> DeleteStock( int equityId )
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

                // If user id is 0 then dont allow to create user equity
                if (userId == 0)
                {
                    return APIResponse<int>.FailureResponse(
                           new List<string> { "Invalid User Id" },
                           $"UserId: {userId} is not valid for deleting a stock"
                       );
                }

                if (equityId <= 0)
                {
                    return APIResponse<int>.FailureResponse(
                           new List<string> { "Invalid Equity Id" },
                           $"EquityId: {equityId} is not valid for deleting a stock"
                       );
                }

                var userEquityResponse = await _userEquityRepository.GetUserEquities(userId, equityId, (int)EquityType.Stocks);

                if (userEquityResponse.Data == null || !userEquityResponse.Data.Any())
                {
                    return APIResponse<int>.FailureResponse(
                       new List<string> { "Stock not found" },
                       $"Stock not found by Id: {equityId}"
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
                           new List<string> { "Failed to delete stock" },
                           "An error occurred while deleting stock. Please try again later."
                       );
                }

                return APIResponse<int>.SuccessResponse(
                        result.Data,
                        "Stock Deleted Successfully"
                     );
            }
            catch(Exception ex)
            {
                _loggingService.LogAsync("Exception occurred while Deleting Stock", Core.Enums.Enum.LogLevel.Critical, "StockController.DeleteStockEquity", ex, new Dictionary<string, object> { { "EquityId", equityId } });

                return APIResponse<int>.FailureResponse(
                    new List<string> { "Internal Server Error" },
                    "An error occurred while deleting stock. Please try again later."
                );
            }
        }

    }
}
