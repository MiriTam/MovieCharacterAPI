using AutoMapper;
using MovieInfoAPI.Models.Domain;
using MovieInfoAPI.Models.DTO.Movie;
using System.Linq;

namespace MovieInfoAPI.Profiles
{
    public class MovieProfile : Profile
    {
        public MovieProfile()
        {
            CreateMap<Movie, MovieReadDTO>()
                .ForMember(dto => dto.Characters, opt => opt
                          .MapFrom(movie => movie.Characters
                                  .Select(c => c.CharacterId).ToList()))
                .ForMember(dto => dto.Franchise, opt => opt
                          .MapFrom(movie => movie.FranchiseId))
                .ReverseMap();

        }
    }
}
