using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using WebApplication14.Data;

namespace WebApplication14.Models
{
    public class User : IdentityUser<int>
    {
        public string Name { get; set; } = string.Empty;
        public List<Report> Reports { get; set; } = new();
        public User() { }
        public User(string name, string email)
        {
            Name = name;
            Email = email; // new EmailUser(email).ToString();
            UserName = email;
        }
        public string Password { get; set; } = null!;
        public Boolean LegalAge { get; set; } = false;
        public int RegionId { get; set; }
        [ForeignKey("RegionId")]
        public virtual Region? Region { get; set; }
    }


public static class UserEndpoints
{
	public static void MapUserEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/User").WithTags(nameof(User));

        group.MapGet("/", async (WebApplication14Context db) =>
        {
            return await db.User.ToListAsync();
        })
        .WithName("GetAllUsers")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<User>, NotFound>> (int id, WebApplication14Context db) =>
        {
            return await db.User.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Id == id)
                is User model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetUserById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int id, User user, WebApplication14Context db) =>
        {
            var affected = await db.User
                .Where(model => model.Id == id)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Name, user.Name)
                  .SetProperty(m => m.Password, user.Password)
                  .SetProperty(m => m.LegalAge, user.LegalAge)
                  .SetProperty(m => m.RegionId, user.RegionId)
                  .SetProperty(m => m.Id, user.Id)
                  .SetProperty(m => m.UserName, user.UserName)
                  .SetProperty(m => m.NormalizedUserName, user.NormalizedUserName)
                  .SetProperty(m => m.Email, user.Email)
                  .SetProperty(m => m.NormalizedEmail, user.NormalizedEmail)
                  .SetProperty(m => m.EmailConfirmed, user.EmailConfirmed)
                  .SetProperty(m => m.PasswordHash, user.PasswordHash)
                  .SetProperty(m => m.SecurityStamp, user.SecurityStamp)
                  .SetProperty(m => m.ConcurrencyStamp, user.ConcurrencyStamp)
                  .SetProperty(m => m.PhoneNumber, user.PhoneNumber)
                  .SetProperty(m => m.PhoneNumberConfirmed, user.PhoneNumberConfirmed)
                  .SetProperty(m => m.TwoFactorEnabled, user.TwoFactorEnabled)
                  .SetProperty(m => m.LockoutEnd, user.LockoutEnd)
                  .SetProperty(m => m.LockoutEnabled, user.LockoutEnabled)
                  .SetProperty(m => m.AccessFailedCount, user.AccessFailedCount)
                  );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateUser")
        .WithOpenApi();

        group.MapPost("/", async (User user, WebApplication14Context db) =>
        {
            db.User.Add(user);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/User/{user.Id}",user);
        })
        .WithName("CreateUser")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int id, WebApplication14Context db) =>
        {
            var affected = await db.User
                .Where(model => model.Id == id)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteUser")
        .WithOpenApi();
    }
}}
