using System;
using System.Collections.Generic;
using MediatR;
using Newtonsoft.Json;
using Shopping.Core.Dtos;
using Shopping.Core.Dtos.User;

namespace Shopping.Core.Queries
{
    public class FilterUserQuery : IRequest<IEnumerable<UserDetailInfoDto>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
