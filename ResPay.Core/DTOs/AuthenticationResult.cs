using System;
using System.Collections.Generic;
using System.Text;

namespace ResPay.Core.DTOs
{
    public class AuthenticationResult
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PhoneNo { get; set; }

        public string Email { get; set; }
        public string Token { get; set; }
        public bool Success { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public string Error { get; set; }
    }
}
