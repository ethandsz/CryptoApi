using Bybit.Net.Enums;
using NUnit.Framework;
using static StockBotInfrastructure.Helpers.GetKlineInterval;

namespace StockBotInfrastructure.Tests.Helpers;

[TestFixture]
public class GetKlineIntervalTests
{
    [TestCase("1m", KlineInterval.OneMinute)]
    [TestCase("3m", KlineInterval.ThreeMinutes)]
    [TestCase("5m", KlineInterval.FiveMinutes)]
    [TestCase("15m", KlineInterval.FifteenMinutes)]
    [TestCase("30m", KlineInterval.ThirtyMinutes)]
    [TestCase("1h", KlineInterval.OneHour)]
    [TestCase("2h", KlineInterval.TwoHours)]
    [TestCase("4h", KlineInterval.FourHours)]
    [TestCase("1d", KlineInterval.OneDay)]
    [TestCase("1w", KlineInterval.OneWeek)]
    public void GetKlineInterval_Should_Return_1m(string timeInterval, KlineInterval expected)
    {
        // Arrange Act
        var result = ConvertStrToKlineInterval(timeInterval);
        
        // Assert
        Assert.AreEqual(result, expected);
    }
    
    [Test]
    public void GetKlineInterval_Should_Throw_ValidationException()
    {
        // Assert
        Assert.Throws<ArgumentException>(TestDelegate);
        return;

        // Arrange Act
        void TestDelegate() => ConvertStrToKlineInterval("1M");
    }
}