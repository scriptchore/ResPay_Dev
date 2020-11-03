using System;
using System.Collections.Generic;
using System.Text;

namespace ResPay.Core.DTOs.Response
{
    public class AuthFailedResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public string Code { get; set; }
        public string Error { get; set; }

    }
}
