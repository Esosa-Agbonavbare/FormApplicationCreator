using AutoMapper;
using FormApplicationCreator.Application.DTOs;
using FormApplicationCreator.Domain.Entities;

namespace FormApplicationCreator.API.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<AddQuestionDto, Question>();
            CreateMap<QuestionResponseDto, Question>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.QuestionId))
                .ReverseMap();
            CreateMap<UpdateQuestionDto, Question>()
                .ReverseMap();
            CreateMap<AddCandidateDto, Candidate>();
            CreateMap<Candidate, CandidateResponseDto>()
                .ReverseMap();
            CreateMap<AddResponseDto, Response>();
            CreateMap<Response, ResponseDto>().ReverseMap();
            CreateMap<UpdateCandidateDto, Candidate>()
                .ReverseMap();
            CreateMap<AddApplicationFormDto, ApplicationForm>();
            CreateMap<ApplicationForm, ApplicationFormResponseDto>()
                .ReverseMap();
            CreateMap<UpdateApplicationFormDto, ApplicationForm>()
                .ReverseMap();
        }
    }
}
