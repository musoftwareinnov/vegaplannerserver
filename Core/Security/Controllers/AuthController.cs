using System.Security.Claims;
using System.Threading.Tasks;
using vegaplanner.Core.Models.Security.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using vegaplanner.Core.Models.Security.JWT;
using vegaplanner.Core.Models.Security.Helpers;
using vegaplanner.Core.Models.Security.Resources;
using AutoMapper;
using vegaplannerserver.Core;
using vega.Extensions.DateTime;
using System.Collections.Generic;
using System.Linq;
using vega.Services.Interfaces;

namespace vegaplanner.Core.Models.Security.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        public IMapper Mapper;
        private readonly IUserRepository userRepository;
        IDateService dateService;
        public AuthController(UserManager<AppUser> userManager,
                                RoleManager<IdentityRole> roleManager,
                                IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions,
                                IUserRepository userRepository, IMapper mapper,
                                IDateService dateService)
        {
            this.userRepository = userRepository;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            this.Mapper = mapper;
            this.dateService = dateService;
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody]CredentialsResource credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password);
            if (identity == null)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", ModelState));
            }

            var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, _jwtOptions,
                            new JsonSerializerSettings { Formatting = Formatting.Indented });

            var jwtResource = Mapper.Map<JwtModel, JwtResource>(jwt);

            jwtResource.BusinessDate = dateService.GetCurrentDate().SettingDateFormat();
            return Ok(jwtResource);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(userName);
            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);   

            //YEAH, this is the one to determin designer/drawing surveys
            //_userManager.GetUsersInRoleAsync()
            // check the credentials
            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                //Get Claims Here From ROLES and Merge Claims (Using HashSet) then pass to GenerateClaimsIdentity
                List<Claim> claimSet = new List<Claim>();
                var roles = await _userManager.GetRolesAsync(userToVerify);
                
                foreach(var role in roles) {
                    var idrole = await _roleManager.FindByNameAsync(role);
                    var claims = await _roleManager.GetClaimsAsync(idrole);
                    foreach(var claim in claims)
                        if(!claimSet.Any(c => c.Type == claim.Type))
                            claimSet.Add(claim);
                }

                //Add Real User Name
                claimSet.Add(new Claim("usr", userToVerify.FirstName + ' ' + userToVerify.LastName));     

                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id, claimSet.ToList()));
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }

        private async Task<IList<string>> GetUserRole(string userName, string password) {

            IList<string> userRoles = new List<string>();
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<IList<string>>(null);

            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(userName);
            if (userToVerify != null) {        
                userRoles = await _userManager.GetRolesAsync(userToVerify);
            }
            return userRoles;
        }
    }
}
