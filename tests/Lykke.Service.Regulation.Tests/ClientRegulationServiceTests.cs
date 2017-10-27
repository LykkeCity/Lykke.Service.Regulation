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
    public class ClientRegulationServiceTests
    {
        private readonly Mock<IRegulationRepository> _regulationRepositoryMock;
        private readonly Mock<IClientRegulationRepository> _clientRegulationRepositoryMock;
        private readonly Mock<IClientAvailableRegulationRepository> _clientAvailableRegulationRepositoryMock;

        private readonly ClientRegulationService _service;

        public ClientRegulationServiceTests()
        {
            _regulationRepositoryMock = new Mock<IRegulationRepository>();
            _clientRegulationRepositoryMock = new Mock<IClientRegulationRepository>();
            _clientAvailableRegulationRepositoryMock = new Mock<IClientAvailableRegulationRepository>();

            _service = new ClientRegulationService(
                _regulationRepositoryMock.Object,
                _clientRegulationRepositoryMock.Object,
                _clientAvailableRegulationRepositoryMock.Object);
        }

        [Fact]
        public async void AddAsync_Ok()
        {
            // arrange
            const string regulationId = "ID";
            const string clientId = "me";

            IClientAvailableRegulation result = null;

            _regulationRepositoryMock.Setup(o => o.GetAsync(It.Is<string>(i => i == regulationId)))
                .ReturnsAsync(new Core.Domain.Regulation
                {
                    Id = regulationId,
                    RequiresKYC = true
                });

            _clientAvailableRegulationRepositoryMock.Setup(o => o.AddAsync(It.IsAny<IClientAvailableRegulation>()))
                .Returns(Task.FromResult(false))
                .Callback((IClientAvailableRegulation p) => result = p);

            // act
            await _service.AddAsync(new ClientAvailableRegulation
            {
                ClientId = clientId,
                RegulationId = regulationId
            });

            // assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void AddAsync_Can_Not_Add_Regulation_To_Client_If_It_Does_Not_Exist()
        {
            // arrange
            const string regulationId = "ID";
            const string clientId = "me";

            _regulationRepositoryMock.Setup(o => o.GetAsync(It.Is<string>(i => i == regulationId)))
                .Returns(Task.FromResult<IRegulation>(null));

            // act
            Task task = _service.AddAsync(new ClientAvailableRegulation
            {
                ClientId = clientId,
                RegulationId = regulationId
            });

            // assert
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(async () => await task);
            Assert.Equal($"Regulation '{regulationId}' not found'.", exception.Message);
        }

        [Fact]
        public async void AddAsync_Can_Not_Add_Regulation_To_Client_If_It_Not_KYC()
        {
            // arrange
            const string regulationId = "ID";
            const string clientId = "me";

            _regulationRepositoryMock.Setup(o => o.GetAsync(It.Is<string>(i => i == regulationId)))
                .ReturnsAsync(new Core.Domain.Regulation
                {
                    Id = regulationId,
                    RequiresKYC = false
                });

            // act
            Task task = _service.AddAsync(new ClientAvailableRegulation
            {
                ClientId = clientId,
                RegulationId = regulationId
            });

            // assert
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(async () => await task);
            Assert.Equal($"Regulation '{regulationId}' is not KYC'.", exception.Message);
        }

        [Fact]
        public async void SetAsync_Can_Not_Set_Not_Available_Regulation()
        {
            // arrange
            const string regulationId = "ID";
            const string clientId = "me";

            _clientAvailableRegulationRepositoryMock.Setup(o => o.GetByClientIdAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IEnumerable<IClientAvailableRegulation>>(new List<ClientAvailableRegulation>()));

            // act
            Task task = _service.SetAsync(new ClientRegulation
            {
                ClientId = clientId,
                RegulationId = regulationId
            });

            // assert
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(async () => await task);
            Assert.Equal($"Regulation '{regulationId}' is not available for client '{clientId}'.", exception.Message);
        }

        [Fact]
        public async void RemoveAvailableAsync_Can_Not_Remove_Client_Current_Regulation()
        {
            // arrange
            const string regulationId = "ID";
            const string clientId = "me";

            _clientRegulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new ClientRegulation
                {
                    ClientId = clientId,
                    RegulationId = regulationId
                });

            // act
            Task task = _service.RemoveAvailableAsync(clientId, regulationId);

            // assert
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(async () => await task);
            Assert.Equal($"Can not remove regulation '{regulationId}'. It is current regulation of client '{clientId}'.", exception.Message);
        }
    }
}
