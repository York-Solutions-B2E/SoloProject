using Moq;

using WebApi.Services; // Your service class namespace
using App.Shared.Dtos; // Where CreateCommunicationDto is
namespace App.UnitTests {
    public class CommunicationServiceTests
    {
        [Fact]
        public async Task GetCommunicationDetailsAsync_ReturnsExpectedResult()
        {
            //arrange
            var mockRepo = new Mock<ICommunicationRepository>();
            var expected = new CommunicationDetailsDto { Id = Guid.NewGuid(), Title = "Test", TypeCode = "EOB" };

            mockRepo.Setup(r => r.GetCommunicationDetailsAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(expected);

            var service = new CommunicationService(mockRepo.Object);
            //act
            var result = await service.GetCommunicationDetailsAsync(Guid.NewGuid());

            //assert
            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }

        [Fact]
        public async Task GetCommunicationDetailsAsync_ReturnsNull_WhenNotFound()
        {
            //arrange
            var mockRepo = new Mock<ICommunicationRepository>();
            var expected = new CommunicationDetailsDto { Id = Guid.NewGuid(), Title = "Test", TypeCode = "EOB" };

            mockRepo.Setup(r => r.GetCommunicationDetailsAsync(It.IsAny<Guid>()))
                    .ReturnsAsync((CommunicationDetailsDto?)null);

            var service = new CommunicationService(mockRepo.Object);
            //act
            var result = await service.GetCommunicationDetailsAsync(Guid.NewGuid());

            //assert
            Assert.Null(result);

        }

        [Fact]
        public async Task GetCommunications_ReturnsExpectedResult()
        {
            var mockRepo = new Mock<ICommunicationRepository>();
            var expected = new CommunicationDto { Id = Guid.NewGuid(), Title = "Test", CurrentStatus = "Created", TypeCode = "EOB" };
            
            mockRepo.Setup(r => r.GetCommunicationAsync(It.IsAny<Guid>()))
                    .ReturnsAsync(expected);

            var service = new CommunicationService(mockRepo.Object);

            var result = await service.GetCommunicationAsync(Guid.NewGuid());

            Assert.Equal(expected, result);

        }

    }
}
