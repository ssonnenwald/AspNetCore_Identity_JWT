using AspNetCore.Identity.Data.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace AspNetCore.Identity.Data
{
    /// <summary>
    /// The application user dbcontext factory.
    /// </summary>
    public class ApplicationUserDbContextFactory
    {
        /// <summary>
        /// Create the dbcontext.
        /// </summary>
        /// <param name="args">An array of arguments passed to the create dbcontext.</param>
        /// <returns>Returns the dbcontext.</returns>
        public ApplicationUserDbContext CreateDbContext(string[] args)
        {
            // Create the dbcontext.  Get the settings from the appsettings.json file.
            ApplicationUserDbContext dbContext = new ApplicationUserDbContext(new DbContextOptionsBuilder<ApplicationUserDbContext>().UseSqlServer(
               new ConfigurationBuilder()
                   .AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), $"appsettings.json"))
                   .Build()
                   .GetConnectionString("DatabaseConnection")
               ).Options);

            // Applies any migrations to the database and if it doesn't exist this will create it.
            dbContext.Database.Migrate();

            // Return the dbcontext.
            return dbContext;
        }
    }
}
