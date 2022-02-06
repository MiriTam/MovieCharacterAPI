using AutoMapper;
using MovieInfoAPI.Models.Domain;
using MovieInfoAPI.Models.DTO.Franchise;

namespace MovieInfoAPI.Profiles
{
    public class FranchiseProfile : Profile
    {
        public FranchiseProfile()
        {
            CreateMap<Franchise, FranchiseReadDTO>().ReverseMap();
        }
    }
}
