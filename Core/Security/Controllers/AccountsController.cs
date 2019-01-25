using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vega.Controllers.Resources;
using vega.Core.Models;
using vega.Core;
using System.Globalization;
using vega.Extensions.DateTime;
using Microsoft.Extensions.Options;
using vega.Core.Models.Settings;
using vega.Core.Utils;
using Microsoft.AspNetCore.Identity;
using vegaplanner.Core.Models.Security;
using vegaplanner.Core.Models.Security.Helpers;
using vegaplanner.Core.Models.Security.Resources;
using Microsoft.AspNetCore.Http;

using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace vegaplanner.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Route("api/[controller]")]
    public class AccountsController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AccountsController(UserManager<AppUser> userManager, 
                                 RoleManager<IdentityRole> roleManager,
                                    IMapper mapper, 
                                    IUserRepository userRepository,
                                    IUnitOfWork unitOfWork,
                                    IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
            this._roleManager = roleManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        // POST api/accounts
        [Authorize(Policy = "AdminUser")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegistrationResource model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdentity = mapper.Map<AppUser>(model);

            var result = await userManager.CreateAsync(userIdentity, model.Password);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            //userRepository.Add(new InternalAppUser { IdentityId = userIdentity.Id, Location = model.Location });

            //Check if Role specified for user exists
            foreach( string role in model.Roles) {
                bool x = await _roleManager.RoleExistsAsync(role);

                if (!x) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

                result = await userManager.AddToRoleAsync(userIdentity, role);
            }
            await unitOfWork.CompleteAsync();

            return new OkObjectResult("Account created");
        }

        [Authorize(Policy = "AdminUser")]
        [HttpGet("users")]
        public IActionResult Account()
        {

            var users = userManager.Users ; 

            return Ok(users); 
        //Retrieve the user info
        //HttpContext.User
        // var userId = httpContextAccessor.HttpContext.User.Claims.Single(c => c.Type == "id");
        // var user = await userRepository.Get(userId);
        
        // return new OkObjectResult(new
        // {
        //     Message = "This is secure API and user data!",
        //     user.Identity.FirstName,
        //     user.Identity.LastName,
        //     user.Identity.PictureUrl,
        //     user.Identity.FacebookId,
        //     user.Identity.Email,
        //     user.Location,
        //     user.Locale,
        //     user.Gender
        // });
        }

        [Authorize(Policy = "ApiUser")]
        [HttpGet("roleUsers/{role}")]
        public async Task<IActionResult> roleUsers(string role)
        {
            var users = await userManager.GetUsersInRoleAsync(role)   ;  

            var result = mapper.Map<IList<AppUser>, IList<AppUserSelectResource>>(users);

            return Ok(result);
        }
    }

}