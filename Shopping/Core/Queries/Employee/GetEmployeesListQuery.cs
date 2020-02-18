﻿using MediatR;
using Shopping.Core.ViewModels;

namespace Shopping.Core.Queries
{
    public class GetEmployeesListQuery : IRequest<EmployeesListVm>
    {
    }
}
