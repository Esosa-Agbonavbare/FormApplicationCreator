using FormApplicationCreator.Application.DTOs;
using FormApplicationCreator.Application.Services.Interface;
using FormApplicationCreator.Domain.Enums;
using FormApplicationCreator.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FormApplicationCreator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;

        public QuestionController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [HttpGet("{questionId}")]
        public async Task<IActionResult> GetQuestionById(string questionId)
        {
            return Ok(await _questionService.GetQuestionByIdAsync(questionId));
        }

        [HttpPost("{applicationFormId}/questions")]
        public async Task<IActionResult> AddQuestion(string applicationFormId, [FromBody] AddQuestionDto addQuestionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<string>(false, StatusCodes.Status400BadRequest, "Invalid model state", ModelState.Values
                    .SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));
            }
            return Ok(await _questionService.AddQuestionAsync(applicationFormId, addQuestionDto));
        }

        [HttpPut("{questionId}")]
        public async Task<IActionResult> UpdateQuestion(string questionId, UpdateQuestionDto updateQuestionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<string>(false, StatusCodes.Status400BadRequest, "Invalid model state", ModelState.Values
                    .SelectMany(x => x.Errors).Select(x => x.ErrorMessage).ToList()));
            }
            return Ok(await _questionService.UpdateQuestionAsync(questionId, updateQuestionDto));
        }

        [HttpDelete("{questionId}")]
        public async Task<IActionResult> DeleteQuestion(string questionId)
        {
            return Ok(await _questionService.DeleteQuestionAsync(questionId));
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetQuestionTypes()
        {
            var questionTypes = await _questionService.GetQuestionTypesAsync();
            return Ok(new ApiResponse<List<string>>(true, StatusCodes.Status200OK, "Question types retrieved successfully.", null, questionTypes));
        }
    }
}
