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

    public async Task Search(string timeInterval)
    {
        Console.WriteLine("Requesting sticks");
        var klineInterval = ConvertStrToKlineInterval(timeInterval);
        var milliseconds = TimeIntervalToMilliseconds(timeInterval);
        var currentTime = DateTime.UtcNow;
        var result = await BybitRestClient.V5Api.ExchangeData.GetKlinesAsync(Category.Spot, "BTCUSDT",
            klineInterval, endTime: new DateTime(currentTime.Ticks - milliseconds * 10), limit: 250);
        var sticks = result.Data.List.Select(kline => ByBitStickMapper.Map(kline));
        var holdLevels = IdentifyHighLevelHoldLevels(sticks.ToList());
        var levels = await ConvertHighLevelHoldLevels(holdLevels, milliseconds);
        StickRepository.AddLevels(levels);
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
   
    private async Task<List<HoldLevel>> ConvertHighLevelHoldLevels(List<HoldLevel> highLevels, int milliSeconds)
    {
        //var milliSeconds = 300000; //Using 4hr interval
        var holdLevels = new List<HoldLevel>();
        var currentTime = DateTime.UtcNow;
        var numOfLevels = highLevels.Count;
        foreach (var holdLevel in highLevels)
        {
            var startTime = holdLevel.TimeStamp;
            var sticks = new List<CandleStick>();
            while (startTime < currentTime.Ticks)
            {
                // var requestedSticks =
                //     await _httpRequester.RequestSticksWithTimeLimit(timeInterval, startTime.ToString(),
                //         currentTime.ToString());
                
                var result = await BybitRestClient.V5Api.ExchangeData.GetKlinesAsync(Category.Spot, "BTCUSDT",
                    KlineInterval.FiveMinutes, startTime: new DateTime(ticks: startTime), endTime: currentTime, limit: 1000);
                if (result.Data.List.FirstOrDefault() is null)
                {
                    break;
                }
                startTime += result.Data.List.FirstOrDefault()!.StartTime.Ticks + milliSeconds;
                sticks.AddRange(result.Data.List.Select(k => ByBitStickMapper.Map(k)));
            }
            
            var sticksToTestForInverseLevel = sticks.Where(s => s.High > holdLevel.Level && s.Low < holdLevel.Level && s.TimeStamp > holdLevel.TimeStamp).ToList();
                    
            if (!await IsTested(sticksToTestForInverseLevel, holdLevel, milliSeconds))
            {
                holdLevels.Add(holdLevel);
            }

            numOfLevels--;
            Console.WriteLine($"Progress: {100 * (double)(highLevels.Count - numOfLevels) / highLevels.Count}");
        }
        return holdLevels;
    }
    private async Task<bool> IsTested(List<CandleStick> candleSticks, HoldLevel holdLevel, long milliseconds)
    {   
        //long milliseconds = 300000;
        if (candleSticks.Count == 0)
        {
            return true;
        }
        foreach (var candleStick in candleSticks)
        {
            // var response = await _httpRequester.RequestSticksWithTimeLimit("1m", candleStick.TimeStamp.ToString(),
            //     (candleStick.TimeStamp + timeInterval.ConvertToMillis()).ToString());
            //
            var result = await BybitRestClient.V5Api.ExchangeData.GetKlinesAsync(Category.Spot, "BTCUSDT",
                KlineInterval.OneMinute, startTime: new DateTime(ticks: candleStick.TimeStamp), endTime: new DateTime(candleStick.TimeStamp + milliseconds * 10000), limit: 1000);
            var sticks = result.Data.List.Select(k => ByBitStickMapper.Map(k));
            var possibleSticks = sticks.Where(w => w.IsBullish == holdLevel.IsInverse);

            if (holdLevel.IsInverse)
            {
                if(possibleSticks.Any(s => s.High > holdLevel.Level * .9999))
                {
                    return true;
                }
                // var firstNonBullishClosedBody = possibleSticks.Where(p => p.Open < holdLevel.Level).FirstOrDefault();
                // if (firstNonBullishClosedBody is not null)
                // {
                //     // possibleSticks = sticks.Where(s => s.TimeStamp > firstNonBullishClosedBody.TimeStamp);
                //     // possibleSticks = possibleSticks.Where(w => w.IsBullish == holdLevel.IsInverse);
                //     if (possibleSticks.Any(p => p.High > holdLevel.Level * .9999))
                //     {
                //         return true;
                //     }
                // }
            }
            if (!holdLevel.IsInverse)
            {
                if(possibleSticks.Any(s => s.Low < holdLevel.Level * 1.0001))
                {
                    return true;
                }
            }
            // var firstBullishClosedBody = possibleSticks.Where(p => p.Open > holdLevel.Level).FirstOrDefault();
            // if (firstBullishClosedBody is not null)
            // {
            //     possibleSticks = sticks.Where(s => s.TimeStamp > firstBullishClosedBody.TimeStamp);
            //     possibleSticks = possibleSticks.Where(w => w.IsBullish == holdLevel.IsInverse);
            //     if (possibleSticks.Any(p => p.Low < holdLevel.Level * 1.0001))
            //     {
            //         return true;
            //     }
            // }
        }
        return false;
    }
}