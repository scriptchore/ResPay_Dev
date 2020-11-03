using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ResPay.Core.DTOs.Request
{
    public class CreateRoleRequest
    {
        [Required] 
        public string RoleName { get; set; } 
    }
}
