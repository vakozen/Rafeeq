using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using WebApplication14.Data;

namespace WebApplication14.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        public static DateTime StringToDate(string theDateInStringFormat)
        {
            DateTime result;
            if (DateTime.TryParse(theDateInStringFormat, out result))
            {
                return result;
            }

            return new DateTime();
        }
        public int ReportLatt { get; set; }
        public int ReportLong { get; set; }
        public string? StreetName { get; set; }
        public string? Comment { get; set; }
        public string? Link { get; set; }
        public int RegionId { get; set; }
        [ForeignKey("RegionId")]
        public Region? Regtion { get; set; }
        public int IssueTypeId { get; set; }
        [ForeignKey("IssueTypeId")]
        public IssueType? IssueType { get; set; }
    }


public static class ReportEndpoints
{
	public static void MapReportEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Report").WithTags(nameof(Report));

        group.MapGet("/", async (WebApplication14Context db) =>
        {
            return await db.Report.ToListAsync();
        })
        .WithName("GetAllReports")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Report>, NotFound>> (int reportid, WebApplication14Context db) =>
        {
            return await db.Report.AsNoTracking()
                .FirstOrDefaultAsync(model => model.ReportId == reportid)
                is Report model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetReportById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int reportid, Report report, WebApplication14Context db) =>
        {
            var affected = await db.Report
                .Where(model => model.ReportId == reportid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.ReportId, report.ReportId)
                  .SetProperty(m => m.ReportLatt, report.ReportLatt)
                  .SetProperty(m => m.ReportLong, report.ReportLong)
                  .SetProperty(m => m.StreetName, report.StreetName)
                  .SetProperty(m => m.Comment, report.Comment)
                  .SetProperty(m => m.Link, report.Link)
                  .SetProperty(m => m.RegionId, report.RegionId)
                  .SetProperty(m => m.IssueTypeId, report.IssueTypeId)
                  );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateReport")
        .WithOpenApi();

        group.MapPost("/", async (Report report, WebApplication14Context db) =>
        {
            db.Report.Add(report);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Report/{report.ReportId}",report);
        })
        .WithName("CreateReport")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int reportid, WebApplication14Context db) =>
        {
            var affected = await db.Report
                .Where(model => model.ReportId == reportid)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteReport")
        .WithOpenApi();
    }
}}
