using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Regulation.Core.Domain;
using Lykke.Service.Regulation.Core.Repositories;
using Lykke.Service.Regulation.Services;
using Moq;
using Xunit;

namespace Lykke.Service.Regulation.Tests
{
    public class RegulationServiceTests
    {
        private readonly Mock<IRegulationRepository> _regulationRepositoryMock;
        private readonly Mock<IClientAvailableRegulationRepository> _clientAvailableRegulationRepositoryMock;

        private readonly RegulationService _service;

        public RegulationServiceTests()
        {
            _regulationRepositoryMock = new Mock<IRegulationRepository>();
            _clientAvailableRegulationRepositoryMock = new Mock<IClientAvailableRegulationRepository>();

            _service = new RegulationService(_regulationRepositoryMock.Object,
                _clientAvailableRegulationRepositoryMock.Object);
        }

        [Fact]
        public async void GetAsync_Returs_Regulations_By_Id()
        {
            // arrange
            const string regulationId = "ID";

            _regulationRepositoryMock.Setup(o => o.GetAsync(It.Is<string>(i => i == regulationId)))
                .ReturnsAsync(new Core.Domain.Regulation
                {
                    Id = regulationId,
                    RequiresKYC = true
                });

            // act
            IRegulation regulation = await _service.GetAsync(regulationId);

            // assert
            Assert.NotNull(regulation);
        }

        [Fact]
        public async void GetAsync_Not_Returs_Regulations_By_Id()
        {
            // arrange
            const string regulationId = "ID";

            _regulationRepositoryMock.Setup(o => o.GetAsync(It.Is<string>(i => i == "nothing")))
                .ReturnsAsync(new Core.Domain.Regulation
                {
                    Id = regulationId,
                    RequiresKYC = true
                });

            // act
            IRegulation regulation = await _service.GetAsync(regulationId);

            // assert
            Assert.Null(regulation);
        }

        [Fact]
        public async void RemoveAsync_Can_Not_Remove_If_Regulation_Assigned_With_Client()
        {
            // arrange
            const string regulationId = "ID";

            _clientAvailableRegulationRepositoryMock
                .Setup(o => o.GetByRegulationIdAsync(It.Is<string>(i => i == regulationId)))
                .ReturnsAsync(new List<ClientAvailableRegulation>
                {
                    new ClientAvailableRegulation
                    {
                        ClientId = regulationId,
                        RegulationId = regulationId,
                    }
                });

            // act
            Task task = _service.RemoveAsync(regulationId);

            // assert
            await Assert.ThrowsAsync<Exception>(async () => await task);
        }

        [Fact]
        public async void RemoveAsync_Remove_If_Regulation_Not_Assigned_With_Client()
        {
            // arrange
            const string regulationId = "ID";

            _clientAvailableRegulationRepositoryMock
                .Setup(o => o.GetByRegulationIdAsync(It.Is<string>(i => i == regulationId)))
                .ReturnsAsync(new List<ClientAvailableRegulation>());

            // act
            Task task = _service.RemoveAsync(regulationId);

            // assert
            await task;
        }
    }
}
