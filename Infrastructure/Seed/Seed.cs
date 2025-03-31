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
            dbContext.RemoveRange(dbContext.Set<Answer>());
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
                    Console.WriteLine($"Role '{role}' created.");
                }
            }
        }

        private static async Task SeedUsersAsync(UserManager<User> userManager)
        {
            var users = new List<User>
            {
                new User { UserName = "admin@example.com", Email = "admin@example.com", FirstName = "Admin", LastName = "User", Gmina = "Central" },
                new User { UserName = "john.doe@example.com", Email = "john.doe@example.com", FirstName = "John", LastName = "Doe", Gmina = "West" },
                new User { UserName = "jane.smith@example.com", Email = "jane.smith@example.com", FirstName = "Jane", LastName = "Smith", Gmina = "East" }
            };

            foreach (var user in users)
            {
                if (await userManager.FindByEmailAsync(user.Email) == null)
                {
                    var result = await userManager.CreateAsync(user, "Password123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, user.UserName == "admin@example.com" ? "Admin" : "User");
                        Console.WriteLine($"User '{user.UserName}' created.");
                    }
                }
            }
        }

        private static async Task SeedFilmsAsync(AppDbContext dbContext)
        {
            var films = new List<Film>
            {
                new Film
                {
                    Id = Guid.NewGuid(),
                    Name = "Sample Film 1",
                    Content = File.ReadAllBytes(GetMoviesPaths().FirstOrDefault()!),
                    ContentType = "video/mp4",
                    Questions = new List<Question>(),
                    Image = GetPhotos().FirstOrDefault()!
                },
                new Film
                {
                    Id = Guid.NewGuid(),
                    Name = "Sample Film 2",
                    Content = File.ReadAllBytes(GetMoviesPaths().FirstOrDefault()!),
                    ContentType = "video/mp4",
                    Questions = new List<Question>(),
                    Image = GetPhotos().FirstOrDefault()!
                },
                new Film
                {
                    Id = Guid.NewGuid(),
                    Name = "Sample Film 3",
                    Content = File.ReadAllBytes(GetMoviesPaths().FirstOrDefault()!),
                    ContentType = "video/mp4",
                    Questions = new List<Question>(),
                    Image = GetPhotos().FirstOrDefault()!
                },
                new Film
                {
                    Id = Guid.NewGuid(),
                    Name = "Sample Film 4",
                    Content = File.ReadAllBytes(GetMoviesPaths().FirstOrDefault()!),
                    ContentType = "video/mp4",
                    Questions = new List<Question>(),
                    Image = GetPhotos().FirstOrDefault()!
                }
            };

            foreach (var film in films)
            {
                var question = new Question
                {
                    Id = Guid.NewGuid(),
                    Text = "Is this a sample question for " + film.Name + "?",
                    Film = film,
                    Answers = new List<Answer>
                    {
                        new Answer { Id = Guid.NewGuid(), Text = "Yes", IsTrue = true },
                        new Answer { Id = Guid.NewGuid(), Text = "No", IsTrue = false }
                    }
                };

                film.Questions.Add(question);
            }

            await AddEntitiesAsync(dbContext, films);
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
