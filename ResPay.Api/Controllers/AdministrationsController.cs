using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResPay.Core.DTOs.Request;
using ResPay.Core.Utilities;

namespace ResPay.Api.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdministrationsController : ControllerBase
    {

        private readonly RoleManager<IdentityRole> _roleManager;

        public AdministrationsController(RoleManager<IdentityRole> roleManager)  
        {
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoleAsync([FromForm] CreateRoleRequest request)
        {


            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole()
                {
                    Name = request.RoleName
                };

                IdentityResult result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return Ok();
                }
            }

            ErrorResponse response = new ErrorResponse
            {
                Error = new Error
                {
                    Code = "400",
                    Message = "An error has occured"
                }
            };
            return BadRequest(response);
        }
    }
}
