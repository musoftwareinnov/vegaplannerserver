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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Linq;

namespace vegaplanner.Core.Models.Security
{
  [Authorize(Policy = "ApiUser")]
  [Route("api/[controller]/[action]")]
    public class DashboardController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly UserManager<AppUser> userManager;
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly ClaimsPrincipal _caller;

        public DashboardController(UserManager<AppUser> userManager, 
                                    IMapper mapper, 
                                    IUserRepository userRepository,
                                    IUnitOfWork unitOfWork,
                                    IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.userRepository = userRepository;
            this.unitOfWork = unitOfWork;
            this._caller = httpContextAccessor.HttpContext.User;
        }

    // GET api/dashboard/home
        [HttpGet]
        public async Task<IActionResult> Home()
        {
        // retrieve the user info
        //HttpContext.User
        var userId = _caller.Claims.Single(c => c.Type == "id");
        var customer = await userRepository.Get(userId);
        
        return new OkObjectResult(new
        {
            Message = "This is secure API and user data!",
            customer.Identity.FirstName,
            customer.Identity.LastName,
            customer.Identity.PictureUrl,
            customer.Identity.FacebookId,
            customer.Location,
            customer.Locale,
            customer.Gender
        });
        }
    }
}