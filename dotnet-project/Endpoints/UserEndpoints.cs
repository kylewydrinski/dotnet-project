
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using dotnet_project.Data;
using dotnet_project.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace dotnet_project.Endpoints;

public static class UserEndpoints
{
    public static void Map(WebApplication app)
    {
        app.MapGet("/api/users/me", (ClaimsPrincipal principal) =>
        {
            var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var username = principal.FindFirstValue(ClaimTypes.Name);

            if (!int.TryParse(userId, out var parsedUserId) ||
                username is null)
            {
                return Results.Unauthorized();
            }

            return Results.Ok(new
            {
                UserId = parsedUserId,
                Username = username
            });
        }).RequireAuthorization();

        app.MapPost("/api/users/signup", async (
            SignupRequest request,
            DotNetContext db,
            IPasswordHasher<User> passwordHasher) =>
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return Results.BadRequest(new
                {
                    message = "Username, email, and password are required."
                });
            }

            var username = request.Username.Trim();
            var email = request.Email.Trim().ToLowerInvariant();

            if (username.Length > 50 ||
                email.Length > 256 ||
                !new EmailAddressAttribute().IsValid(email))
            {
                return Results.BadRequest(new
                {
                    message = "Please provide a valid username and email."
                });
            }

            if (request.Password.Length < 12 ||
                request.Password.Length > 128)
            {
                return Results.BadRequest(new
                {
                    message = "Password must be between 12 and 128 characters."
                });
            }

            // Check for existing accounts
            var exists = await db.Users.AnyAsync(u =>
                u.Username == username || u.Email == email);

            if (exists)
            {
                return Results.Conflict(new
                {
                    message = "That username or email is already registered."
                });
            }

            // Create user and hash the password
            var user = new User
            {
                Username = username,
                Email = email,
                Role = "User",
            };

            user.PasswordHash =
                passwordHasher.HashPassword(user, request.Password);

            // Save to SQL Server
            db.Users.Add(user);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Also handles a concurrent signup violating a unique index.
                return Results.Conflict(new
                {
                    message = "That username or email is already registered."
                });
            }

            // Never return the password or its hash.
            return Results.Created(
                $"/api/users/{user.UserId}",
                new
                {
                    user.UserId,
                    user.Username,
                    user.Email
                });
        });

        _ = app.MapPost("/api/users/login", async (
            LoginRequest request,
            DotNetContext db,
            IPasswordHasher<User> passwordHasher,
            HttpContext httpContext) =>
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return Results.BadRequest(new
                {
                    message = "Username and password are required."
                });
            }

            var username = request.Username.Trim();

            if (username.Length > 50)
            {
                return Results.BadRequest(new
                {
                    message = "Please provide a valid username."
                });
            }

            if (request.Password.Length < 12 ||
                request.Password.Length > 128)
            {
                return Results.BadRequest(new
                {
                    message = "Password is invalid."
                });
            }
            try
            {
                var user = await db.Users.SingleOrDefaultAsync(u =>
                    u.Username == username);

                if (user == null)
                {
                    return Results.Unauthorized();
                }

                var verificationResult = passwordHasher.VerifyHashedPassword(
                    user, user.PasswordHash, request.Password);

                if (verificationResult == PasswordVerificationResult.Failed)
                {
                    return Results.Unauthorized();
                }

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                };
                var identity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme);
                await httpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(identity));

                return Results.Ok(new
                {
                    user.UserId,
                    user.Username,
                    user.Email
                });
            }
            catch (Exception)
            {
                return Results.StatusCode(500);
            }
        });

        app.MapPost("/api/users/logout", async (HttpContext httpContext) =>
        {
            await httpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);
            return Results.Ok(new
            {
                message = "Logged out successfully."
            });
        }).RequireAuthorization();
    }
}

public sealed class SignupRequest
{
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
}

public sealed class LoginRequest
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
}
