using Azure.Identity;
using Common.Models;
using Common.Services;
using Common.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;

namespace ClinicApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly PatientService patientService;
        private readonly DoctorService doctorService;

        public AccountController(//UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, 
            IConfiguration configuration,
            DoctorService doctorService,
            PatientService patientService)
        {
            //this.userManager = userManager;
            //this.roleManager = roleManager;
            this._configuration = configuration;
            this.doctorService = doctorService;
            this.patientService = patientService;
        }


        private GraphServiceClient getGraphClient()
        {
            var scopes = new[] { "https://graph.microsoft.com/.default" };

            var tenantId = _configuration["AzureAd:TenantId"];
            var clientId = _configuration["AzureAd:ClientId"];
            var clientSecret = _configuration["AzureAd:ClientSecret"];

            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };

            // https://docs.microsoft.com/dotnet/api/azure.identity.clientsecretcredential
            var clientSecretCredential = new ClientSecretCredential(
                tenantId, clientId, clientSecret, options);

            var graphClient = new GraphServiceClient(clientSecretCredential, scopes);
            return graphClient;
        }
        [HttpPost]
        [Route("register-Doctor")]
        public async Task<IActionResult> RegisterDoctor([FromBody] DoctorRegister model)
        {

            try
            {
                var mailNickname = model.Email.Split("@")[0];

                var graphClient = getGraphClient();

                var user = new Microsoft.Graph.User
                {
                    AccountEnabled = true,
                    DisplayName = model.Name,
                    MailNickname = mailNickname,
                    UserPrincipalName = model.Email,
                    PasswordProfile = new PasswordProfile
                    {
                        ForceChangePasswordNextSignIn = true,
                        Password = model.Password,
                    }
                };


                //save in Credentials in AD
                var created = await graphClient.Users
                    .Request()
                    .AddAsync(user);

                var RoleId = _configuration["AzureAd:DoctorRole"];
                var ResourceId = _configuration["AzureAd:ClinicAppObjectId"];
                var PrincipalId = created.Id;

                //add Role Assignment
                var appRoleAssignment = new AppRoleAssignment
                {
                    ResourceId = Guid.Parse(ResourceId), // resourceId
                    AppRoleId = Guid.Parse(RoleId), // role
                    PrincipalId = Guid.Parse(created.Id), //user or group

                };

                await graphClient.Users[created.Id].AppRoleAssignments
                    .Request()
                    .AddAsync(appRoleAssignment);


                //save Doctor
                Doctor doctor = new Doctor()
                {
                    Id = created.Id,
                    Name = model.Name,
                    BirthDate = model.BirthDate,
                    Speciality = model.Speciality
                };

                doctorService.Save(doctor);


                return Ok("User created successfully!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("register-Patient")]
        public async Task<IActionResult> Register([FromBody] PatientRegister model)
        {

            try
            {
                var mailNickname = model.Email.Split("@")[0];

                var graphClient = getGraphClient();

                var user = new Microsoft.Graph.User
                {
                    AccountEnabled = true,
                    DisplayName = model.Name,
                    MailNickname = mailNickname,
                    UserPrincipalName = model.Email,
                    PasswordProfile = new PasswordProfile
                    {
                        ForceChangePasswordNextSignIn = true,
                        Password = model.Password,
                    }
                };


                //save in Credentials in AD
                var created = await graphClient.Users
                    .Request()
                    .AddAsync(user);

                var RoleId = _configuration["AzureAd:PatientRole"];
                var ResourceId = _configuration["AzureAd:ClinicAppObjectId"];
                var PrincipalId = created.Id;

                //add Role Assignment
                var appRoleAssignment = new AppRoleAssignment
                {
                    ResourceId = Guid.Parse(ResourceId), // resourceId
                    AppRoleId = Guid.Parse(RoleId), // role
                    PrincipalId = Guid.Parse(created.Id), //user or group

                };

                await graphClient.Users[created.Id].AppRoleAssignments
                    .Request()
                    .AddAsync(appRoleAssignment);

                //save Patient
                Patient patient = new Patient()
                {
                    Id = created.Id,
                    Name = model.Name,
                    BirthDate = model.BirthDate,
                    Gender = model.Gender
                };

                patientService.Save(patient);

                return Ok("User created successfully!");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpPost]
        [Route("register-Admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            try
            {
                var mailNickname = model.Email.Split("@")[0];

                var graphClient = getGraphClient();

                var user = new Microsoft.Graph.User
                {
                    AccountEnabled = true,
                    DisplayName = mailNickname,
                    MailNickname = mailNickname,
                    UserPrincipalName = model.Email,
                    PasswordProfile = new PasswordProfile
                    {
                        ForceChangePasswordNextSignIn = true,
                        Password = model.Password,
                    }
                };


                //save in Credentials in AD
                var created = await graphClient.Users
                    .Request()
                    .AddAsync(user);

                var RoleId = _configuration["AzureAd:AdminRole"];
                var ResourceId = _configuration["AzureAd:ClinicAppObjectId"];
                var PrincipalId = created.Id;

                //add Role Assignment
                var appRoleAssignment = new AppRoleAssignment
                {
                    ResourceId = Guid.Parse(ResourceId), // resourceId
                    AppRoleId = Guid.Parse(RoleId), // role
                    PrincipalId = Guid.Parse(created.Id), //user or group

                };

                await graphClient.Users[created.Id].AppRoleAssignments
                    .Request()
                    .AddAsync(appRoleAssignment);

                return Ok("User created successfully.");

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT: ValidIssuer"],
                    audience: _configuration["JWT: ValidAudience"],
                    expires: DateTime.Now.AddYears(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );
                return Ok(new JwtSecurityTokenHandler().WriteToken(token)
                    );
            }
            return Unauthorized(
                "Please check your login credentials"
                );
        }
        [HttpPost("Account-Info")]
        public async Task<IActionResult> GetAccountInfo([FromQuery] string id)
        {
            try
            {
                var graphClient = getGraphClient();
                var user = await graphClient.Users[id]
                    .Request()
                    .GetAsync();

                return Ok(user);

            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
        }

    }
}
