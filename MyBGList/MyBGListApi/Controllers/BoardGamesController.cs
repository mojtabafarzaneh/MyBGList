using Microsoft.AspNetCore.Mvc;
using MyBGListApi.DTO;
using MyBGListApi.Models;

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
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
    public RestDTO<BoardGame[]> Get()
    {
        return new RestDTO<BoardGame[]>()
        {
            Data = new BoardGame[]
            {
                new BoardGame()
                {
                Id = 1,
                Name = "Axix and Alies",
                Year = 1981,
                },
                new BoardGame()
                {
                    Id = 2,
                    Name = "Citadel",
                    Year= 2000,
                }
            },
            Links = new List<LinkDTO>
            {
                new LinkDTO(
                    Url.Action(null, "BoardGames", null, Request.Scheme)!,
                    "Self",
                    "Get"),
            }
        };
        
    }
}