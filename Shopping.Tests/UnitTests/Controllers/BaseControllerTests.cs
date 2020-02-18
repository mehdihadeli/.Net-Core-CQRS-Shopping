using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Shopping.Tests.UnitTests.Controllers
{
   public abstract class BaseControllerTests
    {
        protected void SetAuthenticationContext(ControllerBase controller)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                 new Claim(
                     type: "userid",
                     value: Guid.NewGuid().ToString())
            }));
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }
    }
}
