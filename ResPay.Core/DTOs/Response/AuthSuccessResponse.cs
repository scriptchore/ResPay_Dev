using System;
using System.Collections.Generic;
using System.Text;

namespace ResPay.Core.DTOs.Response
{
    public class AuthSuccessResponse
    {
        public string Code { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
        public UserDetails User { get; set; }
    }
}
