using FormApplicationCreator.Application.DTOs;
using FormApplicationCreator.Application.Services.Interface;
using FormApplicationCreator.Domain;
using Microsoft.AspNetCore.Mvc;

namespace FormApplicationCreator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidateController : ControllerBase
    {
        private readonly ICandidateService _candidateService;

        public CandidateController(ICandidateService candidateService)
        {
            _candidateService = candidateService;
        }

        [HttpPost]
        public async Task<IActionResult> AddCandidate([FromBody] AddCandidateDto addCandidateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<string>(false, StatusCodes.Status400BadRequest, "Invalid model state", ModelState.Values
                    .SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));
            }
            return Ok(await _candidateService.AddCandidateAsync(addCandidateDto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCandidate(string id, [FromBody] UpdateCandidateDto updateCandidateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<string>(false, StatusCodes.Status400BadRequest, "Invalid model state", ModelState.Values
                    .SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));
            }

            return Ok(await _candidateService.UpdateCandidateAsync(id, updateCandidateDto));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCandidates()
        {
            return Ok(await _candidateService.GetAllCandidatesAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCandidateById(string id)
        {
            return Ok(await _candidateService.GetCandidateByIdAsync(id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCandidate(string id)
        {
            return Ok(await _candidateService.DeleteCandidateAsync(id));
        }
    }
}
