using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyBGListApi.DTO;
using MyBGListApi.Models;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;
using MyBGListApi.Attributes;
namespace MyBGListApi.Controllers;

[Route("Mechanics")]
[ApiController]
public class MechanicsController: ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<MechanicsController> _logger;

    public MechanicsController(ApplicationDbContext dbContext, ILogger<MechanicsController> logger)
    {
        _dbContext = dbContext; 
        _logger = logger;
    }

    [HttpGet]
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
    public async Task<RestDTO<Mechanic[]>> Get([FromQuery] RequestDTO<MechanicDTO> input)
    {
        var query = _dbContext.Mechanics.AsQueryable();
        if (!string.IsNullOrEmpty(input.FilterQuery))
        {
            query = query.Where(b => b.Name.Contains(input.FilterQuery));
        }

        var recordCount = await query.CountAsync();

        query = query
            .OrderBy($"{input.SortColumn} {input.SortOrder}")
            .Skip(input.PageIndex * input.PageSize)
            .Take(input.PageSize);
        return new RestDTO<Mechanic[]>()
        {
            Data = await query.ToArrayAsync(),
            PageIndex = input.PageIndex,
            PageSize = input.PageSize,
            RecordCount = recordCount,
            Links = new List<LinkDTO>
            {
                new LinkDTO(
                    Url.Action(
                        null,
                        "Mechanics",
                        new { input.PageIndex, input.PageSize },
                        Request.Scheme)!,
                    "self",
                    "GET"),
            }
        };
    }

    [HttpPost]
    [ResponseCache(NoStore = true)]
    public async Task<RestDTO<Mechanic?>> Update([FromBody]MechanicDTO model)
    {
        var mechanic = await _dbContext.Mechanics
            .Where(b => b.Id == model.Id)
            .FirstOrDefaultAsync();

        if (mechanic != null)
        {
            if (!string.IsNullOrEmpty(model.Name))
                mechanic.Name = model.Name;
            mechanic.LastModifiedDate = DateTime.Now;
            _dbContext.Mechanics.Update(mechanic);
            await _dbContext.SaveChangesAsync();
        }

        return new RestDTO<Mechanic?>()
            {
                Data = mechanic,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                        Url.Action(
                            null,
                            "Mechanics",
                            model,
                            Request.Scheme)!,
                        "self",
                        "POST"),
                }
            };

    }

    [HttpDelete]
    [ResponseCache(NoStore = true)]
    public async Task<RestDTO<Mechanic?>> Delete([FromQuery] int id)
    {
        var mechanic = await _dbContext.Mechanics
            .Where(b => b.Id == id)
            .FirstOrDefaultAsync();
        if (mechanic != null)
        {
            _dbContext.Mechanics.Remove(mechanic);
            await _dbContext.SaveChangesAsync();

        }
        
        return new RestDTO<Mechanic?>()
        {
            Data = mechanic,
            Links = new List<LinkDTO>
            {
                new LinkDTO(
                    Url.Action(
                        null,
                        "Mechanics",
                        id,
                        Request.Scheme)!,
                    "self",
                    "DELETE"),
            }
        };
    }
    
}