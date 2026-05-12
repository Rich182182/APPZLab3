using AutoMapper;
using FitnessClub.BLL.DTOs;
using FitnessClub.DAL.Entities;

namespace FitnessClub.BLL.Mapping
{
    public class FitnessMappingProfile : Profile
    {
        public FitnessMappingProfile()
        {
            CreateMap<Club, ClubDto>().ReverseMap();
            CreateMap<Client, ClientDto>().ReverseMap();
            CreateMap<SubscriptionType, SubscriptionTypeDto>().ReverseMap();
            CreateMap<User, UserDto>();
        }
    }
}