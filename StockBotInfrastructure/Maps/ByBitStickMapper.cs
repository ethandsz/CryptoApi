using Bybit.Net.Objects.Models.V5;
using Riok.Mapperly.Abstractions;
using StockBotDomain.Models;

namespace StockBotInfrastructure.Maps;

[Mapper]
public partial class ByBitStickMapper
{
    [MapProperty(nameof(BybitKline.HighPrice), nameof(CandleStick.High))]
    [MapProperty(nameof(BybitKline.LowPrice), nameof(CandleStick.Low))]
    [MapProperty(nameof(BybitKline.ClosePrice), nameof(CandleStick.Close))]
    [MapProperty(nameof(BybitKline.OpenPrice), nameof(CandleStick.Open))]
    [MapProperty(nameof(@BybitKline.StartTime.Ticks), nameof(CandleStick.TimeStamp))]
    public partial CandleStick Map(BybitKline bybitKline);
}

