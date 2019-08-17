using Microsoft.IdentityModel.Tokens;
using TokenValidation.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace TokenValidation
{
    public static class TokenValidationParametersClass
    {
        public static TokenValidationParameters Get_C_S_ValidationParameters(string issuer, string audience, SymmetricSecurityKey signingKey)
        {
            //validationParameters
            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,

                ValidateAudience = true,
                ValidAudience = audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };

            return validationParameters;
        }

        public static void AddValidationParameters(this IServiceCollection services, string issuer, string audience, SymmetricSecurityKey signingKey)
        {
            //AddAuthentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //AddJwtBearer
            .AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = issuer;
                configureOptions.TokenValidationParameters = Get_C_S_ValidationParameters(issuer, audience, signingKey);
                configureOptions.SaveToken = true;
            });

            // api user claim policy
            services.AddAuthorization(options =>
            {
                //Add more roles here to handel diffrent type of users: admin, user, editUser
                options.AddPolicy(
                    TokenValidationConstants.Policies.AuthAPIAdmin,
                    policy => policy.RequireClaim(
                        TokenValidationConstants.Roles.Role,
                        TokenValidationConstants.Roles.AdminAccess));

                options.AddPolicy(
                    TokenValidationConstants.Policies.AuthAPICommonUser,
                    policy => policy.RequireClaim(
                        TokenValidationConstants.Roles.Role,
                        TokenValidationConstants.Roles.CommonUserAccess));

                options.AddPolicy(
                TokenValidationConstants.Policies.AuthAPIEditUser,
                policy => policy.RequireClaim(
                    TokenValidationConstants.Roles.Role,
                    TokenValidationConstants.Roles.EditUserAccess));
            });
        }
    }
}
