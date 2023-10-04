using Bybit.Net.Enums;

namespace StockBotInfrastructure.Helpers;

public static class GetKlineInterval
{
    public static KlineInterval ConvertStrToKlineInterval(string timeInterval)
    {
        return timeInterval switch
        {
            "1m" => KlineInterval.OneMinute,
            "3m" => KlineInterval.ThreeMinutes,
            "5m" => KlineInterval.FiveMinutes,
            "15m" => KlineInterval.FifteenMinutes,
            "30m" => KlineInterval.ThirtyMinutes,
            "1h" => KlineInterval.OneHour,
            "2h" => KlineInterval.TwoHours,
            "4h" => KlineInterval.FourHours,
            "1d" => KlineInterval.OneDay,
            "1w" => KlineInterval.OneWeek,
            _ => throw new ArgumentException("Invalid time interval")
        };
    }
}