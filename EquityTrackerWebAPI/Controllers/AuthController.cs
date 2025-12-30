using Azure;
using Core.CommonModels;
using Core.DTOs;
using Core.Entities;
using Core.ViewModels;
using EquityTrackerWebAPI.Services;
using GenericServices.Interfaces;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;

namespace EquityTrackerWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenService _jwtTokenService;
        private readonly ILoggingService _loggingService;

        public AuthController( IUserRepository userRepository, JwtTokenService jwtTokenService, ILoggingService loggingService )
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
            //_appBaseUrl = configuration["AppSettings:APP_BASE_URL"];
            _loggingService = loggingService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<APIResponse<object>> Login( [FromBody] LoginRequestDTO model )
        {
            //var response = new APIResponse<object>();
            try
            {
                // Step 1: Validate input
                if(string.IsNullOrEmpty(model.Email.Trim()) || string.IsNullOrEmpty(model.Password.Trim()))
                {
                    return APIResponse<object>.FailureResponse(
                        new List<string> { "Validation Error" },
                        "Email and Password are required"
                    );
                }

                // Step 2: Check if user exists
                var userResult = await _userRepository.GetUserByEmail(model.Email.Trim(), true, false);
                if(!userResult.Success || userResult.Data == null)
                {

                    return APIResponse<object>.FailureResponse(
                        new List<string> { "Validation Failed" },
                        "Invalid email or password"
                    );

                }

                // Step 3: Check password
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(model.Password.Trim(), userResult.Data.PasswordHash);
                if(!isPasswordValid)
                {
                    return APIResponse<object>.FailureResponse(
                        new List<string> { "Validation Failed" },
                        "Invalid email or password"
                    );
                }

                //// Step 5: Check if 2FA is enabled
                //if(userResult.Data.Is2FAEnabled)
                //{
                //    // Clear old OTPs
                //    await _userOtpRepository.DeleteAllOtpsByUserIdAsync(userResult.Data.Id);

                //    // Generate new OTP
                //    string otpCode = GenerateOtpCode();

                //    // Save OTP to DB
                //    var otpModel = new UserOTPModel
                //    {
                //        UserId = userResult.Data.Id,
                //        OtpCode = otpCode,
                //        ExpiryTime = DateTime.UtcNow.AddMinutes(10),
                //        AttemptCount = 0
                //    };

                //    await _userOtpRepository.SaveOtpAsync(otpModel);

                //    // Send OTP via email
                //    //await _emailService.SendEmailAsync(userResult.Data.Email, "2FA Requests OTP For Login", GenerateBody(otpCode, userResult.Data.FullName));

                //    var htmlBodyResult = _emailTemplateService.GenerateOTPVerificationEmailTemplate(otpCode, userResult.Data.FullName);
                //    await _emailService.SendEmailAsync(userResult.Data.Email, "2FA Requests OTP For Login", htmlBodyResult.Data, null);

                //    var twoFAResposneData = new
                //    {
                //        Status = "2FA_REQUIRED",
                //        Message = "Two-factor authentication is enabled. Please verify using the OTP sent to your email.",
                //        UserId = userResult.Data.Id
                //    };

                //    //return Ok(new APIResponse { Status = 302, Message = "2FA required", Data = twoFAResposneData });

                //    //Return 2FA - required response
                //    return CommonResponse<object>.SuccessResponse(
                //        new
                //        {
                //            Status = "2FA_REQUIRED",
                //            Message = "Two-factor authentication is enabled. Please verify using the OTP sent to your email.",
                //            UserId = userResult.Data.Id
                //        },
                //        "2FA required"
                //    );

                //}

                // Step 4: Update Last Login
                User user = new User()
                {
                    Name = userResult.Data.Name,
                    Email = userResult.Data.Email,
                    Password = userResult.Data.Password,
                    PasswordHash = userResult.Data.PasswordHash,
                    PhoneNumber = userResult.Data.PhoneNumber,
                    IsActive = userResult.Data.IsActive,
                    LastLogin = DateTime.UtcNow,
                };

                var updateResult = await _userRepository.InsertUpdateDeleteUser(user, Core.Enums.Enum.OperationType.UPDATE, userResult.Data.Id);
                //if(!updateResult.Success && updateResult.Data <= 0)
                //{
                    
                //}

                // Step 5: Generate token
                var jwtToken = _jwtTokenService.GenerateJwtToken(userResult.Data);
                var refreshToken = _jwtTokenService.GenerateRefreshToken();

                //// Step 6: Get all existing refresh tokens by userId
                //var existingRefreshTokensResult = await _refreshTokenRepository.GetRefreshTokensByUserIdAsync(userResult.Data.Id);

                //if(existingRefreshTokensResult.Success && existingRefreshTokensResult.Data.Items.Any())
                //{
                //    // Step 7: Revoke old tokens
                //    foreach(var token in existingRefreshTokensResult.Data.Items.Where(t => !t.IsRevoked))
                //    {
                //        token.IsRevoked = true;
                //        //token.ExpiresAt = DateTime.UtcNow; // Optional: expire immediately
                //        await _refreshTokenRepository.UpdateRefreshTokenAsync(token);
                //    }
                //}

                //// Step 8: Save the refreh token in the database
                //await _refreshTokenRepository.SaveRefreshTokenAsync(new RefreshTokenModel
                //{
                //    RefreshToken = refreshToken,
                //    UserId = userResult.Data.Id,
                //    CreatedAt = DateTime.UtcNow,
                //    ExpiresAt = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:RefreshTokenExpireDays"]))
                //});

                //// Step 10: Update last login timestamp
                //userResult.Data.LastLogin = DateTime.UtcNow;
                //await _userRepository.UpdateUserInDBAsync(userResult.Data);

                var userLastLogin = userResult.Data.LastLogin == null ? DateTime.UtcNow : userResult.Data.LastLogin;

                var responseData = new
                {
                    Token = jwtToken,
                    RefreshToken = refreshToken,
                    User = new
                    {
                        userResult.Data.Id,
                        userResult.Data.Name,
                        userResult.Data.Email,
                        userResult.Data.PhoneNumber,
                        userLastLogin
                    }
                };

                return APIResponse<object>.SuccessResponse(
                    responseData,
                    "Login Successfull"
                 );
            }
            catch(Exception ex)
            {
                _loggingService.LogAsync("Exception occurred while login", Core.Enums.Enum.LogLevel.Critical, "AuthController.Login", ex, new Dictionary<string, object> { { "Email", model.Email }, { "Password", model.Password } });

                return APIResponse<object>.FailureResponse(
                    new List<string> { "Internal Server Error" },
                    "An error occurred during login. Please try again later."
                );
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<APIResponse<object>> Register( [FromBody] RegisterRequestDTO model )
        {
            try
            {
                // Step-1: Validations
                if(string.IsNullOrEmpty(model.Email.Trim()) ||
                    string.IsNullOrEmpty(model.Password.Trim()) ||
                    string.IsNullOrEmpty(model.Name.Trim()))
                {
                    return APIResponse<object>.FailureResponse(
                       new List<string> { "Required fields are missing" },
                       "Email, Password & Name are required"
                   );
                }

                // Step 3: Check if user exists
                var existingUserResult = await _userRepository.GetUserByEmail(model.Email.Trim());
                if(existingUserResult.Success)
                {
                    return APIResponse<object>.FailureResponse(
                       new List<string> { "User with email already exists" },
                       "Duplicate email"
                   );
                }

                // Step 4: Hash password (replace with real hashing)
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password.Trim());

                // Step 5: Create new user
                var newUser = new User
                {
                    Name = model.Name.Trim(),
                    Email = model.Email.Trim(),
                    Password = model.Password.Trim(),
                    PasswordHash = hashedPassword,
                    PhoneNumber = model?.PhoneNumber.Trim(),
                    IsActive = true
                };

                // Step 7: Save user to DB
                var isCreated = await _userRepository.InsertUpdateDeleteUser(newUser, Core.Enums.Enum.OperationType.INSERT, 0);
                if(!isCreated.Success)
                {
                    return APIResponse<object>.FailureResponse(
                       new List<string> { "User could not be saved to the database" },
                       "User Registration failed"
                   );
                }

                var profileInfo = new
                {
                    UserId = isCreated.Data,
                    Name = newUser.Name.Trim(),
                    Email = newUser.Email.Trim(),
                    PhoneNumber = newUser.PhoneNumber.Trim(),
                    IsActive = newUser.IsActive,
                };

                return APIResponse<object>.SuccessResponse(
                    profileInfo,
                    "Registration Successfull"
                 );
            }
            catch(Exception ex)
            {
                _loggingService.LogAsync("Exception occurred while registration", Core.Enums.Enum.LogLevel.Critical, "AuthController.Register", ex, new Dictionary<string, object> { { "Name", model.Name.Trim() }, { "Email", model.Email.Trim() } });

                return APIResponse<object>.FailureResponse(
                    new List<string> { "Internal Server Error" },
                    "An error occurred during registration. Please try again later."
                );
            }
        }

    }
}
