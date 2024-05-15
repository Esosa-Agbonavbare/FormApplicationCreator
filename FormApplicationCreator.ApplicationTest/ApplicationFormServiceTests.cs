using AutoMapper;
using FormApplicationCreator.Application.DTOs;
using FormApplicationCreator.Application.Services.Implementation;
using FormApplicationCreator.Application.Services.Interface;
using FormApplicationCreator.Domain.Entities;
using FormApplicationCreator.Domain;
using FormApplicationCreator.Persistence.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Moq;
using FluentAssertions;

namespace FormApplicationCreator.ApplicationTest
{
    public class ApplicationFormServiceTests
    {
        private readonly Mock<IApplicationFormRepository> _applicationFormRepositoryMock;
        private readonly Mock<IQuestionRepository> _questionRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly IApplicationFormService _applicationFormService;

        public ApplicationFormServiceTests()
        {
            _applicationFormRepositoryMock = new Mock<IApplicationFormRepository>();
            _questionRepositoryMock = new Mock<IQuestionRepository>();
            _mapperMock = new Mock<IMapper>();
            _applicationFormService = new ApplicationFormService(_applicationFormRepositoryMock.Object, _questionRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task AddApplicationFormAsync_ShouldReturnSuccess_WhenApplicationFormIsAdded()
        {
            // Arrange
            var addApplicationFormDto = new AddApplicationFormDto
            {
                Title = "Form 1",
                Questions =
                [
                    new AddQuestionDto { QuestionText = "Question 1" }
                ]
            };
            var applicationForm = new ApplicationForm { Id = "f1", Title = "Form 1" };
            var question = new Question { ApplicationFormId = "f1", QuestionText = "Question 1" };
            _mapperMock.Setup(m => m.Map<ApplicationForm>(addApplicationFormDto)).Returns(applicationForm);
            _mapperMock.Setup(m => m.Map<Question>(It.IsAny<AddQuestionDto>())).Returns(question);
            _mapperMock.Setup(m => m.Map<ApplicationFormResponseDto>(applicationForm)).Returns(new ApplicationFormResponseDto { Id = "f1", Title = "Form 1" });
            _applicationFormRepositoryMock.Setup(repo => repo.AddAsync(applicationForm)).Returns(Task.CompletedTask);
            _questionRepositoryMock.Setup(repo => repo.AddListAsync(It.IsAny<IEnumerable<Question>>())).Returns(Task.CompletedTask);

            // Act
            var result = await _applicationFormService.AddApplicationFormAsync(addApplicationFormDto);

            // Assert
            result.Should().NotBeNull();
            result.IsSuceeded.Should().BeTrue();
            result.StatusCode.Should().Be(StatusCodes.Status201Created);
            result.Message.Should().Be("Application form added successfully.");
        }

        [Fact]
        public async Task DeleteApplicationFormAsync_ShouldReturnTrue_WhenApplicationFormIsDeleted()
        {
            // Arrange
            var applicationFormId = "f1";
            var applicationForm = new ApplicationForm { Id = applicationFormId };
            _applicationFormRepositoryMock.Setup(repo => repo.GetByIdAsync(applicationFormId)).ReturnsAsync(applicationForm);
            _applicationFormRepositoryMock.Setup(repo => repo.DeleteAsync(applicationForm)).Returns(Task.CompletedTask);

            // Act
            var result = await _applicationFormService.DeleteApplicationFormAsync(applicationFormId);

            // Assert
            result.Should().NotBeNull();
            result.IsSuceeded.Should().BeTrue();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Message.Should().Be("Application form deleted successfully.");
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnApplicationForms_WhenApplicationFormsExist()
        {
            // Arrange
            var applicationForms = new List<ApplicationForm>
            {
                new() { Id = "f1", Title = "Form 1" },
                new() { Id = "f2", Title = "Form 2" }
            };
            var applicationFormResponseDtos = new List<ApplicationFormResponseDto>
            {
                new() { Id = "f1", Title = "Form 1" },
                new() { Id = "f2", Title = "Form 2" }
            };
            _applicationFormRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(applicationForms);
            _mapperMock.Setup(m => m.Map<List<ApplicationFormResponseDto>>(applicationForms)).Returns(applicationFormResponseDtos);
            foreach (var applicationForm in applicationForms)
            {
                _questionRepositoryMock.Setup(repo => repo.GetAllByApplicationFormIdAsync(applicationForm.Id))
                    .ReturnsAsync([]);
            }

            // Act
            var result = await _applicationFormService.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().HaveCount(2);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnApplicationForm_WhenApplicationFormExists()
        {
            // Arrange
            var applicationFormId = "f1";
            var applicationForm = new ApplicationForm { Id = applicationFormId, Title = "Form 1" };
            var questions = new List<Question>
            {
                new() { ApplicationFormId = applicationFormId, QuestionText = "Question 1" }
            };
            _applicationFormRepositoryMock.Setup(repo => repo.GetByIdAsync(applicationFormId)).ReturnsAsync(applicationForm);
            _questionRepositoryMock.Setup(repo => repo.GetAllByApplicationFormIdAsync(applicationFormId)).ReturnsAsync(questions);
            _mapperMock.Setup(m => m.Map<ApplicationFormResponseDto>(applicationForm)).Returns(new ApplicationFormResponseDto { Id = applicationFormId, Title = "Form 1" });
            _mapperMock.Setup(m => m.Map<List<QuestionResponseDto>>(questions)).Returns(new List<QuestionResponseDto>
            {
                new() { QuestionText = "Question 1" }
            });

            // Act
            var result = await _applicationFormService.GetByIdAsync(applicationFormId);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(applicationFormId);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task UpdateApplicationFormAsync_ShouldReturnSuccess_WhenApplicationFormIsUpdated()
        {
            // Arrange
            var applicationFormId = "f1";
            var updateApplicationFormDto = new UpdateApplicationFormDto
            {
                Title = "Form 1 Updated",
                Questions = new List<AddQuestionDto>
                {
                    new AddQuestionDto { QuestionText = "Updated Question 1" }
                }
            };
            var existingApplicationForm = new ApplicationForm { Id = applicationFormId, Title = "Form 1" };
            _applicationFormRepositoryMock.Setup(repo => repo.GetByIdAsync(applicationFormId)).ReturnsAsync(existingApplicationForm);
            _applicationFormRepositoryMock.Setup(repo => repo.UpdateAsync(existingApplicationForm)).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map(updateApplicationFormDto, existingApplicationForm)).Returns(existingApplicationForm);
            var updatedQuestions = new List<Question>
            {
                new Question { ApplicationFormId = applicationFormId, QuestionText = "Updated Question 1" }
            };
            _mapperMock.Setup(m => m.Map<List<Question>>(updateApplicationFormDto.Questions)).Returns(updatedQuestions);
            _questionRepositoryMock.Setup(repo => repo.UpdateByApplicationFormIdAsync(applicationFormId, updatedQuestions)).Returns(Task.CompletedTask);
            var updatedApplicationFormResponseDto = new ApplicationFormResponseDto { Id = applicationFormId, Title = "Form 1 Updated", Questions = [new() { QuestionText = "Updated Question 1" }] };
            _mapperMock.Setup(m => m.Map<ApplicationFormResponseDto>(existingApplicationForm)).Returns(updatedApplicationFormResponseDto);

            // Act
            var result = await _applicationFormService.UpdateApplicationFormAsync(applicationFormId, updateApplicationFormDto);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(applicationFormId);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Message.Should().Be("Application form updated successfully.");
        }
    }
}
