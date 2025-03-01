using WebApplication14.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;
namespace WebApplication14.Models
{
    public class IssueType
    {
        public int IssueTypeId { get; set; }
        public string? IssueName { get; set; }
        public string? IssueDegree { get; set; }
    }


public static class IssueTypeEndpoints
{
	public static void MapIssueTypeEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/IssueType").WithTags(nameof(IssueType));

        group.MapGet("/", async (WebApplication14Context db) =>
        {
            return await db.IssueType.ToListAsync();
        })
        .WithName("GetAllIssueTypes")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<IssueType>, NotFound>> (int issuetypeid, WebApplication14Context db) =>
        {
            return await db.IssueType.AsNoTracking()
                .FirstOrDefaultAsync(model => model.IssueTypeId == issuetypeid)
                is IssueType model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetIssueTypeById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int issuetypeid, IssueType issueType, WebApplication14Context db) =>
        {
            var affected = await db.IssueType
                .Where(model => model.IssueTypeId == issuetypeid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.IssueTypeId, issueType.IssueTypeId)
                  .SetProperty(m => m.IssueName, issueType.IssueName)
                  .SetProperty(m => m.IssueDegree, issueType.IssueDegree)
                  );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateIssueType")
        .WithOpenApi();

        group.MapPost("/", async (IssueType issueType, WebApplication14Context db) =>
        {
            db.IssueType.Add(issueType);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/IssueType/{issueType.IssueTypeId}",issueType);
        })
        .WithName("CreateIssueType")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int issuetypeid, WebApplication14Context db) =>
        {
            var affected = await db.IssueType
                .Where(model => model.IssueTypeId == issuetypeid)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteIssueType")
        .WithOpenApi();
    }
}}