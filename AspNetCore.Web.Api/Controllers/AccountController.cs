using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AspNetCore.Identity.Api.Models;
using AspNetCore.Identity.Data.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace AspNetCore.Identity.Api.Controllers
{
    /// <summary>
    /// The account controller.
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountController> _logger;

        /// <summary>
        /// The constructor for the accountcontroller.
        /// </summary>
        /// <param name="userManager">Provides the APIs for managing users in a persistence store.</param>
        /// <param name="signInManager">Provides the APIs for user sign in.</param>
        /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
        /// <param name="logger">A generic interface for logging where the category name is derived from the specified TCategoryName type name.  Generally used to enable activation of a named ILogger from dependency injection.</param>
        public AccountController(
           UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           IConfiguration configuration,
           ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="loginModel">The login model the user will post.</param>
        /// <returns>Returns a token for the user.</returns>
        [HttpPost]
        [ApiExplorerSettings(GroupName = "Identity")]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            // Validate the LoginModel.
            if (ModelState.IsValid)
            {
                // Attempt to sign in the specified username and password combination.
                var loginResult = await _signInManager.PasswordSignInAsync(loginModel.UserName, 
                    loginModel.Password, isPersistent: false, lockoutOnFailure: false);

                // Validate the sign in succeeded.
                if (!loginResult.Succeeded)
                {
                    // Log to the debug window.
                    _logger.LogInformation("Could not sign in for user: {User}", loginModel.UserName);

                    // Return a bad request if it did not.  (HTTP status 400)
                    return BadRequest();
                }

                // Find and return a user, if any, who has the specified user name. 
                var user = await _userManager.FindByNameAsync(loginModel.UserName);

                // Log to the debug window.
                _logger.LogInformation("Successful sign in for user: {User}", loginModel.UserName);

                // Return an HTTP Status of 200 and the JWT Token for the user.
                return Ok(GetToken(user));
            }

            // Log to the debug window.
            _logger.LogInformation("Invalid loginModel");

            // Return a bad request and the errors in the LoginModel passed in. (HTTP status 400)
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Signout
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiExplorerSettings(GroupName = "Identity")]
        [Route("signout")]
        public async Task SignOut()
        {
            // Sign out
            await _signInManager.SignOutAsync();
        }

        /// <summary>
        /// Refresh the JWT Token for the user if it expired.
        /// </summary>
        /// <returns>Returns a new JWT Token for the user.</returns>
        [Authorize]
        [HttpPost]
        [ApiExplorerSettings(GroupName = "Identity")]
        [Route("refreshtoken")]
        public async Task<IActionResult> RefreshToken()
        {
            // Find and return a user, if any, who has the specified user name.
            var user = await _userManager.FindByNameAsync(
                User.Identity.Name ??
                User.Claims.Where(c => c.Properties.ContainsKey("unique_name")).Select(c => c.Value).FirstOrDefault()
                );

            // Log to the debug window.
            _logger.LogInformation("Token refresh for user: {User}", user.UserName);

            // Return an HTTP Status of 200 and the JWT Token for the user.
            return Ok(GetToken(user));

        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="registerModel">The register model the user will post.</param>
        /// <returns>Returns a JWT Token for the user on successful registration or the errors if unable to register the user.</returns>
        [HttpPost]
        [ApiExplorerSettings(GroupName = "Identity")]
        [Route("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            // Validate the RegisterModel.
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                Mapper.Map<ApplicationUser, RegisterModel>(user, registerModel);
                
                // Creates the specified user in the backing store with given password.
                var identityResult = await _userManager.CreateAsync(user, registerModel.Password);

                // Validate the user was successfully registered.
                if (identityResult.Succeeded)
                {
                    // Sign in the specified user.
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    // Log to the debug window.
                    _logger.LogInformation("Registration successful for user: {User}", user.UserName);

                    // Return an HTTP Status of 200 and the JWT Token for the user.
                    return Ok(GetToken(user));
                }
                else
                {
                    // Log to the debug window.
                    _logger.LogInformation("Unable to register user: {User}", registerModel.UserName);

                    // Return a bad request and the errors in the registration of the user. (HTTP status 400)
                    return BadRequest(identityResult.Errors);
                }
            }

            // Log to the debug window.
            _logger.LogInformation("Invalid registerModel");

            // Return a bad request and the errors in the RegisterModel passed in. (HTTP status 400)
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Returns a JWT Toekn for the user.
        /// </summary>
        /// <param name="user">The identity user.</param>
        /// <returns>Returns the serialized JWT Token as a string.</returns>
        private string GetToken(IdentityUser user)
        {
            // Get the current Utc time.
            var utcNow = DateTime.UtcNow;

            // Create the claims for the user.
            // A Claim is a property of an Identity consisted of a name-value pair specific to 
            // that Identity while an Identity may have many Claims associated with it.
            var claims = new Claim[]
            {
                        // "sub" (Subject) Claim
                        // The "sub" (subject) claim identifies the principal that is the
                        // subject of the JWT.  The claims in a JWT are normally statements
                        // about the subject.  The subject value MUST either be scoped to be
                        // locally unique in the context of the issuer or be globally unique.
                        // The processing of this claim is generally application specific.  The
                        // "sub" value is a case-sensitive string containing a StringOrURI
                        // value.  Use of this claim is OPTIONAL.
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                        // The Claim Names within a JWT Claims Set MUST be unique.
                        new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                        // "jti" (JWT ID) Claim
                        // The "jti" (JWT ID) claim provides a unique identifier for the JWT.
                        // The identifier value MUST be assigned in a manner that ensures that
                        // there is a negligible probability that the same value will be
                        // accidentally assigned to a different data object; if the application
                        // uses multiple issuers, collisions MUST be prevented among values
                        // produced by different issuers as well.The "jti" claim can be used
                        // to prevent the JWT from being replayed.The "jti" value is a case-
                        // sensitive string.Use of this claim is OPTIONAL.
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        // "iat" (Issued At) Claim
                        // The "iat" (issued at) claim identifies the time at which the JWT was
                        // issued.  This claim can be used to determine the age of the JWT.  Its
                        // value MUST be a number containing a NumericDate value.  Use of this
                        // claim is OPTIONAL.
                        new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString())
            };

            // Create the signing key for the token.
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Tokens:Key")));

            // Create the signing credentials for the token.
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            // Create the new JWT Token.
            var jwt = new JwtSecurityToken(
                signingCredentials: signingCredentials,
                claims: claims,
                notBefore: utcNow,
                expires: utcNow.AddSeconds(_configuration.GetValue<int>("Tokens:Lifetime")),
                audience: _configuration.GetValue<string>("Tokens:Audience"),
                issuer: _configuration.GetValue<string>("Tokens:Issuer")
                );

            // Log to the debug window.
            _logger.LogInformation("Successfully created token for user: {User}", user.UserName);

            // Serialize the JWT Token and return it.
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}