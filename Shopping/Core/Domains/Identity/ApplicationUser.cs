using System;
using Microsoft.AspNetCore.Identity;

namespace Shopping.Core.Domains
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
