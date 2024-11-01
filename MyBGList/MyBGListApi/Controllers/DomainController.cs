using Microsoft.AspNetCore.Mvc;
using MyBGList.DTO;
using MyBGListApi.DTO;
using MyBGListApi.Models;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;

namespace MyBGListApi.Controllers;

[Route("Domains")]
[ApiController]
public class DomainController: ControllerBase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<DomainController> _logger;

    public DomainController(ApplicationDbContext dbContext, ILogger<DomainController> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    [HttpGet]
    [ResponseCache(Location = ResponseCacheLocation.Any, Duration = 60)]
    public async Task<ActionResult<RestDTO<Domain[]>>> Get([FromQuery] RequestDTO<DomainDTO> input)
    {
        var query = _dbContext.Domains.AsQueryable();

        if (!string.IsNullOrEmpty(input.FilterQuery))
        {
            query = query.Where(b => b.Name.Contains(input.FilterQuery));
        }

        var recordCount = await query.CountAsync();
        query = query
            .OrderBy($"{input.SortColumn} {input.SortOrder}")
            .Skip(input.PageIndex * input.PageSize)
            .Take(input.PageSize);
        
                    return new RestDTO<Domain[]>()
            {
                Data = await query.ToArrayAsync(),
                PageIndex = input.PageIndex,
                PageSize = input.PageSize,
                RecordCount = recordCount,
                Links = new List<LinkDTO> {
                    new LinkDTO(
                        Url.Action(
                            null,
                            "Domains",
                            new { input.PageIndex, input.PageSize },
                            Request.Scheme)!,
                        "self",
                        "GET"),
                }
            };
        }

        [HttpPost(Name = "UpdateDomain")]
        [ResponseCache(NoStore = true)]
        public async Task<RestDTO<Domain?>> Post([FromBody]DomainDTO model)
        {
            var domain = await _dbContext.Domains
                .Where(b => b.Id == model.Id)
                .FirstOrDefaultAsync();
            if (domain != null)
            {
                if (!string.IsNullOrEmpty(model.Name))
                    domain.Name = model.Name;
                domain.LastModifiedDate = DateTime.Now;
                _dbContext.Domains.Update(domain);
                await _dbContext.SaveChangesAsync();
            };

            return new RestDTO<Domain?>()
            {
                Data = domain,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                            Url.Action(
                                null,
                                "Domains",
                                model,
                                Request.Scheme)!,
                            "self",
                            "POST"),
                }
            };
        }

        [HttpDelete(Name = "DeleteDomain")]
        [ResponseCache(NoStore = true)]
        public async Task<RestDTO<Domain?>> Delete([FromQuery]int id)
        {
            var domain = await _dbContext.Domains
                .Where(b => b.Id == id)
                .FirstOrDefaultAsync();
            if (domain != null)
            {
                _dbContext.Domains.Remove(domain);
                await _dbContext.SaveChangesAsync();
            };

            return new RestDTO<Domain?>()
            {
                Data = domain,
                Links = new List<LinkDTO>
                {
                    new LinkDTO(
                            Url.Action(
                                null,
                                "Domains",
                                id,
                                Request.Scheme)!,
                            "self",
                            "DELETE"),
                }
            };
        }
        
        
    }