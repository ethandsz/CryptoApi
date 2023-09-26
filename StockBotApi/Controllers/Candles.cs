using Microsoft.AspNetCore.Mvc;
using StockBotInfrastructure;

namespace StockApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Candles : ControllerBase
{
    private readonly ISearchSticksService _searchSticksService;

    public Candles(ISearchSticksService searchSticksService)
    {
        _searchSticksService = searchSticksService;
    }

    [HttpGet]
    public async Task<string> GetStick()
    {
        await _searchSticksService.Search();
        return "This is where we return a stick level";
    }
}