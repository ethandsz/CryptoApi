using System.Numerics;
using Bybit.Net.Clients;
using Bybit.Net.Enums;
using StockBotDomain.Models;
using StockBotInfrastructure.Maps;
using StockBotInfrastructure.Repositories;
using static StockBotInfrastructure.Helpers.GetKlineInterval;
using static StockBotInfrastructure.Helpers.TimeIntervalConverter;

namespace StockBotInfrastructure.Services;

public class SearchSticksService : ISearchSticksService
{
    public SearchSticksService(BybitRestClient bybitRestClient)
    {
        BybitRestClient = bybitRestClient;
        ByBitStickMapper = new ByBitStickMapper();
        StickRepository = new StickRepository();
    }

    private BybitRestClient BybitRestClient { get; }
    private ByBitStickMapper ByBitStickMapper { get; }
    private IStickRepository StickRepository { get; }
    
    private List<HoldLevel> _levels = new();

    public async Task Search(string timeInterval)
    {
        _levels.Clear();
        Console.WriteLine("Requesting sticks");
        var klineInterval = ConvertStrToKlineInterval(timeInterval);
        var milliseconds = TimeIntervalToMilliseconds(timeInterval);
        var currentTime = DateTime.UtcNow;
        var result = await BybitRestClient.V5Api.ExchangeData.GetKlinesAsync(Category.Spot, "BTCUSDT",
            klineInterval, endTime: new DateTime(currentTime.Ticks - milliseconds * 10), limit: 20);
        var sticks = result.Data.List.Select(kline => ByBitStickMapper.Map(kline));
        var holdLevels = IdentifyHighLevelHoldLevels(sticks.ToList());
        await FindAndAddLevels(holdLevels, milliseconds, klineInterval);
        StickRepository.AddLevels(_levels);
    }
    
    
    //todo: Move this logic to domain layer
    private List<HoldLevel> IdentifyHighLevelHoldLevels(List<CandleStick> sticks)
    {
        List<CandleStick> possibleHoldLevelSticks = new List<CandleStick>();
        bool isInsideRange = false;

        // Iterate through the list of wicks from end to beginning.
        for (int i = sticks.Count - 2; i >= 0; i--)
        {
            CandleStick currentCandleStick = sticks[i];
            CandleStick previousCandleStick = sticks[i + 1];

            if (currentCandleStick.IsBullish == previousCandleStick.IsBullish)
            {
                // The current wick is part of the same pattern as the previous wick.
                isInsideRange = true;
            }
            else if (isInsideRange)
            {
                // The current wick is the end of a pattern.
                possibleHoldLevelSticks.Add(previousCandleStick);
                isInsideRange = false;
            }
        }
        
        var holdLevels = possibleHoldLevelSticks.Select(s =>
        {
            if (s.IsBullish)
            {
                return new HoldLevel { IsInverse = true, Level = s.Low, TimeStamp = s.TimeStamp};
            }

            return new HoldLevel { IsInverse = false, Level = s.High, TimeStamp = s.TimeStamp};
        }).ToList();

        return holdLevels;
    }
   
    private async Task FindAndAddLevels(List<HoldLevel> highLevels, int milliSeconds, KlineInterval klineInterval)
    {
        var currentTime = DateTime.UtcNow;
        BigInteger milli = milliSeconds;

        foreach (var holdLevel in highLevels)
        {
            var startTime = holdLevel.TimeStamp;
            var sticks = new List<CandleStick>();
            while (startTime < currentTime.Ticks)
            {
                var result = await BybitRestClient.V5Api.ExchangeData.GetKlinesAsync(Category.Spot, "BTCUSDT",
                    klineInterval, startTime: new DateTime(ticks: startTime), limit: 1000);
                if (result.Data.List.FirstOrDefault() is null)
                {
                    break;
                }

                startTime = (long)(result.Data.List.FirstOrDefault()!.StartTime.Ticks + milli * 10000);
                sticks.AddRange(result.Data.List.Select(k => ByBitStickMapper.Map(k)));
            }
            
            var sticksToTestForInverseLevel = sticks.Where(s => s.High > holdLevel.Level * .9999 && s.Low < holdLevel.Level * 1.0001 && s.TimeStamp > holdLevel.TimeStamp).ToList();
                    
            if (!await IsTested(sticksToTestForInverseLevel, holdLevel, milliSeconds))
            {
                _levels.Add(holdLevel);
            }
        }
    }
    private async Task<bool> IsTested(List<CandleStick> candleSticks, HoldLevel holdLevel, long milliseconds)
    {   
        if (candleSticks.Count == 0)
        {
            return true;
        }
        foreach (var candleStick in candleSticks)
        {
            var result = await BybitRestClient.V5Api.ExchangeData.GetKlinesAsync(Category.Spot, "BTCUSDT",
                KlineInterval.OneMinute, startTime: new DateTime(ticks: candleStick.TimeStamp), endTime: new DateTime(candleStick.TimeStamp + milliseconds * 10000), limit: 1000);
            var sticks = result.Data.List.Select(k => ByBitStickMapper.Map(k));
            var possibleSticks = sticks.Where(s => s.IsBullish == holdLevel.IsInverse);

            if (holdLevel.IsInverse)
            {
                if(possibleSticks.Any(s => s.High > holdLevel.Level * .9999))
                {
                    if(milliseconds == 60000)
                    {
                        return true;
                    }
                    var testedSticks = possibleSticks.Where(s => s.High > holdLevel.Level * .9999);
                    var sticksToTestInRange = await BybitRestClient.V5Api.ExchangeData.GetKlinesAsync(Category.Spot, "BTCUSDT",
                        KlineInterval.OneMinute, new DateTime(ticks: holdLevel.TimeStamp), new DateTime(ticks: testedSticks.Max(s => s.TimeStamp)));
                    var sticksInRange = sticksToTestInRange.Data.List.Select(k => ByBitStickMapper.Map(k)).ToList();
                    var holdLevels = IdentifyHighLevelHoldLevels(sticksInRange).Where(h => h.IsInverse).ToList();
                    await FindAndAddLevels(holdLevels, 60000, KlineInterval.OneMinute);
                    return true;
                }
            }
            if (!holdLevel.IsInverse)
            {
                if(possibleSticks.Any(s => s.Low < holdLevel.Level * 1.0001))
                {
                    if(milliseconds == 60000)
                    {
                        return true;
                    }
                    var testedSticks = possibleSticks.Where(s => s.Low < holdLevel.Level * 1.0001);
                    var sticksToTestInRange = await BybitRestClient.V5Api.ExchangeData.GetKlinesAsync(Category.Spot, "BTCUSDT",
                        KlineInterval.OneMinute, new DateTime(ticks: holdLevel.TimeStamp), new DateTime(ticks: testedSticks.Max(s => s.TimeStamp)));
                    var sticksInRange = sticksToTestInRange.Data.List.Select(k => ByBitStickMapper.Map(k)).ToList();
                    var holdLevels = IdentifyHighLevelHoldLevels(sticksInRange).Where(h => !h.IsInverse).ToList();
                    await FindAndAddLevels(holdLevels, 60000, KlineInterval.OneMinute);
                    return true;
                }
            }
        }
        return false;
    }
}