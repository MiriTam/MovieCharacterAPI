using AutoMapper;
using MovieInfoAPI.Models.Domain;
using MovieInfoAPI.Models.DTO.Movie;

namespace MovieInfoAPI.Profiles
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<Movie, MovieReadDTO>()
                .ForMember(dto => dto.Franchise, opt => opt.MapFrom(movie => movie.FranchiseId))
                .ReverseMap();

            CreateMap<Movie, MovieCreateDTO>()
                .ForMember(dto => dto.Franchise, opt => opt.MapFrom(movie => movie.FranchiseId))
                .ReverseMap();

            CreateMap<Movie, MovieEditDTO>()
                .ForMember(dto => dto.Franchise, opt => opt.MapFrom(movie => movie.FranchiseId))
                .ReverseMap();
        }
    }
}
