using System;
using System.ComponentModel.DataAnnotations;
using Common.Core.Binding;
using MediatR;
using Newtonsoft.Json;
using Shopping.Core.Domains;

namespace Shopping.Core.Commands
{
    public class RegisterUserCommand : IRequest<ApplicationUser>
    {
        [Required(ErrorMessage = "First name value is mandatory")]
        public string FirstName { get; }

        [Required(ErrorMessage = "Last name value is mandatory")]
        public string LastName { get; }

        [Required(ErrorMessage = "Email address value is mandatory")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; }

        [Required(ErrorMessage = "UserName value is mandatory")]
        public string UserName { get; }

        [Required(ErrorMessage = "Password value is mandatory")]
        public string Password { get; }

        public DateTime DateOfBirth { get; }

        [JsonConstructor]
        public RegisterUserCommand(string firstName, string lastName, DateTime dateOfBirth, string email,
            string password, string userName)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            UserName = userName;
            DateOfBirth = dateOfBirth;
        }
    }
}