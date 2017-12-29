using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.Regulation.Core.Domain;
using Lykke.Service.Regulation.Core.Exceptions;
using Lykke.Service.Regulation.Core.Repositories;
using Lykke.Service.Regulation.Services;
using Moq;
using Xunit;

namespace Lykke.Service.Regulation.Tests
{
    public class ClientMarginRegulationServiceTests
    {
        private readonly Mock<IRegulationRepository> _regulationRepositoryMock;
        private readonly Mock<IClientMarginRegulationRepository> _clientMarginRegulationRepositoryMock;

        private readonly ClientMarginRegulationService _service;

        public ClientMarginRegulationServiceTests()
        {
            var logMock = new Mock<ILog>();
            _regulationRepositoryMock = new Mock<IRegulationRepository>();
            _clientMarginRegulationRepositoryMock = new Mock<IClientMarginRegulationRepository>();

            _service = new ClientMarginRegulationService(
                _regulationRepositoryMock.Object,
                _clientMarginRegulationRepositoryMock.Object,
                logMock.Object);
        }

        [Fact]
        public async void AddAsync_Throw_Exception_If_Regulation_Does_Not_Exist()
        {
            // arrange
            const string regulationId = "ID";
            const string clientId = "me";

            _regulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IRegulation>(null));

            // act
            Task task = _service.AddAsync(clientId, regulationId);

            // assert
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(async () => await task);
            Assert.Equal("Regulation not found.", exception.Message);
        }

        [Fact]
        public async void AddAsync_Throw_Exception_If_Already_Exists()
        {
            // arrange
            const string regulationId = "ID";
            const string clientId = "me";

            _regulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IRegulation>(new Core.Domain.Regulation
                {
                    Id = regulationId
                }));

            _clientMarginRegulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IClientMarginRegulation>(new ClientMarginRegulation
                {
                    ClientId = clientId,
                    RegulationId = regulationId
                }));

            // act
            Task task = _service.AddAsync(clientId, regulationId);

            // assert
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(async () => await task);
            Assert.Equal("Client margin regulation already exists.", exception.Message);
        }

        [Fact]
        public async Task SetAsync_Throw_Exception_If_Regulation_Does_Not_Exist()
        {
            // arrange
            const string regulationId = "ID";
            const string clientId = "me";

            _regulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IRegulation>(null));

            // act
            Task task = _service.SetAsync(clientId, regulationId);

            // assert
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(async () => await task);
            Assert.Equal("Regulation not found.", exception.Message);
        }

        [Fact]
        public async Task SetAsync_Delete_Existing_Client_Margin_Regulations_Befor_Adding()
        {
            // arrange
            const string regulationId = "ID";
            const string clientId = "me";

            _regulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IRegulation>(new Core.Domain.Regulation
                {
                    Id = regulationId
                }));

            // act
            await _service.SetAsync(clientId, regulationId);

            // assert
            _clientMarginRegulationRepositoryMock.Verify(o => o.DeleteAsync(It.IsAny<string>()));
        }
        
        [Fact]
        public async Task SetAsync_Insert_Method_Called()
        {
            // arrange
            const string regulationId = "ID";
            const string clientId = "me";

            _regulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IRegulation>(new Core.Domain.Regulation
                {
                    Id = regulationId
                }));

            // act
            await _service.SetAsync(clientId, regulationId);

            // assert
            _clientMarginRegulationRepositoryMock.Verify(o => o.InsertAsync(It.IsAny<ClientMarginRegulation>()));
        }
    }
}
