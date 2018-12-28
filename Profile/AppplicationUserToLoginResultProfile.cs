using AutoMapper;
using System.Collections.Generic;

namespace Remax.Web.Server
{
    /// <summary>
    /// 
    /// </summary>
    public class AppplicationUserToLoginResultProfile: Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public AppplicationUserToLoginResultProfile(List<string> roles)
        {
            CreateMap<ApplicationUser, LoginResultApiModel>()
                .ForMember(x => x.FirstName, y => y.MapFrom(z => z.Employee.FirstName))
                .ForMember(x => x.LastName, y => y.MapFrom(z => z.Employee.LastName))
                .ForMember(x => x.ImageSource, y => y.MapFrom(z => z.Employee.ImageSource))
                .ForMember(x => x.Token, y => y.MapFrom(z => z.GenerateJwtToken(roles)))
                ;

        }
        
    }
}
