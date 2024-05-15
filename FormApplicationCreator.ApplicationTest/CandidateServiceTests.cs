using AutoMapper;
using FluentAssertions;
using FormApplicationCreator.Application.DTOs;
using FormApplicationCreator.Application.Services.Implementation;
using FormApplicationCreator.Domain.Entities;
using FormApplicationCreator.Persistence.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Moq;

namespace FormApplicationCreator.ApplicationTest
{
    public class CandidateServiceTests
    {
        private readonly Mock<ICandidateRepository> _candidateRepositoryMock;
        private readonly Mock<IResponseRepository> _responseRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CandidateService _candidateService;

        public CandidateServiceTests()
        {
            _candidateRepositoryMock = new Mock<ICandidateRepository>();
            _responseRepositoryMock = new Mock<IResponseRepository>();
            _mapperMock = new Mock<IMapper>();
            _candidateService = new CandidateService(_candidateRepositoryMock.Object, _responseRepositoryMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task AddCandidateAsync_ShouldReturnSuccess_WhenCandidateIsAdded()
        {
            // Arrange
            var addCandidateDto = new AddCandidateDto
            {
                FirstName = "John",
                LastName = "Doe",
                Responses =
                [
                    new AddResponseDto { Answer = "Answer 1" }
                ]
            };
            var candidate = new Candidate { Id = "c1", FirstName = "John", LastName = "Doe" };
            var response = new Response { CandidateId = "c1", Answer = "Answer 1" };
            _mapperMock.Setup(m => m.Map<Candidate>(addCandidateDto)).Returns(candidate);
            _mapperMock.Setup(m => m.Map<Response>(It.IsAny<AddResponseDto>())).Returns(response);
            _mapperMock.Setup(m => m.Map<CandidateResponseDto>(candidate)).Returns(new CandidateResponseDto { Id = "c1", FirstName = "John", LastName = "Doe" });
            _candidateRepositoryMock.Setup(repo => repo.AddAsync(candidate)).Returns(Task.CompletedTask);
            _responseRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<IEnumerable<Response>>())).Returns(Task.CompletedTask);

            // Act
            var result = await _candidateService.AddCandidateAsync(addCandidateDto);

            // Assert
            result.Should().NotBeNull();
            result.IsSuceeded.Should().BeTrue();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Message.Should().Be("Candidate application added successfully.");
        }

        [Fact]
        public async Task DeleteCandidateAsync_ShouldReturnTrue_WhenCandidateIsDeleted()
        {
            // Arrange
            var candidateId = "c1";
            var candidate = new Candidate { Id = candidateId };
            _candidateRepositoryMock.Setup(repo => repo.GetByIdAsync(candidateId)).ReturnsAsync(candidate);
            _candidateRepositoryMock.Setup(repo => repo.DeleteAsync(candidate)).Returns(Task.CompletedTask);

            // Act
            var result = await _candidateService.DeleteCandidateAsync(candidateId);

            // Assert
            result.Should().NotBeNull();
            result.IsSuceeded.Should().BeTrue();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Message.Should().Be("Candidate deleted successfully.");
        }

        [Fact]
        public async Task GetAllCandidatesAsync_ShouldReturnCandidates_WhenCandidatesExist()
        {
            // Arrange
            var candidates = new List<Candidate>
            {
                new() { Id = "c1", FirstName = "John", LastName = "Doe"  },
                new() { Id = "c2", FirstName = "John", LastName = "Doe"  }
            };
            var candidateResponseDtos = new List<CandidateResponseDto>
            {
                new() { Id = "c1", FirstName = "John", LastName = "Doe"  },
                new() { Id = "c2", FirstName = "John", LastName = "Doe"  }
            };
            _candidateRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(candidates);
            _mapperMock.Setup(m => m.Map<List<CandidateResponseDto>>(candidates)).Returns(candidateResponseDtos);
            foreach (var candidate in candidates)
            {
                _responseRepositoryMock.Setup(repo => repo.GetAllByCandidateIdAsync(candidate.Id))
                    .ReturnsAsync([]);
            }

            // Act
            var result = await _candidateService.GetAllCandidatesAsync();

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().HaveCount(2);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task GetCandidateByIdAsync_ShouldReturnCandidate_WhenCandidateExists()
        {
            // Arrange
            var candidateId = "c1";
            var candidate = new Candidate { Id = candidateId, FirstName = "John", LastName = "Doe" };
            var responses = new List<Response>
            {
                new() { CandidateId = candidateId, QuestionId = "q1", Answer = "Answer 1" }
            };
            _candidateRepositoryMock.Setup(repo => repo.GetByIdAsync(candidateId)).ReturnsAsync(candidate);
            _responseRepositoryMock.Setup(repo => repo.GetAllByCandidateIdAsync(candidateId)).ReturnsAsync(responses);
            _mapperMock.Setup(m => m.Map<CandidateResponseDto>(candidate)).Returns(new CandidateResponseDto { Id = candidateId, FirstName = "John", LastName = "Doe" });
            _mapperMock.Setup(m => m.Map<List<ResponseDto>>(responses)).Returns(
            [
                new ResponseDto { QuestionId = "q1", Answer = "Answer 1" }
            ]);

            // Act
            var result = await _candidateService.GetCandidateByIdAsync(candidateId);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(candidateId);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task UpdateCandidateAsync_ShouldReturnSuccess_WhenCandidateIsUpdated()
        {
            // Arrange
            var candidateId = "c1";
            var updateCandidateDto = new UpdateCandidateDto
            {
                FirstName = "John",
                LastName = "Daniels",
                Responses =
                [
                    new AddResponseDto { Answer = "Updated Answer 1" }
                ]
            };
            var existingCandidate = new Candidate { Id = candidateId, FirstName = "John", LastName = "Doe" };
            _candidateRepositoryMock.Setup(repo => repo.GetByIdAsync(candidateId)).ReturnsAsync(existingCandidate);
            _candidateRepositoryMock.Setup(repo => repo.UpdateAsync(existingCandidate)).Returns(Task.CompletedTask);
            _mapperMock.Setup(m => m.Map(updateCandidateDto, existingCandidate)).Returns(existingCandidate);
            var updatedResponses = new List<Response>
            {
                new() { CandidateId = candidateId, QuestionId = "q1", Answer = "Updated Answer 1" }
            };
            _mapperMock.Setup(m => m.Map<List<Response>>(updateCandidateDto.Responses)).Returns(updatedResponses);
            _responseRepositoryMock.Setup(repo => repo.UpdateByCandidateIdAsync(candidateId, updatedResponses)).Returns(Task.CompletedTask);
            _responseRepositoryMock.Setup(repo => repo.GetAllByCandidateIdAsync(candidateId)).ReturnsAsync(updatedResponses);

            var candidateResponseDto = new CandidateResponseDto { Id = candidateId, FirstName = "John", LastName = "Daniels", Responses = new List<ResponseDto> { new ResponseDto { QuestionId = "q1", Answer = "Updated Answer 1" } } };
            _mapperMock.Setup(m => m.Map<CandidateResponseDto>(existingCandidate)).Returns(candidateResponseDto);

            // Act
            var result = await _candidateService.UpdateCandidateAsync(candidateId, updateCandidateDto);

            // Assert
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();
            result.Data.Id.Should().Be(candidateId);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Message.Should().Be("Candidate updated successfully.");
        }
    }
}
