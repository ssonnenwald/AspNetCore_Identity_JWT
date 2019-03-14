using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Identity.Data.Entities
{
    /// <summary>
    /// This model extends the default IdentityUser to add additional properties
    /// to the database table for the User's Identity.
    /// The default properties that come out of the box for the IdentityUser are:
    ///     AccessFailedCount - Gets or sets the number of failed login attempts for the current user.
    ///     ConcurrencyStamp - A random value that must change whenever a user is persisted to the store
    ///     Email - Gets or sets the email address for this user.
    ///     EmailConfirmed - Gets or sets a flag indicating if a user has confirmed their email address.
    ///     Id - Gets or sets the primary key for this user.
    ///     LockoutEnabled - Gets or sets a flag indicating if the user could be locked out.
    ///     LockoutEnd - Gets or sets the date and time, in UTC, when any user lockout ends.
    ///     NormalizedEmail - Gets or sets the normalized email address for this user.
    ///     NormalizedUserName - Gets or sets the normalized user name for this user.
    ///     PasswordHash - Gets or sets a salted and hashed representation of the password for this user.
    ///     PhoneNumber - Gets or sets a telephone number for the user.
    ///     PhoneNumberConfirmed - Gets or sets a flag indicating if a user has confirmed their telephone address.
    ///     SecurityStamp - A random value that must change whenever a users credentials change (password changed, login removed)
    ///     TwoFactorEnabled - Gets or sets a flag indicating if two factor authentication is enabled for this user.
    ///     UserName - Gets or sets the user name for this user.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// The first name of the user.
        /// We are extending the IdentityUser by adding this property.
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the user.
        /// We are extending the IdentityUser by adding this property.
        /// </summary>
        [Required]
        [MaxLength(250)]
        public string LastName { get; set; }
    }
}
