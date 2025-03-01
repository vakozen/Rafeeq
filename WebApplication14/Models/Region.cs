using WebApplication14.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace WebApplication14.Models
{
    public class Region
    {
        public int RegionId { get; set; }
        public required string RegionName { get; set; }
        public string? Government { get; set; }

        public int Latt { get; set; }
        public int Long { get; set;}
    }


public static class RegionEndpoints
{
	public static void MapRegionEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Region").WithTags(nameof(Region));

        group.MapGet("/", async (WebApplication14Context db) =>
        {
            return await db.Region.ToListAsync();
        })
        .WithName("GetAllRegions")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Region>, NotFound>> (int regionid, WebApplication14Context db) =>
        {
            return await db.Region.AsNoTracking()
                .FirstOrDefaultAsync(model => model.RegionId == regionid)
                is Region model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetRegionById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int regionid, Region region, WebApplication14Context db) =>
        {
            var affected = await db.Region
                .Where(model => model.RegionId == regionid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.RegionId, region.RegionId)
                  .SetProperty(m => m.RegionName, region.RegionName)
                  .SetProperty(m => m.Government, region.Government)
                  .SetProperty(m => m.Latt, region.Latt)
                  .SetProperty(m => m.Long, region.Long)
                  );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateRegion")
        .WithOpenApi();

        group.MapPost("/", async (Region region, WebApplication14Context db) =>
        {
            db.Region.Add(region);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Region/{region.RegionId}",region);
        })
        .WithName("CreateRegion")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int regionid, WebApplication14Context db) =>
        {
            var affected = await db.Region
                .Where(model => model.RegionId == regionid)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteRegion")
        .WithOpenApi();
    }
}}
