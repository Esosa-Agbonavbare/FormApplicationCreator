using FormApplicationCreator.Application.DTOs;
using FormApplicationCreator.Domain;

namespace FormApplicationCreator.Application.Services.Interface
{
    public interface IApplicationFormService
    {
        Task<ApiResponse<ApplicationFormResponseDto>> AddApplicationFormAsync(AddApplicationFormDto applicationFormDto);
        Task<ApiResponse<bool>> DeleteApplicationFormAsync(string applicationFormId);
        Task<ApiResponse<ApplicationFormResponseDto>> GetByIdAsync(string applicationFormId);
        Task<ApiResponse<List<ApplicationFormResponseDto>>> GetAllAsync();
        Task<ApiResponse<ApplicationFormResponseDto>> UpdateApplicationFormAsync(string applicationFormId, UpdateApplicationFormDto updateApplicationFormDto);
    }
}
