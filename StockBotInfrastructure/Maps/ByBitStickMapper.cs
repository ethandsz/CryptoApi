using Bybit.Net.Objects.Models.V5;
using Riok.Mapperly.Abstractions;
using StockBotDomain.Models;

namespace StockBotInfrastructure.Maps;

[Mapper]
public partial class ByBitStickMapper
{
    [MapProperty(nameof(BybitKline.HighPrice), nameof(CandleSticks.High))]
    [MapProperty(nameof(BybitKline.LowPrice), nameof(CandleSticks.Low))]
    [MapProperty(nameof(BybitKline.ClosePrice), nameof(CandleSticks.Close))]
    [MapProperty(nameof(BybitKline.OpenPrice), nameof(CandleSticks.Open))]
    [MapProperty(nameof(@BybitKline.StartTime.Ticks), nameof(CandleSticks.TimeStamp))]
    public partial CandleSticks Map(BybitKline bybitKline);
}

