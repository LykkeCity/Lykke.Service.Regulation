using System.Threading.Tasks;
using Lykke.Service.Regulation.Core.Domain;
using Lykke.Service.Regulation.Core.Repositories;
using Lykke.Service.Regulation.Services;
using Lykke.Service.Regulation.Services.Exceptions;
using Moq;
using Xunit;

namespace Lykke.Service.Regulation.Tests
{
    public class WelcomeRegulationRuleServiceTest
    {
        private readonly Mock<IRegulationRepository> _regulationRepositoryMock;
        private readonly Mock<IWelcomeRegulationRuleRepository> _welcomeRegulationRuleRepositoryMock;

        private readonly WelcomeRegulationRuleService _service;

        public WelcomeRegulationRuleServiceTest()
        {
            _regulationRepositoryMock = new Mock<IRegulationRepository>();
            _welcomeRegulationRuleRepositoryMock = new Mock<IWelcomeRegulationRuleRepository>();

            _service = new WelcomeRegulationRuleService(
                _regulationRepositoryMock.Object,
                _welcomeRegulationRuleRepositoryMock.Object);
        }

        [Fact]
        public async void GetAsync_Throw_Exception_If_Regulation_Rule_Does_Not_Exist()
        {
            // arrange
            const string regulationRuleId = "ID";

            _welcomeRegulationRuleRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IWelcomeRegulationRule>(null));

            // act
            Task task = _service.GetAsync(regulationRuleId);

            // assert
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(async () => await task);
            Assert.Equal("Regulation rule not found.", exception.Message);
        }

        [Fact]
        public async void GetByRegulationIdAsync_Throw_Exception_If_Regulation_Does_Not_Exist()
        {
            // arrange
            const string regulationId = "ID";

            _regulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IRegulation>(null));

            // act
            Task task = _service.GetByRegulationIdAsync(regulationId);

            // assert
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(async () => await task);
            Assert.Equal("Regulation not found.", exception.Message);
        }

        [Fact]
        public async void AddAsync_Throw_Exception_If_Regulation_Does_Not_Exist()
        {
            // arrange
            const string regulationId = "ID";

            var rule = new WelcomeRegulationRule
            {
                RegulationId = regulationId
            };

            _regulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IRegulation>(null));

            // act
            Task task = _service.AddAsync(rule);

            // assert
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(async () => await task);
            Assert.Equal("Regulation not found.", exception.Message);
        }

        [Fact]
        public async void UpdateAsync_Throw_Exception_If_Regulation_Does_Not_Exist()
        {
            // arrange
            const string regulationRuleId = "ID";

            _regulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IRegulation>(null));

            // act
            Task task = _service.UpdateAsync(new WelcomeRegulationRule
            {
                RegulationId = regulationRuleId
            });

            // assert
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(async () => await task);
            Assert.Equal("Regulation not found.", exception.Message);
        }

        [Fact]
        public async void UpdateAsync_Throw_Exception_If_Rule_Does_Not_Exist()
        {
            // arrange
            const string regulationRuleId = "ID";

            _regulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new Core.Domain.Regulation());

            _welcomeRegulationRuleRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IWelcomeRegulationRule>(null));

            // act
            Task task = _service.UpdateAsync(new WelcomeRegulationRule
            {
                RegulationId = regulationRuleId
            });

            // assert
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(async () => await task);
            Assert.Equal("Regulation rule not found.", exception.Message);
        }

        [Fact]
        public async void DeleteAsync_Throw_Exception_If_Rule_Does_Not_Exist()
        {
            // arrange
            const string regulationRuleId = "ID";

            _welcomeRegulationRuleRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IWelcomeRegulationRule>(null));

            // act
            Task task = _service.DeleteAsync(regulationRuleId);

            // assert
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(async () => await task);
            Assert.Equal("Regulation rule not found.", exception.Message);
        }
    }
}
