using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace docker_sandbox_dev_api.Filters
{
    public class UserMapperFilter : System.Attribute, IAsyncAuthorizationFilter
    {
        private readonly IConfiguration configuration;

        public UserMapperFilter(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            bool authorized = false;
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                string accessToken = context.HttpContext.Request.Headers[HeaderNames.Authorization][0].Replace("Bearer ", "");
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                SecurityToken jsonToken = handler.ReadToken(accessToken);
                JwtSecurityToken tokenS = handler.ReadToken(accessToken) as JwtSecurityToken;
                System.Security.Claims.Claim subClaim = tokenS.Claims.FirstOrDefault(t => t.Type == "sub");
                if (subClaim == null)
                {
                    throw new Exception("400");
                }
                Guid userId = Guid.Parse(subClaim.Value);

                context.HttpContext.Items["UserId"] = userId;
                authorized = true;
            }

            if (!authorized)
            {
                //throw new ApiException(new ApiExceptionMessage { Message = "Unauthorized", StatusCode = (int)HttpStatusCode.Unauthorized });
                throw new Exception("403");
            }
        }
    }
}
