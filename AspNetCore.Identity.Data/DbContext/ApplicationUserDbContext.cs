using AspNetCore.Identity.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Identity.Data.DbContext
{
    /// <summary>
    /// DbContext for the Application User Identity.
    /// </summary>
    public class ApplicationUserDbContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options">Options for the DbContext.</param>
        public ApplicationUserDbContext(DbContextOptions<ApplicationUserDbContext> options) : base(options)
        {

        }
    }
}
