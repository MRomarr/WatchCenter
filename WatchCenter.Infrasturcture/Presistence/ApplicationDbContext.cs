
namespace WatchCenter.Infrasturcture.Presistence
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<Genre> Generes { get; set; } = null!;
        public DbSet<Content> Contents { get; set; } = null!;
        public DbSet<Movie> Movies { get; set; } = null!;
        public DbSet<Series> Series { get; set; } = null!;
        public DbSet<Season> Seasons { get; set; } = null!;
        public DbSet<Episode> Episodes { get; set; } = null!;
    }
    
}
