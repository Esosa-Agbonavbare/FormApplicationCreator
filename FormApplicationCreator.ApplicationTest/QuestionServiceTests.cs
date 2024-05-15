using AutoMapper;
using FluentAssertions;
using FormApplicationCreator.Application.DTOs;
using FormApplicationCreator.Application.Services.Implementation;
using FormApplicationCreator.Application.Services.Interface;
using FormApplicationCreator.Domain.Entities;
using FormApplicationCreator.Domain.Enums;
using FormApplicationCreator.Persistence.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Moq;

namespace FormApplicationCreator.ApplicationTest
{
    public class QuestionServiceTests
    {
        private readonly Mock<IQuestionRepository> _questionRepositoryMock;
        private readonly Mock<IApplicationFormRepository> _applicationFormRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IQuestionService _questionService;

        public QuestionServiceTests()
        {
            _questionRepositoryMock = new Mock<IQuestionRepository>();
            _applicationFormRepositoryMock = new Mock<IApplicationFormRepository>();
            _mapperMock = new Mock<IMapper>();
            _questionService = new QuestionService(_questionRepositoryMock.Object, _applicationFormRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task AddQuestionAsync_ShouldReturnSuccess_WhenQuestionIsAdded()
        {
            // Arrange
            var applicationFormId = "test-form-id";
            var addQuestionDto = new AddQuestionDto { QuestionText = "Sample question", QuestionType = QuestionType.Paragraph };
            var question = new Question { Id = "q1", ApplicationFormId = applicationFormId, QuestionText = "Sample question", QuestionType = QuestionType.Paragraph };
            _mapperMock.Setup(m => m.Map<Question>(addQuestionDto)).Returns(question);
            _mapperMock.Setup(m => m.Map<QuestionResponseDto>(question)).Returns(new QuestionResponseDto { QuestionId = question.Id, QuestionText = question.QuestionText });
            _questionRepositoryMock.Setup(repo => repo.AddAsync(question)).Returns(Task.CompletedTask);

            // Act
            var result = await _questionService.AddQuestionAsync(applicationFormId, addQuestionDto);

            // Assert
            result.Should().NotBeNull();
            result.IsSuceeded.Should().BeTrue();
            result.StatusCode.Should().Be(StatusCodes.Status201Created);
            result.Message.Should().Be("Question added successfully.");
        }

        [Fact]
        public async Task DeleteQuestionAsync_ShouldReturnTrue_WhenQuestionIsDeleted()
        {
            // Arrange
            var questionId = "q1";
            var question = new Question { Id = questionId };

            _questionRepositoryMock.Setup(repo => repo.GetByIdAsync(questionId)).ReturnsAsync(question);
            _questionRepositoryMock.Setup(repo => repo.DeleteAsync(questionId)).Returns(Task.CompletedTask);

            // Act
            var result = await _questionService.DeleteQuestionAsync(questionId);

            // Assert
            result.Should().NotBeNull();
            result.IsSuceeded.Should().BeTrue();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Message.Should().Be("Question deleted successfully.");
        }

        [Fact]
        public async Task GetQuestionByIdAsync_ShouldReturnQuestion_WhenQuestionExists()
        {
            // Arrange
            var questionId = "q1";
            var question = new Question { Id = questionId, QuestionText = "Sample question", QuestionType = QuestionType.Paragraph };
            _questionRepositoryMock.Setup(repo => repo.GetByIdAsync(questionId)).ReturnsAsync(question);
            _mapperMock.Setup(m => m.Map<QuestionResponseDto>(question)).Returns(new QuestionResponseDto { QuestionId = questionId, QuestionText = question.QuestionText });

            // Act
            var result = await _questionService.GetQuestionByIdAsync(questionId);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.QuestionId.Should().Be(questionId);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task GetQuestionsAsync_ShouldReturnQuestions_WhenQuestionsExist()
        {
            // Arrange
            var applicationFormId = "test-form-id";
            var questions = new List<Question>
            {
                new() { Id = "q1", ApplicationFormId = applicationFormId, QuestionText = "Question 1", QuestionType = QuestionType.Paragraph },
                new() { Id = "q2", ApplicationFormId = applicationFormId, QuestionText = "Question 2", QuestionType = QuestionType.Multiple_Choice }
            };
            _applicationFormRepositoryMock.Setup(repo => repo.GetByIdAsync(applicationFormId)).ReturnsAsync(new ApplicationForm { Id = applicationFormId });
            _questionRepositoryMock.Setup(repo => repo.GetAllByApplicationFormIdAsync(applicationFormId)).ReturnsAsync(questions);
            _mapperMock.Setup(m => m.Map<List<QuestionResponseDto>>(questions)).Returns(new List<QuestionResponseDto>
            {
                new() { QuestionId = "q1", QuestionText = "Question 1", QuestionType = QuestionType.Paragraph },
                new() { QuestionId = "q2", QuestionText = "Question 2", QuestionType = QuestionType.Multiple_Choice }
            });

            // Act
            var result = await _questionService.GetQuestionsAsync(applicationFormId);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().HaveCount(2);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task UpdateQuestionAsync_ShouldReturnSuccess_WhenQuestionIsUpdated()
        {
            // Arrange
            var questionId = "q1";
            var updateQuestionDto = new UpdateQuestionDto { QuestionText = "Updated question" };
            var question = new Question { Id = questionId, QuestionText = "Updated question" };
            _mapperMock.Setup(m => m.Map<Question>(updateQuestionDto)).Returns(question);
            _mapperMock.Setup(m => m.Map<QuestionResponseDto>(question)).Returns(new QuestionResponseDto { QuestionId = questionId, QuestionText = "Updated question" });
            _questionRepositoryMock.Setup(repo => repo.UpdateAsync(question)).Returns(Task.CompletedTask);

            // Act
            var result = await _questionService.UpdateQuestionAsync(questionId, updateQuestionDto);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.QuestionId.Should().Be(questionId);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Message.Should().Be("Question updated successfully.");
        }

        [Fact]
        public void GetQuestionTypesAsync_ShouldReturnQuestionTypes()
        {
            // Act
            var result = _questionService.GetQuestionTypesAsync();

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().Contain(Enum.GetNames(typeof(QuestionType)));
        }

    }
}
