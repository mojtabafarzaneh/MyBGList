using Microsoft.AspNetCore.Mvc;

namespace MyBGListApi.Controllers;

[Route("controller")]
[ApiController]
public class BoardGamesController: ControllerBase
{
    private readonly ILogger<BoardGamesController> _logger;

    public BoardGamesController(ILogger<BoardGamesController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetBoardGames")]
    public IEnumerable<BoardGame> Get()
    {
        return new[]
        {
            new BoardGame
            {
                Id = 1,
                Name = "Axix and Alies",
                Year = 1981,
            }
        };
        
    }
}