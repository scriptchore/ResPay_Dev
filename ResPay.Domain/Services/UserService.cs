using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ResPay.Core.DTOs;
using ResPay.Core.Interface;
using ResPay.Core.Models;
using ResPay.Core.Options;
using ResPay.Core.Utilities;
using ResPay.Data.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ResPay.Domain.Services
{
    public class UserService : IUserService
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration configuration;
        private readonly JwtSettings _jwtSettings;
        //private readonly UserToken _userToken;
        private readonly ApplicationDbContext _applicationDbContext;
        //private readonly ILogger<UserService> _logger;
        //private readonly ISmsService _smsService;
        //private readonly IEmailService _emailService;
        string _tokenUser = "";
        string Token = "";


        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration, IOptionsSnapshot<JwtSettings> jwtSettings,
         ApplicationDbContext applicationDbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.configuration = configuration;
            _jwtSettings = jwtSettings.Value;
            //_userToken = userToken;
            _applicationDbContext = applicationDbContext;
            
        }

        public async Task<AuthenticationResult> SigninAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            string Email = "";
            string checkForEmail = "@";



            if (!email.Contains(checkForEmail))
            {
                var PhoneNumber = email;

                var confirmUser = _userManager.Users.FirstOrDefault(x => x.PhoneNumber == email);

                var getEmail = confirmUser.Email;

                Email = getEmail;

                var newUser = await _userManager.FindByEmailAsync(getEmail);

                if (confirmUser == null)
                {
                    return new AuthenticationResult
                    {
                        Errors = new[] { "User does not exist" }
                    };
                }


                var loginWithPhone = await _userManager.CheckPasswordAsync(newUser, password);

                if (!loginWithPhone)
                {
                    return new AuthenticationResult
                    {
                        Error = "User/Password Combination is wrong"
                    };
                }


            }

            else
            {
                var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);

                Email = email;

                if (!userHasValidPassword)

                {
                    return new AuthenticationResult
                    {
                        Error = "User/Password Combination is wrong"
                    };

                }
            }

            var currentuser = _userManager.Users.FirstOrDefault(x => x.UserName == Email);


            return GenerateAuthenticationResultForUser2(currentuser);
        }

        private AuthenticationResult GenerateAuthenticationResultForUser2(AppUser currentuser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var TokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims: new[]
                {
                    new Claim(type: JwtRegisteredClaimNames.Sub, value: currentuser.Email),
                    new Claim(type: JwtRegisteredClaimNames.Jti, value: Guid.NewGuid().ToString()),
                    new Claim("id", value:  currentuser.Id)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), algorithm: SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(TokenDescriptor);

            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                Firstname = currentuser.Firstname,
                Lastname = currentuser.Lastname,
                Email = currentuser.Email,
                PhoneNo = currentuser.PhoneNumber
            };
        }



        public async Task<Tuple<string, AuthenticationResult, string>> SignupAsync(UserRegistrationRequest request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            var currentuser = _userManager.Users.FirstOrDefault(x => x.PhoneNumber == request.phoneNo);

            if (existingUser != null)
            {
                return new Tuple<string, AuthenticationResult, string>(StatusMessage.USER_EXIST, null, null);
            }

            if (currentuser != null)
            {
                return new Tuple<string, AuthenticationResult, string>(StatusMessage.NUMBER_EXIST, null, null);
            }



            if (request.Password != request.confirmPassword)
            {
                return new Tuple<string, AuthenticationResult, string>(StatusMessage.PASSWORD_MISMATCH, null, null);
            }

            //var newUser = new IdentityUser
            var newUser = new AppUser
            {


                Firstname = request.Firstname,
                Lastname = request.Lastname,
                Email = request.Email,
                UserName = request.Email,
                PhoneNumber = request.phoneNo
                

            };

            var createdUser = await _userManager.CreateAsync(newUser, request.Password);


            //newUser.Id




            if (!createdUser.Succeeded)
            {
                return new Tuple<string, AuthenticationResult, string>(StatusMessage.SERVER_ERROR, null, null);
            }



            return new Tuple<string, AuthenticationResult, string>(StatusMessage.OK, GenerateAuthenticationResultForUser(newUser), newUser.Id);
        }

        private AuthenticationResult GenerateAuthenticationResultForUser(AppUser newUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var TokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims: new[]
                {
                    new Claim(type: JwtRegisteredClaimNames.Sub, value: newUser.Email),
                    new Claim(type: JwtRegisteredClaimNames.Jti, value: Guid.NewGuid().ToString()),
                    new Claim(type: "id", value: newUser.Id)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), algorithm: SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(TokenDescriptor);

            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                Firstname = newUser.Firstname,
                Lastname = newUser.Lastname,
                Email = newUser.Email,
                PhoneNo = newUser.PhoneNumber
                
            };
        }
    }
}
