using AutoMapper;
using Domain.Models;
using Domain.ViewModels;

namespace Infrastructure.Configuration.AutoMapperProfiles
{
    public class MainProfile : Profile
    {
        public MainProfile() 
        {
            CreateMap<Question, QuestionVM>().ReverseMap();
        }
    }
}
