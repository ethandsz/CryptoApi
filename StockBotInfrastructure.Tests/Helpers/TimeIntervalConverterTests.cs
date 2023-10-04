using NUnit.Framework;
using static StockBotInfrastructure.Helpers.TimeIntervalConverter;

namespace StockBotInfrastructure.Tests.Helpers;

[TestFixture]
public class TimeIntervalConverterTests
{
    [TestCase("1m", 60000)]
    [TestCase("3m", 180000)]
    [TestCase("5m", 300000)]
    [TestCase("15m", 900000)]
    [TestCase("30m", 1800000)]
    [TestCase("1h", 3600000)]
    [TestCase("2h", 7200000)]
    [TestCase("4h", 14400000)]
    [TestCase("1d", 86400000)]
    [TestCase("1w", 604800000)]
    public void TimeIntervalToMilliseconds_Should_Return_60000(string timeInterval, int expected)
    {
        // Arrange Act
        var result = TimeIntervalToMilliseconds(timeInterval);
        
        // Assert
        Assert.AreEqual(result, expected);
    }
    
    [Test]
    public void TimeIntervalToMilliseconds_Should_Throw_ValidationException()
    {
        // Assert
        Assert.Throws<ArgumentException>(TestDelegate);
        return;

        // Arrange Act
        void TestDelegate() => TimeIntervalToMilliseconds("1M");
    }
}