using FormApplicationCreator.Application.DTOs;
using FormApplicationCreator.Application.Services.Interface;
using FormApplicationCreator.Domain;
using Microsoft.AspNetCore.Mvc;

namespace FormApplicationCreator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationFormController : ControllerBase
    {
        private readonly IApplicationFormService _applicationFormService;

        public ApplicationFormController(IApplicationFormService applicationFormService)
        {
            _applicationFormService = applicationFormService;
        }

        [HttpPost]
        public async Task<IActionResult> AddApplicationForm([FromBody] AddApplicationFormDto applicationFormDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<string>(false, StatusCodes.Status400BadRequest, "Invalid model state", ModelState.Values
                    .SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));
            }
            return Ok(await _applicationFormService.AddApplicationFormAsync(applicationFormDto));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplicationForm(string id)
        {
            return Ok(await _applicationFormService.DeleteApplicationFormAsync(id));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetApplicationFormById(string id)
        {
            return Ok(await _applicationFormService.GetByIdAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllApplicationForms()
        {
            return Ok(await _applicationFormService.GetAllAsync());
        }
    }
}
