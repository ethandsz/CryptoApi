using Bybit.Net.Objects.Models.V5;
using NUnit.Framework;
using StockBotInfrastructure.Maps;

namespace StockBotInfrastructure.Tests.Maps;

[TestFixture]
public class ByBitStickMapperTests
{
    [Test]
    public void Map_ByBitKline_ReturnsCandleSticks()
    {
        // Arrange
        var bybitKline = new BybitKline
        {
            StartTime = new DateTime(2020, 1, 1, 1, 1, 1), //Translates to 637134372610000000 ticks
            OpenPrice = 100,
            HighPrice = 200,
            LowPrice = 50,
            ClosePrice = 150,
            Volume = 1000,
            QuoteVolume = 2000
        };
        var byBitStickMapper = new ByBitStickMapper();
        
        // Act
        var result = byBitStickMapper.Map(bybitKline);
        
        // Assert
        Assert.AreEqual(200, result.High);
        Assert.AreEqual(50, result.Low);
        Assert.AreEqual(150, result.Close);
        Assert.AreEqual(100, result.Open);
        Assert.AreEqual(637134372610000000, result.TimeStamp);
    }
}