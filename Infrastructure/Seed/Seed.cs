using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

namespace Infrastructure.Seed
{
    public static class Seed
    {
        public static async Task Init(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            await ClearDatabaseAsync(dbContext);
            await SeedRolesAsync(roleManager);
            await SeedUsersAsync(userManager);
            await SeedFilmsAsync(dbContext);
        }

        private static async Task ClearDatabaseAsync(AppDbContext dbContext)
        {
            dbContext.RemoveRange(dbContext.Set<User>());
            dbContext.RemoveRange(dbContext.Set<Film>());
            dbContext.RemoveRange(dbContext.Set<Image>());
            dbContext.RemoveRange(dbContext.Set<Question>());

            await dbContext.SaveChangesAsync();
            Console.WriteLine("All data has been removed from the database.");
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager)
        {
            var roles = new[] { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid> { Name = role });
                    //Console.WriteLine($"Role '{role}' created.");
                }
            }
        }

        private static async Task SeedUsersAsync(UserManager<User> userManager)
        {
            var users = new List<User>
            {
                new User { UserName = "admin@example.com", Email = "admin@example.com", FirstName = "Admin", LastName = "User" },
                new User { UserName = "user1@example.com", Email = "user1@example.com", FirstName = "John", LastName = "Doe" },
                new User { UserName = "user2@example.com", Email = "user2@example.com", FirstName = "Jane", LastName = "Smith" }
            };

            foreach (var user in users)
            {
                if (await userManager.FindByEmailAsync(user.Email!) == null)
                {
                    var result = await userManager.CreateAsync(user, "Haslo123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, user.UserName == "admin@example.com" ? "Admin" : "User");
                        //Console.WriteLine($"User '{user.UserName}' created.");
                    }
                }
            }
        }

        private static async Task SeedFilmsAsync(AppDbContext dbContext)
        {
            var film = new Film
            {
                Id = Guid.NewGuid(),
                Name = "Sample Film 1",
                Content = File.ReadAllBytes(GetMoviesPaths().FirstOrDefault()!),
                ContentType = "video/mp4",
                Image = GetPhotos().FirstOrDefault()!
            };

            var questions = new List<Question>
            {
                new Question
                {
                    Id = Guid.NewGuid(),
                    Film = film,
                    FilmId = film.Id,
                    Content = "Is this a sample question for " + film.Name + "?",
                    A = "Yes",
                    B = "No",
                    C = "Maybe",
                    D = "Not sure",
                    CorrectAnswer = "Yes"
                },
                new Question
                {
                    Id = Guid.NewGuid(),
                    Film = film,
                    FilmId = film.Id,
                    Content = "Is this a sample question for " + film.Name + "?",
                    A = "Yes",
                    B = "No",
                    C = "Maybe",
                    D = "Not sure",
                    CorrectAnswer = "No"
                }
            };

            film.Questions = questions;

            await dbContext.AddAsync(film);
            await dbContext.SaveChangesAsync();
        }

        private static async Task AddEntitiesAsync<T>(AppDbContext dbContext, List<T> entities) where T : class
        {
            await dbContext.Set<T>().AddRangeAsync(entities);
            await dbContext.SaveChangesAsync();
            Console.WriteLine($"{entities.Count} entities added to the database.");
        }

        private static List<Image> GetPhotos()
        {
            var images = new List<Image>();
            var paths = GetImagesPaths();

            foreach (var filePath in paths)
            {
                var image = new Image
                {
                    Id = Guid.NewGuid(),
                    Caption = Path.GetFileNameWithoutExtension(filePath).NormalizeString(),
                    Content = File.ReadAllBytes(filePath),
                    ContentType = GetContentType(Path.GetExtension(filePath))
                };

                images.Add(image);
                Console.WriteLine($"Loaded image: {image.Caption}");
            }

            return images;
        }

        private static string GetContentType(string extension)
        {
            return extension.ToLower() switch
            {
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".bmp" => "image/bmp",
                ".gif" => "image/gif",
                _ => "application/octet-stream"
            };
        }

        private static IEnumerable<string> GetImagesPaths()
        {
            string folderPath = "../Infrastructure/Seed/InitialPhotos";

            if (!Directory.Exists(folderPath))
            {
                throw new ArgumentException("Folder not found at path: " + folderPath);
            }

            HashSet<string> allowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                ".jpg", ".jpeg", ".png", ".bmp", ".gif"
            };

            return Directory.GetFiles(folderPath, "*.*").Where(file => allowedExtensions.Contains(Path.GetExtension(file)));
        }

        private static IEnumerable<string> GetMoviesPaths()
        {
            string folderPath = "../Infrastructure/Seed/InitialMovies";

            if (!Directory.Exists(folderPath))
            {
                throw new ArgumentException("Folder not found at path: " + folderPath);
            }

            HashSet<string> allowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                ".mp4"
            };

            return Directory.GetFiles(folderPath, "*.*").Where(file => allowedExtensions.Contains(Path.GetExtension(file)));
        }

        public static string NormalizeString(this string str, char from = ' ', char to = '-')
        {
            return Regex.Replace(str, "[^a-zA-Z0-9 ]", "").ToLower().Replace(from, to);
        }

    }
}
