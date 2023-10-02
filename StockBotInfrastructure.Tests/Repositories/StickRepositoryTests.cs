using NSubstitute;
using NUnit.Framework;
using StockBotDomain.Models;
using StockBotInfrastructure.Repositories;

namespace StockBotInfrastructure.Tests.Repositories;

[TestFixture]
public class StickRepositoryTests
{
    [Test]
    public void AddLevels_Should_Insert_Candlesticks_IntoDatabase()
    {
        // Arrange
        var holdLevels = new List<HoldLevel>
        {
            new() { IsInverse = false, Level = 50 },
            new() { IsInverse = true, Level = 60 },
        };

        var connectionMock = Substitute.For<IStickRepository>();
        
        // Act
        connectionMock.AddLevels(holdLevels);
        
       // Assert.That(connectionMock.Received());
    }
}