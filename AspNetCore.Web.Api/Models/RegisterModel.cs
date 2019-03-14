using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Identity.Api.Models
{
    /// <summary>
    /// The registration model for the user.
    /// </summary>
    public class RegisterModel : LoginModel
    {
        /// <summary>
        /// The first name the user is going to register with.
        /// </summary>
        [Required]
        [StringLength(200)]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name the user is going to register with.
        /// </summary>
        [Required]
        [StringLength(250)]
        public string LastName { get; set; }

        /// <summary>
        /// The email the user is going to register with.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// The password confirmation the user is going to register with.
        /// </summary>
        [Required]
        [Compare("Password")]
        [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
        public string PasswordConfirmation { get; set; }
    }
}
