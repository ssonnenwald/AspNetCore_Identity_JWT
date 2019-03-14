using AutoMapper;

namespace AspNetCore.Identity.Api.Mappings
{
    /// <summary>
    /// Configuration for Automapper.
    /// </summary>
    public class AutoMapperConfiguration
    {
        /// <summary>
        /// The organization profile.
        /// </summary>
        public static void OrganizationProfile()
        {
            // Initialize automapper with the configuration.
            Mapper.Initialize(x =>
            {
                x.AllowNullCollections = true;
                x.ValidateInlineMaps = false;
                x.AddProfile<ViewModelToDomainMappingProfile>();
            });
        }
    }
}
