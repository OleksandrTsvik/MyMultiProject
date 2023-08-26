using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations;

namespace Persistence;

public class DataContext : IdentityDbContext<AppUser>
{
    public DbSet<Duty> Duties { get; set; }
    public DbSet<Photo> Photos { get; set; }
    public DbSet<UserFollowing> UserFollowings { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<DictionaryCategory> DictionaryCategories { get; set; }
    public DbSet<DictionaryItem> DictionaryItems { get; set; }
    public DbSet<DictionaryCategoryItem> DictionaryCategoryItems { get; set; }
    public DbSet<GrammarRule> GrammarRules { get; set; }
    public DbSet<GrammarRuleItem> GrammarRuleItems { get; set; }

    public DataContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new RefreshTokenConfiguration());
        builder.ApplyConfiguration(new DutyConfiguration());
        builder.ApplyConfiguration(new PhotoConfiguration());
        builder.ApplyConfiguration(new UserFollowingConfiguration());
        builder.ApplyConfiguration(new ImageConfiguration());
        builder.ApplyConfiguration(new GrammarRuleItemConfiguration());
        builder.ApplyConfiguration(new DictionaryCategoryItemConfiguration());
    }
}
