using System;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Newtonsoft.Json;
using Shopping.Core.Domains;

namespace Shopping.Core.Commands
{
    public class UpdateUserInfoCommand : IRequest
    {
        [JsonIgnore] public int Id { get; }

        [Required(ErrorMessage = "First name value is mandatory")]
        [MaxLength(length: 100, ErrorMessage = "Maximum length is 100 characters")]
        public String FirstName { get; }

        [Required(ErrorMessage = "Last name value is mandatory")]
        [MaxLength(length: 200, ErrorMessage = "Maximum length is 200 characters")]
        public String LastName { get; }

        [JsonConstructor]
        public UpdateUserInfoCommand(int id, String firstName, String lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}