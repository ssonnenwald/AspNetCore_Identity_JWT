using AspNetCore.Identity.Api.Models;
using AspNetCore.Identity.Data.Entities;
using AutoMapper;

namespace AspNetCore.Identity.Api.Mappings
{
    /// <summary>
    /// Automapper mappings for the view model object to the domain object.
    /// </summary>
    public class ViewModelToDomainMappingProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<RegisterModel, ApplicationUser>()
                .ForMember(s => s.UserName, map => map.MapFrom(vm => vm.UserName))
                .ForMember(s => s.FirstName, map => map.MapFrom(vm => vm.FirstName))
                .ForMember(s => s.LastName, map => map.MapFrom(vm => vm.LastName))
                .ForMember(s => s.Email, map => map.MapFrom(vm => vm.Email));
        }
    }
}
