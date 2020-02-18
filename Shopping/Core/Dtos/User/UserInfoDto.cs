using System;

namespace Shopping.Core.Dtos.User
{
    public class UserInfoDto
    {
        /// <summary>
        /// User's first name
        /// </summary>
        public String FirstName { get; set; }

        /// <summary>
        /// User's last name
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
