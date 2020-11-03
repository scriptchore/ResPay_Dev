using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResPay.Core.DTOs;
using ResPay.Core.DTOs.Request;
using ResPay.Core.DTOs.Response;
using ResPay.Core.Interface;
using ResPay.Core.Models;
using ResPay.Core.Utilities;

namespace ResPay.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IUserService _userService; 
        private readonly UserManager<AppUser> _userManager;
        string checkForEmail = "@";

        public UsersController(IUserService userService, UserManager<AppUser> userManager) 
        {
            _userService = userService;
            _userManager = userManager;
           
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromForm] UserRegistrationRequest request)
        {
            try
            {

                var authResponse = await _userService.SignupAsync(request);
                
                if (authResponse.Item1 == StatusMessage.USER_EXIST)
                {
                    ErrorResponse response = new ErrorResponse
                    {
                        Error = new Error
                        {
                            Code = "400",
                            Message = $"User with this email: {request.Email}  already exists"
                        }
                    };
                    return BadRequest(response);
                }

                if (authResponse.Item1 == StatusMessage.NUMBER_EXIST)
                {
                    ErrorResponse response = new ErrorResponse
                    {
                        Error = new Error
                        {
                            Code = "400",
                            Message = $"User with this phone number: {request.phoneNo} already exists"
                        }
                    };
                    return BadRequest(response);
                }


                if (authResponse.Item1 == StatusMessage.PASSWORD_MISMATCH)
                {
                    ErrorResponse response = new ErrorResponse
                    {
                        Error = new Error
                        {
                            Code = "400",
                            Message = "The password does not match"
                        }
                    };
                    return BadRequest(response);
                }

                return Created("", new AuthSuccessResponse
                {
                    Code = "201",
                    Token = authResponse.Item2.Token,
                    User = new UserDetails
                    {
                        Id = authResponse.Item3.ToString(),
                        Firstname = authResponse.Item2.Firstname,
                        Lastname = authResponse.Item2.Lastname,
                        Email = authResponse.Item2.Email
                       
                    }
                });

            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.ToString());
            }
        }

        [HttpPost("signin")]

        public async Task<IActionResult> Signin([FromBody] UserLoginRequest request) 
        {
            var authResponse = await _userService.SigninAsync(request.username, request.password);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Code = "400",
                    Error = authResponse.Error
                });
            }

            if (request.username.Contains(checkForEmail))
            {

                var currentuser = _userManager.Users.FirstOrDefault(x => x.UserName == request.username);

                return Ok(new AuthSuccessResponse
                {
                    Code = "200",
                    Token = authResponse.Token,
                    User = new UserDetails
                    {

                        Id = currentuser.Id,
                        Firstname = authResponse.Firstname,
                        Lastname = authResponse.Lastname,
                        Email = authResponse.Email

                    }
                });

            }
            else
            {
                var currentuser = _userManager.Users.FirstOrDefault(x => x.PhoneNumber == request.username);

                return Ok(new AuthSuccessResponse
                {
                    Code = "200",
                    Token = authResponse.Token,
                    User = new UserDetails
                    {

                        Id = currentuser.Id,
                        Firstname = authResponse.Firstname,
                        Lastname = authResponse.Lastname,
                        Email = authResponse.Email

                    }
                });

            }

        }



    }
}
