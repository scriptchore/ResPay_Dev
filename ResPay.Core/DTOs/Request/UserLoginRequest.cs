using System;
using System.Collections.Generic;
using System.Text;

namespace ResPay.Core.DTOs.Request
{
    public class UserLoginRequest
    {
        public string username { get; set; } 
        public string password { get; set; }
    }
}
