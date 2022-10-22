using ProjectServices = BreweryManagement.Infrastructure.Services;
using ProjectRepositories = BreweryManagement.Domain.Repositories;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace BreweryManagement.UnitTests.Services
{
    public class BrewerService
    {
        [Fact(DisplayName = "GetById_Success")]
        [Trait("Category", "Getters")]
        public void GetById_Success()
        {
            // Config
            var inMemorySettings = new Dictionary<string, string> {
                                        {"TopLevelKey", "TopLevelValue"},
                                        {"SectionName:SomeKey", "SectionValue"},
                                    };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            // Arrange & Setup
            string expectedName = "TestBrewer";
            Mock<ProjectRepositories.BaseRepository> _baseRepositoryMock = new Mock<ProjectRepositories.BaseRepository>(configuration);
            _baseRepositoryMock.Setup(x => x.Brewers).ReturnsDbSet(new List<Domain.Models.Brewer>()
            {
                new Domain.Models.Brewer() { Id = "b111", Name = expectedName }
            });

            ProjectServices.IBrewerService _brewerService = new ProjectServices.BrewerService(_baseRepositoryMock.Object);

            // Act
            string existingBrewerId = "b111";
            Domain.Models.Brewer? brewer = _brewerService.GetById(existingBrewerId);

            // Assert
            Assert.Equal(brewer?.Name, expectedName);
        }
    }
}
