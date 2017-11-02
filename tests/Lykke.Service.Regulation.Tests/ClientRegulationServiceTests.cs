using System.Collections.Generic;
using System.Linq;
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
        private readonly Mock<IWelcomeRegulationRuleRepository> _welcomeRegulationRuleRepositoryMock;

        private readonly ClientRegulationService _service;

        public ClientRegulationServiceTests()
        {
            _regulationRepositoryMock = new Mock<IRegulationRepository>();
            _clientRegulationRepositoryMock = new Mock<IClientRegulationRepository>();
            _welcomeRegulationRuleRepositoryMock = new Mock<IWelcomeRegulationRuleRepository>();

            _service = new ClientRegulationService(
                _regulationRepositoryMock.Object,
                _clientRegulationRepositoryMock.Object,
                _welcomeRegulationRuleRepositoryMock.Object);
        }

        [Fact]
        public async void GetAsync_Throw_Exception_If_Does_Not_Exist()
        {
            // arrange
            const string regulationId = "ID";
            const string clientId = "me";

            _clientRegulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult<IClientRegulation>(null));

            // act
            Task task = _service.GetAsync(clientId, regulationId);

            // assert
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(async () => await task);
            Assert.Equal("Client regulation not found.", exception.Message);
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
        public async void AddAsync_Throw_Exception_If_Regulation_Does_Not_Exist()
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
        public async void UpdateKycAsync_Throw_Exception_If_Client_Regulation_Does_Not_Exist()
        {
            // arrange
            const string regulationId = "ID";
            const string clientId = "me";

            _clientRegulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult<IClientRegulation>(null));

            // act
            Task task = _service.UpdateKycAsync(clientId, regulationId, true);

            // assert
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(async () => await task);
            Assert.Equal("Client regulation not found.", exception.Message);
        }

        [Fact]
        public async void UpdateKycAsync_OK()
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
            await _service.UpdateKycAsync(clientId, regulationId, true);

            // assert
            Assert.True(clientRegulation.Kyc);
        }

        [Fact]
        public async void UpdateActiveAsync_Throw_Exception_If_Client_Regulation_Does_Not_Exist()
        {
            // arrange
            const string regulationId = "ID";
            const string clientId = "me";

            _clientRegulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult<IClientRegulation>(null));

            // act
            Task task = _service.UpdateActiveAsync(clientId, regulationId, true);

            // assert
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(async () => await task);
            Assert.Equal("Client regulation not found.", exception.Message);
        }

        [Fact]
        public async void UpdateActiveAsync_OK()
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
            await _service.UpdateActiveAsync(clientId, regulationId, true);

            // assert
            Assert.True(clientRegulation.Active);
        }

        [Fact]
        public async void DeleteAsync_Throw_Exception_If_Does_Not_Exist()
        {
            // arrange
            const string regulationId = "ID";
            const string clientId = "me";

            _clientRegulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult<IClientRegulation>(null));

            // act
            Task task = _service.DeleteAsync(clientId, regulationId);

            // assert
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(async () => await task);
            Assert.Equal("Client regulation not found.", exception.Message);
        }

        [Fact]
        public async void DeleteAsync_Throw_Exception_If_KYC_True()
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
        public async void DeleteAsync_Throw_Exception_If_Active_True()
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

        [Fact]
        public async void SetDefaultAsync_Throw_Exception_If_Client_Alredy_Have_Regulations()
        {
            // arrange
            const string regulationId = "ID";
            const string clientId = "me";
            const string country = "uk";

            _clientRegulationRepositoryMock.Setup(o => o.GetByClientIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<ClientRegulation>
                {
                    new ClientRegulation
                    {
                        Id = "0",
                        ClientId = clientId,
                        RegulationId = regulationId,
                        Active = false,
                        Kyc = true
                    }
                });

            // act
            Task task = _service.SetDefaultAsync(clientId, country);

            // assert
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(async () => await task);
            Assert.Equal("Client already have regulations.", exception.Message);
        }

        [Fact]
        public async void SetDefaultAsync_Throw_Exception_If_No_Default_Regulations_For_Country()
        {
            // arrange
            const string clientId = "me";
            const string country = "uk";

            _clientRegulationRepositoryMock.Setup(o => o.GetByClientIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<ClientRegulation>());

            _welcomeRegulationRuleRepositoryMock.Setup(o => o.GetByCountryAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<WelcomeRegulationRule>());

            // act
            Task task = _service.SetDefaultAsync(clientId, country);

            // assert
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(async () => await task);
            Assert.Equal("No default regulations for country.", exception.Message);
        }

        [Fact]
        public async void SetDefaultAsync_OK()
        {
            // arrange
            const string clientId = "me";
            const string country = "uk";

            IEnumerable<IClientRegulation> result = null;

            _clientRegulationRepositoryMock.Setup(o => o.GetByClientIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<ClientRegulation>());

            _clientRegulationRepositoryMock.Setup(o => o.AddAsync(It.IsAny<IEnumerable<IClientRegulation>>()))
                .Returns(Task.CompletedTask)
                .Callback((IEnumerable<IClientRegulation> o) => { result = o; });

            _welcomeRegulationRuleRepositoryMock.Setup(o => o.GetByCountryAsync(It.Is<string>(c => c == country)))
                .ReturnsAsync(new List<WelcomeRegulationRule>
                {
                    new WelcomeRegulationRule
                    {
                        RegulationId = "ru",
                        Country = country
                    },
                    new WelcomeRegulationRule
                    {
                        RegulationId = "en",
                        Country = country
                    }
                });

            // act
            await _service.SetDefaultAsync(clientId, country);

            // assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }
    }
}
