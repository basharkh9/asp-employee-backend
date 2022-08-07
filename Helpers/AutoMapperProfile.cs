using AutoMapper;
using EmployeeCRUD.API.Dtos;
using EmployeeCRUD.API.Models;
using Helpers;

namespace EmployeeCRUD.API.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //CalculateAge is an Extension method on DateTime
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.PhotoUrl,
                    opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.isMain).Url))
                .ForMember(des => des.Age,
                    opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
        }
    }
}