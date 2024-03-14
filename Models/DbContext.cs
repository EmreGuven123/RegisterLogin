using Microsoft.EntityFrameworkCore;

namespace registerLogin.Models
{
	public class RegLogContext : DbContext
	{
		public RegLogContext(DbContextOptions<RegLogContext> options) : base(options) { }

			//public DbSet<RegName> RegName { get; set; }
		//public DbSet<RegPassword> regPassword { get; set; }

		public DbSet<RegisterLogin> registerLogin { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.EnableSensitiveDataLogging();
			base.OnConfiguring(optionsBuilder);
		}
	}
}
