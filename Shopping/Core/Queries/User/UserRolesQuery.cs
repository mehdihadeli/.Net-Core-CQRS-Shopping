using System;
using System.Collections.Generic;
using MediatR;

namespace Shopping.Core.Queries
{
    public class UserRolesQuery : IRequest<IEnumerable<string>>
    {
        public int UserId { get; set; }
    }
}