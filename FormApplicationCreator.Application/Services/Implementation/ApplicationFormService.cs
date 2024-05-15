using AutoMapper;
using FormApplicationCreator.Application.DTOs;
using FormApplicationCreator.Application.Services.Interface;
using FormApplicationCreator.Domain.Entities;
using FormApplicationCreator.Domain;
using Microsoft.AspNetCore.Http;
using FormApplicationCreator.Persistence.Repositories.Interface;

namespace FormApplicationCreator.Application.Services.Implementation
{
    public class ApplicationFormService : IApplicationFormService
    {
        private readonly IApplicationFormRepository _applicationFormRepository;
        private readonly IMapper _mapper;
        private readonly IQuestionRepository _questionRepository;

        public ApplicationFormService(IApplicationFormRepository applicationFormRepository, IMapper mapper, IQuestionRepository questionRepository)
        {
            _applicationFormRepository = applicationFormRepository;
            _mapper = mapper;
            _questionRepository = questionRepository;
        }

        public async Task<ApiResponse<ApplicationFormResponseDto>> AddApplicationFormAsync(AddApplicationFormDto applicationFormDto)
        {
            try
            {
                var applicationForm = _mapper.Map<ApplicationForm>(applicationFormDto);
                await _applicationFormRepository.AddAsync(applicationForm);
                var questions = applicationFormDto.Questions.Select(r => _mapper.Map<Question>(r));
                await _questionRepository.AddListAsync(questions);
                var responseDto = _mapper.Map<ApplicationFormResponseDto>(applicationForm);
                return new ApiResponse<ApplicationFormResponseDto>(true, StatusCodes.Status201Created, "Application form added successfully.", responseDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while adding application form: {ex.Message}");
                return new ApiResponse<ApplicationFormResponseDto>(false, StatusCodes.Status500InternalServerError, "An error occurred while adding the application form.", [ex.Message]);
            }
        }

        public async Task<ApiResponse<bool>> DeleteApplicationFormAsync(string applicationFormId)
        {
            try
            {
                var existingApplicationForm = await _applicationFormRepository.GetByIdAsync(applicationFormId);
                if (existingApplicationForm == null)
                {
                    return new ApiResponse<bool>(false, StatusCodes.Status404NotFound, "Application form not found.", false);
                }
                await _applicationFormRepository.DeleteAsync(existingApplicationForm);
                return new ApiResponse<bool>(true, StatusCodes.Status200OK, "Application form deleted successfully.", true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while deleting application form: {ex.Message}");
                return new ApiResponse<bool>(false, StatusCodes.Status500InternalServerError, "An error occurred while deleting the application form.", false);
            }
        }

        public async Task<ApiResponse<List<ApplicationFormResponseDto>>> GetAllAsync()
        {
            try
            {
                var applicationForms = await _applicationFormRepository.GetAllAsync();
                var applicationFormResponseDtos = _mapper.Map<List<ApplicationFormResponseDto>>(applicationForms);
                foreach (var applicationFormResponseDto in applicationFormResponseDtos)
                {
                    var questions = await _questionRepository.GetAllByApplicationFormIdAsync(applicationFormResponseDto.Id);
                    applicationFormResponseDto.Questions = _mapper.Map<List<QuestionResponseDto>>(questions);
                }
                return new ApiResponse<List<ApplicationFormResponseDto>>(true, StatusCodes.Status200OK, "Application forms retrieved successfully.", applicationFormResponseDtos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while retrieving application forms: {ex.Message}");
                return new ApiResponse<List<ApplicationFormResponseDto>>(false, StatusCodes.Status500InternalServerError, "An error occurred while retrieving application forms.", [ex.Message]);
            }
        }

        public async Task<ApiResponse<ApplicationFormResponseDto>> GetByIdAsync(string applicationFormId)
        {
            try
            {
                var applicationForm = await _applicationFormRepository.GetByIdAsync(applicationFormId);
                if (applicationForm == null)
                {
                    return new ApiResponse<ApplicationFormResponseDto>(false, StatusCodes.Status404NotFound, "Application form not found.");
                }
                var applicationFormResponseDto = _mapper.Map<ApplicationFormResponseDto>(applicationForm);
                var questions = await _questionRepository.GetAllByApplicationFormIdAsync(applicationFormId);
                applicationFormResponseDto.Questions = _mapper.Map<List<QuestionResponseDto>>(questions);
                return new ApiResponse<ApplicationFormResponseDto>(true, StatusCodes.Status200OK, "Application form retrieved successfully.", applicationFormResponseDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while retrieving application form: {ex.Message}");
                return new ApiResponse<ApplicationFormResponseDto>(false, StatusCodes.Status500InternalServerError, "An error occurred while retrieving application form.", [ex.Message]);
            }
        }

        public async Task<ApiResponse<ApplicationFormResponseDto>> UpdateApplicationFormAsync(string applicationFormId, UpdateApplicationFormDto updateApplicationFormDto)
        {
            try
            {
                var existingApplicationForm = await _applicationFormRepository.GetByIdAsync(applicationFormId);
                if (existingApplicationForm == null)
                {
                    return new ApiResponse<ApplicationFormResponseDto>(false, StatusCodes.Status404NotFound, "Application form not found.");
                }
                _mapper.Map(updateApplicationFormDto, existingApplicationForm);
                await _applicationFormRepository.UpdateAsync(existingApplicationForm);
                if (updateApplicationFormDto.Questions != null)
                {
                    var updatedQuestions = _mapper.Map<List<Question>>(updateApplicationFormDto.Questions);
                    await _questionRepository.UpdateByApplicationFormIdAsync(applicationFormId, updatedQuestions);
                }
                var updatedApplicationFormResponseDto = _mapper.Map<ApplicationFormResponseDto>(existingApplicationForm);
                return new ApiResponse<ApplicationFormResponseDto>(true, StatusCodes.Status200OK, "Application form updated successfully.", updatedApplicationFormResponseDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while updating application form: {ex.Message}");
                return new ApiResponse<ApplicationFormResponseDto>(false, StatusCodes.Status500InternalServerError, "An error occurred while updating application form.", [ex.Message]);
            }
        }
    }
}
