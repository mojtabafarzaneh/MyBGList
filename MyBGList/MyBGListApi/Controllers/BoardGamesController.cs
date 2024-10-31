using Microsoft.AspNetCore.Mvc;
using MyBGListApi.DTO;
using MyBGListApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using HostingEnvironmentExtensions = Microsoft.AspNetCore.Hosting.HostingEnvironmentExtensions;


namespace MyBGListApi.Controllers;

[Route("boardGames")]
[ApiController]
public class BoardGamesController : ControllerBase
{
    private readonly ILogger<BoardGamesController> _logger;
    private readonly ApplicationDbContext _dbContext;

    public BoardGamesController(ILogger<BoardGamesController> logger, ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    [HttpGet(Name = "GetBoardGames")]
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
    public async Task<RestDTO<BoardGame[]>> Get(int pageIndex = 0,
        int pageSize = 10,
        string? sortColumn = "Name",
        string? sortOrder = "ASC",
        string? filterQuery = null)
    {
        var query = _dbContext.BoardGames.AsQueryable();

        if (!string.IsNullOrEmpty(filterQuery))
        {
            query = query.Where(b => b.Name.Contains(filterQuery));
        }

        var recordCount = await query.CountAsync();


        query = query
            .OrderBy($"{sortColumn} {sortOrder}")
            .Skip(pageIndex * pageSize)
            .Take(pageSize);
        return new RestDTO<BoardGame[]>()
        {
            Data = await query.ToArrayAsync(),
            PageIndex = pageIndex,
            PageSize = pageSize,
            RecordCount = recordCount,
            Links = new List<LinkDTO>
            {
                new LinkDTO(
                    Url.Action(null, "BoardGames", new { pageIndex, pageSize }, null, Request.Scheme)!,
                    "Self",
                    "Get"),
            }
        };
    }

    [HttpPost("UpdateBoardGame")]
    [ResponseCache(NoStore = true)]
    public async Task<RestDTO<BoardGame?>> Post(BoardGameDto model)
    {
        var boardGame = await _dbContext.BoardGames
            .Where(b => b.Id == model.Id)
            .FirstOrDefaultAsync();
        if (boardGame != null)
        {
            if (!string.IsNullOrEmpty(model.Name))
                boardGame.Name = model.Name;
            if (model.Year.HasValue && model.Year.Value > 0)
                boardGame.Year = model.Year.Value;
            boardGame.LastModifiedDate = DateTime.Now;
            _dbContext.BoardGames.Update(boardGame);
            await _dbContext.SaveChangesAsync();
        }

        return new RestDTO<BoardGame?>()
        {
            Data = boardGame,
            Links = new List<LinkDTO>()
            {
                new LinkDTO(
                    Url.Action(null, "BoardGames", model, Request.Scheme)!,
                    "self",
                    "POST")

            }

        };

    }

    [HttpDelete(Name = "DeleteBoardGame")]
    [ResponseCache(NoStore = true)]
    public async Task<RestDTO<BoardGame?>> Delete(int id)
    {
        var boardGame = await _dbContext.BoardGames
            .Where(b => b.Id == id)
            .FirstOrDefaultAsync();
        if (boardGame != null)
        {
            _dbContext.BoardGames.Remove(boardGame);
            await _dbContext.SaveChangesAsync();
        }

        return new RestDTO<BoardGame?>()
        {
            Data = boardGame,
            Links = new List<LinkDTO>
            {
                new LinkDTO(
                    Url.Action(null, "BoardGames", id, Request.Scheme)!,
                    "Self",
                    "DELETE")

            }
        };
    }
}