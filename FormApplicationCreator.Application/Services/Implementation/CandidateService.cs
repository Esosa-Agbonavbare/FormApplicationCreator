using AutoMapper;
using FormApplicationCreator.Application.DTOs;
using FormApplicationCreator.Application.Services.Interface;
using FormApplicationCreator.Domain.Entities;
using FormApplicationCreator.Domain;
using FormApplicationCreator.Persistence.Repositories.Interface;
using Microsoft.AspNetCore.Http;

namespace FormApplicationCreator.Application.Services.Implementation
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateRepository _candidateRepository;
        private readonly IMapper _mapper;
        private readonly IResponseRepository _responseRepository;

        public CandidateService(ICandidateRepository candidateRepository, IMapper mapper, IResponseRepository responseRepository)
        {
            _candidateRepository = candidateRepository;
            _mapper = mapper;
            _responseRepository = responseRepository;
        }

        public async Task<ApiResponse<CandidateResponseDto>> AddCandidateAsync(AddCandidateDto addCandidateDto)
        {
            try
            {
                var candidate = _mapper.Map<Candidate>(addCandidateDto);
                await _candidateRepository.AddAsync(candidate);
                var responses = addCandidateDto.Responses.Select(r => _mapper.Map<Response>(r));
                await _responseRepository.AddAsync(responses);
                var candidateResponseDto = _mapper.Map<CandidateResponseDto>(candidate);
                return new ApiResponse<CandidateResponseDto>(true, StatusCodes.Status200OK, "Candidate application added successfully.", candidateResponseDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while adding candidate application: {ex.Message}");
                return new ApiResponse<CandidateResponseDto>(false, StatusCodes.Status500InternalServerError, "An error occurred while adding the candidate application.", [ex.Message]);
            }
        }

        public async Task<ApiResponse<bool>> DeleteCandidateAsync(string candidateId)
        {
            try
            {
                var existingCandidate = await _candidateRepository.GetByIdAsync(candidateId);
                if (existingCandidate == null)
                {
                    return new ApiResponse<bool>(false, StatusCodes.Status404NotFound, "Candidate not found.");
                }
                await _candidateRepository.DeleteAsync(existingCandidate);
                return new ApiResponse<bool>(true, StatusCodes.Status200OK, "Candidate deleted successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while deleting candidate: {ex.Message}");
                return new ApiResponse<bool>(false, StatusCodes.Status500InternalServerError, "An error occurred while deleting the candidate.", [ex.Message]);
            }
        }

        public async Task<ApiResponse<List<CandidateResponseDto>>> GetAllCandidatesAsync()
        {
            try
            {
                var candidates = await _candidateRepository.GetAllAsync();
                var candidateResponseDtos = _mapper.Map<List<CandidateResponseDto>>(candidates);
                foreach (var candidateResponseDto in candidateResponseDtos)
                {
                    var responses = await _responseRepository.GetAllByCandidateIdAsync(candidateResponseDto.Id);
                    candidateResponseDto.Responses = _mapper.Map<List<ResponseDto>>(responses);
                }
                return new ApiResponse<List<CandidateResponseDto>>(true, StatusCodes.Status200OK, "Candidates retrieved successfully.", candidateResponseDtos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while retrieving candidates: {ex.Message}");
                return new ApiResponse<List<CandidateResponseDto>>(false, StatusCodes.Status500InternalServerError, "An error occurred while retrieving candidates.", [ex.Message]);
            }
        }

        public async Task<ApiResponse<CandidateResponseDto>> GetCandidateByIdAsync(string candidateId)
        {
            try
            {
                var candidate = await _candidateRepository.GetByIdAsync(candidateId);
                if (candidate == null)
                {
                    return new ApiResponse<CandidateResponseDto>(false, StatusCodes.Status404NotFound, "Candidate not found.");
                }
                var responses = await _responseRepository.GetAllByCandidateIdAsync(candidateId);
                var candidateResponseDto = _mapper.Map<CandidateResponseDto>(candidate);
                candidateResponseDto.Responses = _mapper.Map<List<ResponseDto>>(responses);
                return new ApiResponse<CandidateResponseDto>(true, StatusCodes.Status200OK, "Candidate retrieved successfully.", candidateResponseDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while retrieving candidate: {ex.Message}");
                return new ApiResponse<CandidateResponseDto>(false, StatusCodes.Status500InternalServerError, "An error occurred while retrieving the candidate.", [ex.Message]);
            }
        }

        public async Task<ApiResponse<CandidateResponseDto>> UpdateCandidateAsync(string candidateId, UpdateCandidateDto updateCandidateDto)
        {
            try
            {
                var existingCandidate = await _candidateRepository.GetByIdAsync(candidateId);
                if (existingCandidate == null)
                {
                    return new ApiResponse<CandidateResponseDto>(false, StatusCodes.Status404NotFound, "Candidate not found.");
                }
                _mapper.Map(updateCandidateDto, existingCandidate);
                await _candidateRepository.UpdateAsync(existingCandidate);
                var updatedResponses = _mapper.Map<List<Response>>(updateCandidateDto.Responses);
                await _responseRepository.UpdateByCandidateIdAsync(candidateId, updatedResponses);
                var candidateResponseDto = _mapper.Map<CandidateResponseDto>(existingCandidate);
                candidateResponseDto.Responses = _mapper.Map<List<ResponseDto>>(await _responseRepository.GetAllByCandidateIdAsync(candidateId));
                return new ApiResponse<CandidateResponseDto>(true, StatusCodes.Status200OK, "Candidate updated successfully.", candidateResponseDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while updating candidate: {ex.Message}");
                return new ApiResponse<CandidateResponseDto>(false, StatusCodes.Status500InternalServerError, "An error occurred while updating the candidate.", [ex.Message]);
            }
        }
    }
}
