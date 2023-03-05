using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace MoviesApp.Entities
{
	public class NotifierContext : DbContext
	{
		public NotifierContext(DbContextOptions<NotifierContext> options)
			: base(options) { }

		public DbSet<MovieStreamer> MovieStreamers { get; set; }

		public DbSet<MovieStudio> Studio { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<MovieStudio>().HasData(
				new MovieStudio()
				{
					MovieStudioId = { ProductionStudioId = 1, Name = "MPC Ltd." }
				}
			);

			//not creating any streamers, as it is loaded into memory and just for testing
			//but this will store permanently if you change sq-lite to persistent...
		}
	}
}
