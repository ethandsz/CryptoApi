using Microsoft.AspNetCore.Mvc;
using StockBotInfrastructure.Services;

namespace StockApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScanController : ControllerBase
{
    private readonly ISearchSticksService _searchSticksService;

    public ScanController(ISearchSticksService searchSticksService)
    {
        _searchSticksService = searchSticksService;
    }

    [HttpGet("start")]
    public async Task<string> StartService(string timeInterval)
    {
        await _searchSticksService.Search(timeInterval);
        return "Finished";
    }
}