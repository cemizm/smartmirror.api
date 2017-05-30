using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    public class BaseController : Controller
    {
        private IQueryable<Claim> Claims => HttpContext.User.Claims.AsQueryable();

        protected string UserName => Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).FirstOrDefault();

        protected string UserEmail => Claims.Where(c => c.Type == ClaimTypes.Email).Select(c => c.Value).FirstOrDefault();
    }
}
