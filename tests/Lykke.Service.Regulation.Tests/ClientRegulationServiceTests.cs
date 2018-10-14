using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Service.Regulation.Core.Contracts;
using Lykke.Service.Regulation.Core.Domain;
using Lykke.Service.Regulation.Core.Repositories;
using Lykke.Service.Regulation.Core.Services;
using Lykke.Service.Regulation.Services;
using Lykke.Service.Regulation.Core.Exceptions;
using Moq;
using Xunit;
using ClientRegulation = Lykke.Service.Regulation.Core.Domain.ClientRegulation;

namespace Lykke.Service.Regulation.Tests
{
    public class ClientRegulationServiceTests
    {
        private readonly Mock<IRegulationRepository> _regulationRepositoryMock;
        private readonly Mock<IClientRegulationRepository> _clientRegulationRepositoryMock;
        private readonly Mock<IWelcomeRegulationRuleRepository> _welcomeRegulationRuleRepositoryMock;
        private readonly Mock<IClientRegulationPublisher> _clientRegulationPublisherMock;

        private readonly ClientRegulationService _service;

        public ClientRegulationServiceTests()
        {
            var logMock = new Mock<ILogFactory>();
            logMock.Setup(x => x.CreateLog(It.IsAny<object>())).Returns(EmptyLog.Instance);
            _regulationRepositoryMock = new Mock<IRegulationRepository>();
            _clientRegulationRepositoryMock = new Mock<IClientRegulationRepository>();
            _welcomeRegulationRuleRepositoryMock = new Mock<IWelcomeRegulationRuleRepository>();
            _clientRegulationPublisherMock = new Mock<IClientRegulationPublisher>();
            
            _service = new ClientRegulationService(
                _regulationRepositoryMock.Object,
                _clientRegulationRepositoryMock.Object,
                _welcomeRegulationRuleRepositoryMock.Object,
                _clientRegulationPublisherMock.Object,
                logMock.Object);
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
        public async void AddAsync_Publish_Ok()
        {
            // arrange
            const string regulationId = "ID";
            const string clientId = "me";

            bool published = false;

            _regulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(new Core.Domain.Regulation
                {
                    Id = regulationId
                });

            _clientRegulationPublisherMock.Setup(o => o.PublishAsync(It.IsAny<ClientRegulationsMessage>()))
                .Returns(Task.CompletedTask)
                .Callback(() => published = true);

            // act
            await _service.AddAsync(new ClientRegulation
            {
                ClientId = clientId,
                RegulationId = regulationId
            });

            // assert
            Assert.True(published);
        }

        [Fact]
        public async void SetDefaultAsync_Publish_Ok()
        {
            // arrange
            const string clientId = "me";
            const string country = "uk";

            bool published = false;

            WelcomeRegulationRule rule1 = Create("1", "reg_1", null, true, 100);

            _clientRegulationRepositoryMock.Setup(o => o.GetByClientIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<ClientRegulation>());

            _welcomeRegulationRuleRepositoryMock.Setup(o => o.GetByCountryAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<WelcomeRegulationRule>
                {
                    rule1
                });

            _clientRegulationPublisherMock.Setup(o => o.PublishAsync(It.IsAny<ClientRegulationsMessage>()))
                .Returns(Task.CompletedTask)
                .Callback(() => published = true);

            // act
            await _service.SetDefaultAsync(clientId, country);

            // assert
            Assert.True(published);
        }

        [Fact]
        public async void UpdateKycAsync_Publish_Ok()
        {
            // arrange
            const string regulationId = "ID";
            const string clientId = "me";

            bool published = false;

            _clientRegulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new ClientRegulation());

            _clientRegulationRepositoryMock.Setup(o => o.UpdateAsync(It.IsAny<IClientRegulation>()))
                .Returns(Task.CompletedTask);

            _clientRegulationPublisherMock.Setup(o => o.PublishAsync(It.IsAny<ClientRegulationsMessage>()))
                .Returns(Task.CompletedTask)
                .Callback(() => published = true);

            // act
            await _service.UpdateKycAsync(clientId, regulationId, true);

            // assert
            Assert.True(published);
        }

        [Fact]
        public async void UpdateActiveAsync_Publish_Ok()
        {
            // arrange
            const string regulationId = "ID";
            const string clientId = "me";

            bool published = false;

            _clientRegulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new ClientRegulation());

            _clientRegulationRepositoryMock.Setup(o => o.UpdateAsync(It.IsAny<IClientRegulation>()))
                .Returns(Task.CompletedTask);

            _clientRegulationPublisherMock.Setup(o => o.PublishAsync(It.IsAny<ClientRegulationsMessage>()))
                .Returns(Task.CompletedTask)
                .Callback(() => published = true);

            // act
            await _service.UpdateActiveAsync(clientId, regulationId, true);

            // assert
            Assert.True(published);
        }

        [Fact]
        public async void DeleteAsync_Publish_Ok()
        {
            // arrange
            const string regulationId = "ID";
            const string clientId = "me";

            bool published = false;

            _clientRegulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new ClientRegulation());

            _clientRegulationRepositoryMock.Setup(o => o.DeleteAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            _clientRegulationPublisherMock.Setup(o => o.PublishAsync(It.IsAny<ClientRegulationsMessage>()))
                .Returns(Task.CompletedTask)
                .Callback(() => published = true);

            // act
            await _service.UpdateActiveAsync(clientId, regulationId, true);

            // assert
            Assert.True(published);
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
        public async void AddAsync_Throw_Exception_If_Client_Regulation_Already_Exists()
        {
            // arrange
            const string regulationId = "ID";
            const string clientId = "me";

            _regulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IRegulation>(new Core.Domain.Regulation
                {
                    Id = regulationId
                }));

            _clientRegulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult<IClientRegulation>(new ClientRegulation
                {
                    ClientId = clientId,
                    RegulationId = regulationId
                }));

