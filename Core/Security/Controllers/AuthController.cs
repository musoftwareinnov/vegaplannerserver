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
        IBusinessDateRepository businessDateRepository;
        public AuthController(UserManager<AppUser> userManager,
                                RoleManager<IdentityRole> roleManager,
                                IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions,
                                IUserRepository userRepository, IMapper mapper,
                                IBusinessDateRepository businessDateRepository)
        {
            this.userRepository = userRepository;
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            this.Mapper = mapper;
            this.businessDateRepository = businessDateRepository;
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



            //Take first role (should only have one as this is claims based authorisation)
            var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, credentials.UserName, _jwtOptions, 
                            new JsonSerializerSettings { Formatting = Formatting.Indented }, 
                            Constants.Strings.JwtClaimIdentifiers.rol);

            var jwtResource = Mapper.Map<JwtModel, JwtResource>(jwt);

            //Add username to token for convenience
            var user = await userRepository.Get(jwt.Id);

            jwtResource.UserName = user.Identity.FirstName + " " + user.Identity.LastName;

            var bd = await businessDateRepository.GetBusinessDate();
            jwtResource.BusinessDate = bd.CurrBusDate.SettingDateFormat();
            return Ok(jwtResource);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(userName);
            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);   

            // check the credentials
            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                var roles = await GetUserRole(userName, password);
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id, roles));
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
