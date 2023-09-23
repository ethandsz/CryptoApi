using Microsoft.AspNetCore.Mvc;

namespace StockApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Candles : ControllerBase
{
    [HttpGet]
    public string GetStick()
    {
        return "This is where we return a stick level";
    }
}