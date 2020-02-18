using System;
using MediatR;
using Newtonsoft.Json;
using Shopping.Core.Dtos;
using Shopping.Core.Dtos.User;

namespace Shopping.Core.Queries
{
    public class UserQuery : IRequest<UserDetailInfoDto>
    {
        public int Id { get; }

        [JsonConstructor]
        public UserQuery(int id)
        {
            Id = id;
        }
    }
}