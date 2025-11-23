using AutoMapper;
using FollowUpWorks.DTOs;
using FollowUpWorks.Models;

namespace FollowUpWorks.Core
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {

            CreateMap<EventClass, EventClassDTO>()
               .ForMember(dest => dest.idEvent, opt => opt.MapFrom(src => src.Id))
               .ReverseMap()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.idEvent));

            CreateMap<ExpensesClass, ExpensesClassDTO>()
                .ForMember(dest => dest.IdExpense, opt => opt.MapFrom(src => src.Id))
               .ReverseMap()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdExpense));

            CreateMap<MemoryGameClass,MemoryGameClassDTO>().ReverseMap();

            CreateMap<NoteClass, NoteClassDTO>()
                 .ForMember(dest => dest.idNotes, opt => opt.MapFrom(src => src.Id))
               .ReverseMap()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.idNotes));

            CreateMap<PasswordHashClass, PasswordHashClassDTO>()
                .ForMember(dest => dest.idPassword, opt => opt.MapFrom(src => src.Id))
               .ReverseMap()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.idPassword));

            CreateMap<RecipeClass, RecipeClassDTO>()
                .ForMember(dest => dest.idRecipe, opt => opt.MapFrom(src => src.Id))
               .ReverseMap()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.idRecipe));

            CreateMap<ReservationClass, ReservationClassDTO>()
                 .ForMember(dest => dest.idReservation, opt => opt.MapFrom(src => src.Id))
               .ReverseMap()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.idReservation));

            // Survey <-> SurveyClassDTO
            CreateMap<SurveyClass, SurveyClassDTO>()
                .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions))
                .ReverseMap()
                .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions));

            // SurveyQuestion <-> SurveyQuestionDTO
            CreateMap<SurveyQuestion, SurveyQuestionDTO>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => ParseQuestionType(src.Type)))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id == Guid.Empty ? Guid.NewGuid() : src.Id));

            // SurveyResponse <-> SurveyResponseDTO
            CreateMap<SurveyResponse, SurveyResponseDTO>();
            CreateMap<SurveyResponseDTO, SurveyResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ForMember(dest => dest.SubmittedAt, opt => opt.MapFrom(src => DateTime.UtcNow));

            // QuestionResponse <-> QuestionResponseDTO
            CreateMap<QuestionResponse, QuestionResponseDTO>().ReverseMap();

            CreateMap<TaskListClass, TaskListClassDTO>()
            .ForMember(dest => dest.idTask, opt => opt.MapFrom(src => src.Id))
                   .ReverseMap()
                   .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.idTask));

            CreateMap<TimeKeeperClass, TimeKeeperClassDTO>()
                    .ForMember(dest => dest.idTimeKeeper, opt => opt.MapFrom(src => src.Id))
                   .ReverseMap()
                   .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.idTimeKeeper));

            CreateMap<TipClass, TipClassDTO>()
                .ForMember(dest => dest.IdTips, opt => opt.MapFrom(src => src.Id))
               .ReverseMap()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdTips));
        }

        private static QuestionType ParseQuestionType(string type)
        {
            return Enum.TryParse<QuestionType>(type, out var result)
                ? result
                : QuestionType.SingleChoice;
        }
    }
}

