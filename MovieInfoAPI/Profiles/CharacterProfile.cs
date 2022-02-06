using AutoMapper;
using MovieInfoAPI.Models.Domain;
using MovieInfoAPI.Models.DTO.Character;

namespace MovieInfoAPI.Profiles
{
    public class CharacterProfile : Profile
    {
        public CharacterProfile()
        {
            CreateMap<Character, CharacterReadDTO>().ReverseMap();

            CreateMap<Character, CharacterCreateDTO>().ReverseMap();

            CreateMap<Character, CharacterEditDTO>().ReverseMap();
        }
    }
}
