using AutoMapper;
using CoreCodeCamp.Models;

namespace CoreCodeCamp.Data
{
    public class CampProfile : Profile
    { 
        public CampProfile()
        {
            this.CreateMap<Camp, CampModel>()
                .ForMember(c => c.Venue, o => o.MapFrom(s => s.Location.VenueName))
                .ForMember(c => c.Address1, o => o.MapFrom(s => s.Location.Address1))
                .ForMember(c => c.Address2, o => o.MapFrom(s => s.Location.Address2))
                .ForMember(c => c.Address3, o => o.MapFrom(s => s.Location.Address3))
                .ForMember(c => c.CityTown, o => o.MapFrom(s => s.Location.CityTown))
                .ForMember(c => c.StateProvince, o => o.MapFrom(s => s.Location.StateProvince))
                .ForMember(c => c.PostalCode, o => o.MapFrom(s => s.Location.PostalCode))
                .ForMember(c => c.Country, o => o.MapFrom(s => s.Location.Country));
        }

    }
}
