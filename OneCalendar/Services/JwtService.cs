using CaseSolutionsTokenValidationParameters.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OneCalendar.Helpers.Settings;
using OneCalendar.Interfaces;
using OneCalendar.Models;
using OneCalendar.ResponseModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace OneCalendar.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtIssuerOptions _jwtOptions;

        public JwtService(UserManager<User> userManager, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
            UserManager = userManager;
        }

        public UserManager<User> UserManager { get; }

        public ClaimsIdentity GenerateClaimsIdentity(string userName, string id, List<string> roles)
        {
            List<Claim> listOfClaims = new List<Claim>()
            {
                new Claim(TokenValidationConstants.Roles.Id, id)
            };

            //Add all roles linked to the current user as claims
            foreach (string roleItem in roles)
            {
                listOfClaims.Add(new Claim(TokenValidationConstants.Roles.Role, roleItem));
            }

            return new ClaimsIdentity(new GenericIdentity(userName, "Token"), listOfClaims);
        }

        public async Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity)
        {
            List<Claim> claims = new List<Claim>
            {
                 new Claim(JwtRegisteredClaimNames.Sub, userName),
                 new Claim(JwtRegisteredClaimNames.Jti, await _jwtOptions.JtiGenerator()),
                 new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64),
                 identity.FindFirst(TokenValidationConstants.Roles.Id)
             };

            //Adding all claims from AuthDatabase
            claims.AddRange(identity.FindAll(TokenValidationConstants.Roles.Role));

            // Create the JWT security token and encode it.
            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            string encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        public async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                return await Task.FromResult<ClaimsIdentity>(null);
            }

            // get the user to verifty
            User userToVerify = await UserManager.FindByNameAsync(userName);

            if (userToVerify == null)
            {
                return await Task.FromResult<ClaimsIdentity>(null);
            }

            IList<string> userRoles = await UserManager.GetRolesAsync(userToVerify);

            if (!userRoles.Any())
            {
                return await Task.FromResult<ClaimsIdentity>(null);
            }

            // check the credentials
            if (await UserManager.CheckPasswordAsync(userToVerify, password))
            {
                return await Task.FromResult(GenerateClaimsIdentity(userName, userToVerify.Id, userRoles.ToList()));
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }

        public async Task<string> GenerateJwt(ClaimsIdentity identity, IJwtService jwtService, string userName, JwtIssuerOptions jwtOptions, JsonSerializerSettings serializerSettings)
        {
            JwtResponse response = new JwtResponse
            {
                Id = identity.Claims.Single(c => c.Type == "id").Value,
                UserName = userName,
                Auth_Token = await GenerateEncodedToken(userName, identity),
                Expires_In = (int)jwtOptions.ValidFor.TotalSeconds,
                StatusCode = HttpStatusCode.OK,
                Error = "no_error",
                Description = "User was authenticated"
            };

            return JsonConvert.SerializeObject(response, serializerSettings);
        }

        private static long ToUnixEpochDate(DateTime date)
       => (long)Math.Round((date.ToUniversalTime() -
                            new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                           .TotalSeconds);
    }
}
