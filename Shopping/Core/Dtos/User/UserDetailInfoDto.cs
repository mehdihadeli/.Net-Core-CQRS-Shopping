using System;
using AutoMapper;
using Shopping.Core.Domains;

namespace Shopping.Core.Dtos.User
{
    public class UserDetailInfoDto
    {
        /// <summary>
        /// Unique user identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// User's first name
        /// </summary>
        public String FirstName { get; set; }

        /// <summary>
        /// Users's last name
        /// </summary>
        public String LastName { get; set; }

        /// <summary>
        /// User's email address
        /// </summary>
        public String Email { get; set; }

        /// <summary>
        /// User's date of birth
        /// </summary>
        public DateTime DateOfBirth { get; set; }
    }
}
