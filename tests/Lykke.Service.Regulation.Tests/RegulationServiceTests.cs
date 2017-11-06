using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Regulation.Core.Domain;
using Lykke.Service.Regulation.Core.Repositories;
using Lykke.Service.Regulation.Services;
using Lykke.Service.Regulation.Services.Exceptions;
using Moq;
using Xunit;

namespace Lykke.Service.Regulation.Tests
{
    public class RegulationServiceTests
    {
        private readonly Mock<IRegulationRepository> _regulationRepositoryMock;
        private readonly Mock<IClientRegulationRepository> _clientRegulationRepositoryMock;
        private readonly Mock<IWelcomeRegulationRuleRepository> _welcomeRegulationRuleRepositoryMock;

        private readonly RegulationService _service;

        public RegulationServiceTests()
        {
            _regulationRepositoryMock = new Mock<IRegulationRepository>();
            _clientRegulationRepositoryMock = new Mock<IClientRegulationRepository>();
            _welcomeRegulationRuleRepositoryMock = new Mock<IWelcomeRegulationRuleRepository>();

            _service = new RegulationService(
                _regulationRepositoryMock.Object,
                _clientRegulationRepositoryMock.Object,
                _welcomeRegulationRuleRepositoryMock.Object);
        }

        [Fact]
        public async void GetAsync_Throw_Exception_If_Regulation_Does_Not_Exist()
        {
            // arrange
            const string regulationId = "ID";

            _regulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IRegulation>(null));

            // act
            Task task = _service.GetAsync(regulationId);

            // assert
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(async () => await task);
            Assert.Equal("Regulation not found.", exception.Message);
        }

        [Fact]
        public async void DeleteAsync_Can_Not_Delete_If_Regulation_Does_Not_Exist()
        {
            // arrange
            const string regulationId = "ID";

            _regulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IRegulation>(null));

            // act
            Task task = _service.DeleteAsync(regulationId);

            // assert
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(async () => await task);
            Assert.Equal("Regulation not found.", exception.Message);
        }

        [Fact]
        public async void DeleteAsync_Can_Not_Delete_If_Regulation_Assigned_With_Client()
        {
            // arrange
            const string regulationId = "ID";

            _regulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new Core.Domain.Regulation());

            _clientRegulationRepositoryMock
                .Setup(o => o.GetByRegulationIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<ClientRegulation>
                {
                    new ClientRegulation()
                });

            // act
            Task task = _service.DeleteAsync(regulationId);

            // assert
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(async () => await task);
            Assert.Equal("Can not delete regulation associated with one or more clients.", exception.Message);
        }

        [Fact]
        public async void DeleteAsync_Can_Not_Delete_If_Regulation_Assigned_With_Welcome_Rule()
        {
            // arrange
            const string regulationId = "ID";

            _regulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new Core.Domain.Regulation());

            _clientRegulationRepositoryMock
                .Setup(o => o.GetByRegulationIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<ClientRegulation>());

            _welcomeRegulationRuleRepositoryMock
                .Setup(o => o.GetByRegulationIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<WelcomeRegulationRule>
                {
                    new WelcomeRegulationRule()
                });

            // act
            Task task = _service.DeleteAsync(regulationId);

            // assert
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(async () => await task);
            Assert.Equal("Can not delete regulation associated with one or more welcome rules.", exception.Message);
        }
    }
}
