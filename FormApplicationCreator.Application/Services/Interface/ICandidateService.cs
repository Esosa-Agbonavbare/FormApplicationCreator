using FormApplicationCreator.Application.DTOs;
using FormApplicationCreator.Domain;

namespace FormApplicationCreator.Application.Services.Interface
{
    public interface ICandidateService
    {
        Task<ApiResponse<CandidateResponseDto>> AddCandidateAsync(AddCandidateDto addCandidateDto);
        Task<ApiResponse<List<CandidateResponseDto>>> GetAllCandidatesAsync();
        Task<ApiResponse<CandidateResponseDto>> GetCandidateByIdAsync(string candidateId);
        Task<ApiResponse<bool>> DeleteCandidateAsync(string candidateId);
        Task<ApiResponse<CandidateResponseDto>> UpdateCandidateAsync(string candidateId, UpdateCandidateDto updateCandidateDto);
    }
}
