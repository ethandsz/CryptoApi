using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NUnit.Framework;
using StockBotDomain.Models;
using StockBotInfrastructure.Repositories;
using StockBotInfrastructure.Services;

namespace StockBotInfrastructure.Tests.Services;

[TestFixture]
public class SearchSticksServiceTests
{
    [Test]
    public void Search_Should_Return_Candlesticks()
    {
        // Arrange
        var searchSticksServiceMock = Substitute.For<ISearchSticksService>();
        var stickRepositoryMock = Substitute.For<IStickRepository>();
        
        // Act
        searchSticksServiceMock.Search();
        
        // Assert
        stickRepositoryMock.Received().AddLevels(Arg.Any<List<HoldLevel>>());
    }
    
}