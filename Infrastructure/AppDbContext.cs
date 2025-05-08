using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    public virtual DbSet<Question> Questions { get; set; }
    public virtual DbSet<UserAnswerOnQuestion> Answers { get; set; }
    public virtual DbSet<Image> Images { get; set; }
    public virtual DbSet<Film> Films { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Film>()
            .HasOne(f => f.Image)
            .WithOne(i => i.Film)
            .HasForeignKey<Image>(i => i.FilmId);

        modelBuilder.Entity<Image>()
            .HasOne(i => i.Film)
            .WithOne(f => f.Image)
            .HasForeignKey<Film>(f => f.ImageId);

        modelBuilder.Entity<UserAnswerOnQuestion>()
            .HasKey(ua => ua.Id);

        modelBuilder.Entity<UserAnswerOnQuestion>()
            .HasOne(ua => ua.User)
            .WithMany(u => u.Answers)
            .HasForeignKey(ua => ua.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserAnswerOnQuestion>()
            .HasOne(ua => ua.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(ua => ua.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
        base.OnModelCreating(modelBuilder);
    }
}
