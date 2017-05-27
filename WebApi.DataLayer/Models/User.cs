using System;
namespace WebApi.DataLayer.Models
{
    public class User
    {
        public User() { }

        /// <summary>
        /// The Name of the User
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The Email address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The MD5 hashed Passowrd of the User
        /// </summary>
        public string Password { get; set; }
    }
}
