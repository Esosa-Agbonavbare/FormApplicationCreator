using FormApplicationCreator.Application.DTOs;
using FormApplicationCreator.Domain.Enums;
using FormApplicationCreator.Domain;

namespace FormApplicationCreator.Application.Services.Interface
{
    public interface IQuestionService
    {
        Task<ApiResponse<QuestionResponseDto>> GetQuestionByIdAsync(string questionId);
        Task<ApiResponse<List<QuestionResponseDto>>> GetQuestionsAsync(string applicationFormId);
        Task<ApiResponse<QuestionResponseDto>> AddQuestionAsync(string applicationFormId, AddQuestionDto addQuestionDto);
        Task<ApiResponse<QuestionResponseDto>> UpdateQuestionAsync(string questionId, UpdateQuestionDto updateQuestionDto);
        Task<ApiResponse<bool>> DeleteQuestionAsync(string questionId);
        Task<List<string>> GetQuestionTypesAsync();
    }
}