            // act
            Task task = _service.AddAsync(new ClientRegulation
            {
                ClientId = clientId,
                RegulationId = regulationId
            });

            // assert
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(async () => await task);
            Assert.Equal("Client regulation already exists.", exception.Message);
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

            // act
            Task task = _service.SetDefaultAsync(clientId, country);

            // assert
            ServiceException exception = await Assert.ThrowsAsync<ServiceException>(async () => await task);
            Assert.Equal("No default regulations.", exception.Message);
        }

        [Fact]
        public async void SetDefaultAsync_Choose_Regulation_Rule_With_Most_Priority()
        {
            // arrange
            const string clientId = "me";
            const string country = "uk";

            WelcomeRegulationRule rule1 = Create("1", "reg_1", country, true, 10);
            WelcomeRegulationRule rule2 = Create("2", "reg_2", country, true, 100);

            IClientRegulation result = null;

            _clientRegulationRepositoryMock.Setup(o => o.GetByClientIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<ClientRegulation>());

            _clientRegulationRepositoryMock.Setup(o => o.AddAsync(It.IsAny<IClientRegulation>()))
                .Returns(Task.CompletedTask)
                .Callback((IClientRegulation o) => result = o);

            _welcomeRegulationRuleRepositoryMock.Setup(o => o.GetByCountryAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<WelcomeRegulationRule>
                {
                    rule1,
                    rule2
                });

            // act
            await _service.SetDefaultAsync(clientId, country);

            // assert
            Assert.NotNull(result);
            Assert.Equal(result.RegulationId, rule2.RegulationId);
            Assert.Equal(result.Active, rule2.Active);
        }

        [Fact]
        public async void SetDefaultAsync_Choose_Default_Regulation_Rule_With_Most_Priority()
        {
            // arrange
            const string clientId = "me";
            const string country = "uk";

            WelcomeRegulationRule rule1 = Create("1", "reg_1", null, true, 50);
            WelcomeRegulationRule rule2 = Create("2", "reg_2", null, false, 100);
            WelcomeRegulationRule rule3 = Create("3", "reg_3", null, true, 10);

            IClientRegulation result = null;

            _clientRegulationRepositoryMock.Setup(o => o.GetByClientIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<ClientRegulation>());

            _clientRegulationRepositoryMock.Setup(o => o.AddAsync(It.IsAny<IClientRegulation>()))
                .Returns(Task.CompletedTask)
                .Callback((IClientRegulation o) => result = o);

            _welcomeRegulationRuleRepositoryMock.Setup(o => o.GetDefaultAsync())
                .ReturnsAsync(new List<WelcomeRegulationRule>
                {
                    rule1,
                    rule2,
                    rule3
                });

            _welcomeRegulationRuleRepositoryMock.Setup(o => o.GetByCountryAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<WelcomeRegulationRule>());

            // act
            await _service.SetDefaultAsync(clientId, country);

            // assert
            Assert.NotNull(result);
            Assert.Equal(result.RegulationId, rule2.RegulationId);
            Assert.Equal(result.Active, rule2.Active);
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
        public async Task SetAsync_Delete_Existing_Client_Regulations_Befor_Adding()
        {
            // arrange
            const string regulationId = "ID";
            const string clientId = "me";

            var clientRegulation = new ClientRegulation
            {
                RegulationId = "test"
            };

            _regulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IRegulation>(new Core.Domain.Regulation
                {
                    Id = regulationId
                }));

            _clientRegulationRepositoryMock.Setup(o => o.GetByClientIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<ClientRegulation>
                {
                    clientRegulation
                });

            _clientRegulationRepositoryMock.Setup(o => o.AddAsync(It.IsAny<IClientRegulation>()))
                .Returns(Task.CompletedTask);

            // act
            await _service.SetAsync(clientId, regulationId);

            // assert
            _clientRegulationRepositoryMock.Verify(o =>
                o.DeleteAsync(It.Is<string>(clientIdArg => clientIdArg == clientId),
                    It.Is<string>(regulationIdArg => regulationIdArg == clientRegulation.RegulationId)));
        }

        [Fact]
        public async Task SetAsync_Inserted_Correct_Client_Regulation_Active_True_KYC_False()
        {
            // arrange
            const string regulationId = "ID";
            const string clientId = "me";

            IClientRegulation result = null;

            _regulationRepositoryMock.Setup(o => o.GetAsync(It.IsAny<string>()))
                .Returns(Task.FromResult<IRegulation>(new Core.Domain.Regulation
                {
                    Id = regulationId
                }));

            _clientRegulationRepositoryMock.Setup(o => o.GetByClientIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new List<ClientRegulation>());

            _clientRegulationRepositoryMock.Setup(o => o.AddAsync(It.IsAny<IClientRegulation>()))
                .Returns(Task.CompletedTask);

            _clientRegulationRepositoryMock.Setup(o=>o.AddAsync(It.IsAny<IClientRegulation>()))
                .Returns(Task.CompletedTask)
                .Callback((IClientRegulation o) => result = o);

            // act
            await _service.SetAsync(clientId, regulationId);

            // assert
            Assert.True(result.ClientId == clientId && result.RegulationId == regulationId && result.Active && !result.Kyc);
        }

        private WelcomeRegulationRule Create(string id, string regulation, string country, bool active, int priority)
        {
            return new WelcomeRegulationRule
            {
                Id = id,
                Name = $"Rule {id}",
                RegulationId = regulation,
                Countries = country == null
                    ? new List<string>()
                    : new List<string>
                    {
                        "tmp",
                        country,
                        "tmp1"
                    },
                Active = active,
                Priority = priority
            };
        }
    }
}
