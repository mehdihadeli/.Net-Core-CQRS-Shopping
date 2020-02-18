using System;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Newtonsoft.Json;

namespace Shopping.Core.Commands
{
    public class AddUserRoleCommand : IRequest
    {
        [JsonIgnore] 
        public int UserId { get; }

        [Required] public String RoleName { get; }

        [JsonConstructor]
        public AddUserRoleCommand(int userId, String roleName)
        {
            UserId = userId;
            RoleName = roleName;
        }
    }
}