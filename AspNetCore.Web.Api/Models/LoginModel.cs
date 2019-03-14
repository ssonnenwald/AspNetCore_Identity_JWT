using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Identity.Api.Models
{
    /// <summary>
    /// The login model that a user will post.
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// The user name.
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// The password for the user.
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
