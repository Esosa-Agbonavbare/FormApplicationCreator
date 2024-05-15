using FormApplicationCreator.Application.DTOs;
using FormApplicationCreator.Application.Services.Interface;
using FormApplicationCreator.Domain.Entities;
using FormApplicationCreator.Domain.Enums;
using FormApplicationCreator.Domain;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using FormApplicationCreator.Persistence.Repositories.Interface;

namespace FormApplicationCreator.Application.Services.Implementation
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IApplicationFormRepository _applicationFormRepository;
        private readonly IMapper _mapper;


        public QuestionService(IQuestionRepository questionRepository, IApplicationFormRepository applicationFormRepository, IMapper mapper)
        {
            _questionRepository = questionRepository;
            _applicationFormRepository = applicationFormRepository;
            _mapper = mapper;

        }

        public async Task<ApiResponse<QuestionResponseDto>> AddQuestionAsync(string applicationFormId, AddQuestionDto addQuestionDto)
        {
            try
            {
                var question = _mapper.Map<Question>(addQuestionDto);
                question.ApplicationFormId = applicationFormId;
                await _questionRepository.AddAsync(question);
                var questionResponseDto = _mapper.Map<QuestionResponseDto>(question);
                return new ApiResponse<QuestionResponseDto>(true, StatusCodes.Status201Created, "Question added successfully.", questionResponseDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while adding question: {ex.Message}");
                return new ApiResponse<QuestionResponseDto>(false, StatusCodes.Status500InternalServerError, "An error occurred while adding the question.", [ex.Message]);
            }
        }

        public async Task<ApiResponse<bool>> DeleteQuestionAsync(string questionId)
        {
            try
            {
                var question = await _questionRepository.GetByIdAsync(questionId);
                if (question == null)
                {
                    return new ApiResponse<bool>(false, StatusCodes.Status404NotFound, "Question not found");
                }
                await _questionRepository.DeleteAsync(questionId);
                return new ApiResponse<bool>(true, StatusCodes.Status200OK, "Question deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while deleting question: {ex.Message}");
                return new ApiResponse<bool>(false, StatusCodes.Status500InternalServerError, "An error occurred while deleting the question.", [ex.Message]);
            }
        }

        public async Task<ApiResponse<QuestionResponseDto>> GetQuestionByIdAsync(string questionId)
        {
            try
            {
                var question = await _questionRepository.GetByIdAsync(questionId);
                if (question == null)
                {
                    return new ApiResponse<QuestionResponseDto>(false, StatusCodes.Status404NotFound, "Question not found.");
                }
                var questionResponseDto = _mapper.Map<QuestionResponseDto>(question);
                return new ApiResponse<QuestionResponseDto>(true, StatusCodes.Status200OK, "Question retrieved successfully.", questionResponseDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while retrieving question: {ex.Message}");
                return new ApiResponse<QuestionResponseDto>(false, StatusCodes.Status500InternalServerError, "An error occurred while retrieving the question.", [ex.Message]);
            }
        }

        public async Task<ApiResponse<List<QuestionResponseDto>>> GetQuestionsAsync(string applicationFormId)
        {
            try
            {
                var application = await _applicationFormRepository.GetByIdAsync(applicationFormId);
                if (application == null)
                {
                    return new ApiResponse<List<QuestionResponseDto>>(false, StatusCodes.Status404NotFound, "Application form not found.");
                }
                var questions = await _questionRepository.GetAllByApplicationFormIdAsync(applicationFormId);
                var questionResponseDtos = _mapper.Map<List<QuestionResponseDto>>(questions);
                return new ApiResponse<List<QuestionResponseDto>>(true, StatusCodes.Status200OK, "Questions retrieved successfully.", questionResponseDtos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while retrieving questions: {ex.Message}");
                return new ApiResponse<List<QuestionResponseDto>>(false, StatusCodes.Status500InternalServerError, "An error occurred while retrieving the questions.", [ex.Message]);
            }
        }

        public async Task<ApiResponse<QuestionResponseDto>> UpdateQuestionAsync(string questionId, UpdateQuestionDto updateQuestionDto)
        {
            try
            {
                var question = _mapper.Map<Question>(updateQuestionDto);
                question.Id = questionId;
                await _questionRepository.UpdateAsync(question);
                var questionResponseDto = _mapper.Map<QuestionResponseDto>(question);
                return new ApiResponse<QuestionResponseDto>(true, StatusCodes.Status200OK, "Question updated successfully.", questionResponseDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while updating question: {ex.Message}");
                return new ApiResponse<QuestionResponseDto>(false, StatusCodes.Status500InternalServerError, "An error occurred while updating the question.", [ex.Message]);
            }
        }

        public Task<List<string>> GetQuestionTypesAsync()
        {
            var questionTypes = Enum.GetNames(typeof(QuestionType)).ToList();
            return Task.FromResult(questionTypes);
        }
    }
}
