using AutoMapper;
using CoreCodeCamp.Models;

namespace CoreCodeCamp.Data
{
    public class CampModelProfile : Profile
    {
        public CampModelProfile()
        {
            this.CreateMap<CampModel, Camp>()
                .ForPath(c => c.Location.LocationId, o => o.MapFrom(s => s.LocationId))
                .ForPath(c => c.Location.VenueName, o => o.MapFrom(s => s.Venue))
                .ForPath(c => c.Location.Address1, o => o.MapFrom(s => s.Address1))
                .ForPath(c => c.Location.Address2, o => o.MapFrom(s => s.Address2))
                .ForPath(c => c.Location.Address3, o => o.MapFrom(s => s.Address3))
                .ForPath(c => c.Location.CityTown, o => o.MapFrom(s => s.CityTown))
                .ForPath(c => c.Location.StateProvince, o => o.MapFrom(s => s.StateProvince))
                .ForPath(c => c.Location.PostalCode, o => o.MapFrom(s => s.PostalCode))
                .ForPath(c => c.Location.Country, o => o.MapFrom(s => s.Country));

            this.CreateMap<TalkModel, Talk>()
                .ReverseMap();

            this.CreateMap<SpeakerModel, Speaker>()
                .ReverseMap();
        }

    }
}
