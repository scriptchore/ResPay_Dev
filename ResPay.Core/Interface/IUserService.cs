using ResPay.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ResPay.Core.Interface
{
    public interface IUserService
    {
        Task<Tuple<string, AuthenticationResult, string>> SignupAsync(UserRegistrationRequest request);

        Task<AuthenticationResult> SigninAsync(string email, string password);


    }
}
