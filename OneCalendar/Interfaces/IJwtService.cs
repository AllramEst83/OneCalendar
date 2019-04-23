using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using OneCalendar.Helpers.Settings;
using OneCalendar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OneCalendar.Interfaces
{
    public interface IJwtService
    {
        UserManager<User> UserManager { get; }

        ClaimsIdentity GenerateClaimsIdentity(string userName, string id, List<string> roles);
        Task<string> GenerateEncodedToken(string userName, ClaimsIdentity identity);
        Task<string> GenerateJwt(ClaimsIdentity identity, Interfaces.IJwtService jwtService, string userName, JwtIssuerOptions jwtOptions, JsonSerializerSettings serializerSettings);
        Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password);
    }
}
