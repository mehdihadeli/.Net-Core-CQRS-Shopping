using System;
using MediatR;

namespace Shopping.Core.Commands
{
    public class UpsertEmployeeCommand : IRequest<int>
    {
        public int? Id { get; set; }

        public string Title { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public string PostalCode { get; set; }

        public string Country { get; set; }

        public string HomePhone { get; set; }

        public string Position { get; set; }

        public string Extension { get; set; }

        public DateTime? HireDate { get; set; }

        public string Notes { get; set; }

        public byte[] Photo { get; set; }

        public int? ManagerId { get; set; }

    }
}
