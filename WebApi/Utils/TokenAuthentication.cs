using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Utils
{
    public static class TokenAuthentication
    {

        public static void UseTokenAuthentication(this IApplicationBuilder app, TokenSettings settings)
        {
            TokenManager mngr = new TokenManager(settings);

            app.UseJwtBearerAuthentication(new JwtBearerOptions()
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = mngr.GetSecurityKey(),

                    ValidateIssuer = true,
                    ValidIssuer = settings.Issuer,

                    ValidateAudience = true,
                    ValidAudience = settings.Audience,

                    ValidateLifetime = true,

                    ClockSkew = TimeSpan.Zero
                },
            });
        }
    }
}
