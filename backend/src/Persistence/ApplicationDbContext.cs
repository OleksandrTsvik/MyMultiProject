using System.Reflection;
using Domain.Birthdays;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public sealed class ApplicationDbContext : DbContext
{
    public DbSet<Birthday> Birthdays { get; set; }

    public DbSet<EmailVerificationToken> EmailVerificationTokens { get; set; }
    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserPermission> UserPermissions { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly(),
            ApplicationDbConfigurationsFilter);

        base.OnModelCreating(builder);
    }

    private static bool ApplicationDbConfigurationsFilter(Type type) =>
        type.FullName?.Contains("Configurations") ?? false;
}
