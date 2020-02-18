using System;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Newtonsoft.Json;

namespace Shopping.Core.Commands
{
    public class RemoveUserRoleCommand : IRequest
    {
        [JsonIgnore] 
        public int UserId { get; }
        [Required] 
        public String RoleName { get; }

        public RemoveUserRoleCommand(int userId, String roleName)
        {
            UserId = userId;
            RoleName = roleName;
        }
    }
}