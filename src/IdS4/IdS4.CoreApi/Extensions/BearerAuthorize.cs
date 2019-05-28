using Microsoft.AspNetCore.Authorization;

namespace IdS4.CoreApi.Extensions
{
    public class BearerAuthorizeAttribute: AuthorizeAttribute
    {
        public BearerAuthorizeAttribute()
        {
            base.AuthenticationSchemes = "Bearer";
        }
    }
}
