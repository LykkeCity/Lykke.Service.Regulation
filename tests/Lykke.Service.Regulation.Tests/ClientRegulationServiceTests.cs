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

        private readonly ClientRegulationService _service;

        public ClientRegulationServiceTests()
        {
            _regulationRepositoryMock = new Mock<IRegulationRepository>();
            _clientRegulationRepositoryMock = new Mock<IClientRegulationRepository>();

            _service = new ClientRegulationService(
                _regulationRepositoryMock.Object,
                _clientRegulationRepositoryMock.Object);
        }

        [Fact]
        public async void AddAsync_Ok()
        {
            // arrange
            const string regulationId = "ID";
            const string clientId = "me";

            IClientRegulation result = null;

            _regulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new Core.Domain.Regulation
                {
                    Id = regulationId
                });

            _clientRegulationRepositoryMock.Setup(o => o.AddAsync(It.IsAny<IClientRegulation>()))
                .Returns(Task.FromResult(false))
                .Callback((IClientRegulation p) => result = p);

            // act
            await _service.AddAsync(new ClientRegulation
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

            _regulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IRegulation>(null));

            // act
            Task task = _service.AddAsync(new ClientRegulation
            {
                ClientId = clientId,
                RegulationId = regulationId
            });

            // assert
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(async () => await task);
            Assert.Equal("Regulation not found.", exception.Message);
        }

        [Fact]
        public async void SetKycAsync_Can_Not_Set_KYC_If_Client_Regulation_Does_Not_Exist()
        {
            // arrange
            const string regulationId = "ID";
            const string clientId = "me";

            _clientRegulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult<IClientRegulation>(null));

            // act
            Task task = _service.SetKycAsync(clientId, regulationId);

            // assert
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(async () => await task);
            Assert.Equal("Client regulation not found.", exception.Message);
        }

        [Fact]
        public async void SetKycAsync_OK()
        {
            // arrange
            const string regulationId = "ID";
            const string clientId = "me";

            var clientRegulation = new ClientRegulation
            {
                Id = "0",
                ClientId = clientId,
                RegulationId = regulationId,
                Active = false,
                Kyc = false
            };

            _clientRegulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(clientRegulation);

            _clientRegulationRepositoryMock.Setup(o => o.UpdateAsync(It.IsAny<IClientRegulation>()))
                .Returns(Task.CompletedTask);

            // act
            await _service.SetKycAsync(clientId, regulationId);

            // assert
            Assert.True(clientRegulation.Kyc);
        }

        [Fact]
        public async void SetActiveAsync_Can_Not_Set_Active_If_Client_Regulation_Does_Not_Exist()
        {
            // arrange
            const string regulationId = "ID";
            const string clientId = "me";

            _clientRegulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult<IClientRegulation>(null));

            // act
            Task task = _service.SetActiveAsync(clientId, regulationId, true);

            // assert
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(async () => await task);
            Assert.Equal("Client regulation not found.", exception.Message);
        }

        [Fact]
        public async void SetActiveAsync_OK()
        {
            // arrange
            const string regulationId = "ID";
            const string clientId = "me";

            var clientRegulation = new ClientRegulation
            {
                Id = "0",
                ClientId = clientId,
                RegulationId = regulationId,
                Active = false,
                Kyc = false
            };

            _clientRegulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(clientRegulation);

            _clientRegulationRepositoryMock.Setup(o => o.UpdateAsync(It.IsAny<IClientRegulation>()))
                .Returns(Task.CompletedTask);

            // act
            await _service.SetActiveAsync(clientId, regulationId, true);

            // assert
            Assert.True(clientRegulation.Active);
        }


        [Fact]
        public async void DeleteAsync_Can_Not_Delete_KYC_Regulation()
        {
            // arrange
            const string regulationId = "ID";
            const string clientId = "me";

            _clientRegulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new ClientRegulation
                {
                    Id = "0",
                    ClientId = clientId,
                    RegulationId = regulationId,
                    Active = false,
                    Kyc = true
                });

            // act
            Task task = _service.DeleteAsync(clientId, regulationId);

            // assert
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(async () => await task);
            Assert.Equal("Can not delete KYC client regulation.", exception.Message);
        }

        [Fact]
        public async void DeleteAsync_Can_Not_Delete_Active_Regulation()
        {
            // arrange
            const string regulationId = "ID";
            const string clientId = "me";

            _clientRegulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new ClientRegulation
                {
                    Id = "0",
                    ClientId = clientId,
                    RegulationId = regulationId,
                    Active = true,
                    Kyc = false
                });

            // act
            Task task = _service.DeleteAsync(clientId, regulationId);

            // assert
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(async () => await task);
            Assert.Equal("Can not delete active client regulation.", exception.Message);
        }
    }
}
