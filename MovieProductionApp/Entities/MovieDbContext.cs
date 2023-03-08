using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace MovieProductionApp.Entities
{
	// Define our DB context class that inherits from EF Core's base DbContext:
	public class MovieDbContext : DbContext
	{
		// Define a constructor that accepts a context options object that
		// is simply passed to the base class:
		public MovieDbContext(DbContextOptions<MovieDbContext> options)
			: base(options)
		{
		}

		// Define a property for accessing/querying all movies from the DB:
		public DbSet<Movie> Movies { get; set; }

		public DbSet<Genre> Genres { get; set; }

		public DbSet<Actor> Actors { get; set; }

		public DbSet<Casting> Castings { get; set; }

		public DbSet<ProductionStudio> ProductionStudios { get; set; }

		public DbSet<StreamCompanyInfo> StreamCompanies { get; set; }

		public DbSet<MovieApiData> MovieApiData { get; set; }

		// override the protected OnModelCreating method to seed the DB w some movies
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// setup the composite key in Castings:
			modelBuilder.Entity<Casting>()
				.HasKey(c => new { c.ActorId, c.MovieId });

			// setup 1-to-many between movie & casting:
			modelBuilder.Entity<Casting>()
				.HasOne(c => c.Movie)
				.WithMany(m => m.Castings)
				.HasForeignKey(c => c.MovieId);

			// setup 1-to-many between actor & casting:
			modelBuilder.Entity<Casting>()
				.HasOne(c => c.Actor)
				.WithMany(m => m.Castings)
				.HasForeignKey(c => c.ActorId);

			//make api info also stored based on production company & movie FK
			modelBuilder.Entity<MovieApiData>()
				.HasKey(m => new { m.MovieId, m.ProductionStudioId });

			// make moviedata registereable 1-many with streaming company
			modelBuilder.Entity<MovieApiData>()
				.HasOne(m => m.StreamPartner)
				.WithMany(s => s.RegisteredMovies)
				.HasForeignKey(m => m.StreamCompanyInfoId);

			//make production company names unique, and stream guids
			modelBuilder.Entity<ProductionStudio>()
				.HasIndex(p => p.Name)
				.IsUnique();

			modelBuilder.Entity<StreamCompanyInfo>()
				.HasIndex(s => s.StreamGUID)
				.IsUnique();

			//now seed data

			modelBuilder.Entity<Genre>().HasData(
				new Genre() { GenreId = "A", Name = "Action" },
				new Genre() { GenreId = "C", Name = "Comedy" },
				new Genre() { GenreId = "D", Name = "Drama" },
				new Genre() { GenreId = "H", Name = "Horror" },
				new Genre() { GenreId = "M", Name = "Musical" },
				new Genre() { GenreId = "R", Name = "RomCom" },
				new Genre() { GenreId = "S", Name = "SciFi" }
			);

			// seed some actors:
			modelBuilder.Entity<Actor>().HasData(
				new Actor() { ActorId = 1, FirstName = "Humphrey", LastName = "Bogart" },
				new Actor() { ActorId = 2, FirstName = "Ingrid", LastName = "Bergman" },
				new Actor() { ActorId = 3, FirstName = "Keanu", LastName = "Reeves" },
				new Actor() { ActorId = 4, FirstName = "Carrie-Anne", LastName = "Moss" },
				new Actor() { ActorId = 5, FirstName = "John", LastName = "Travolta" },
				new Actor() { ActorId = 6, FirstName = "Uma", LastName = "Thurman" }
			);

			modelBuilder.Entity<Movie>().HasData(
				new Movie()
				{
					MovieId = 1,
					Name = "Casablanca",
					Year = 1942,
					GenreId = "D"
				},
				new Movie()
				{
					MovieId = 2,
					Name = "The Matrix",
					Year = 1998,
					GenreId = "A"
				},
				new Movie()
				{
					MovieId = 3,
					Name = "Pulp Fiction",
					Year = 1992,
					GenreId = "C"
				}
			);

			// seed some reviews:
			modelBuilder.Entity<Review>().HasData(
				new Review()
				{
					ReviewId = 1,
					Rating = 5,
					Comments = "A classic!",
					MovieId = 1
				},
				new Review()
				{
					ReviewId = 2,
					Rating = 3,
					Comments = "They should have gotten together in the end!",
					MovieId = 1
				},
				new Review()
				{
					ReviewId = 3,
					Rating = 3,
					Comments = "Too slow of a pace",
					MovieId = 1
				},
				new Review()
				{
					ReviewId = 4,
					Rating = 4,
					Comments = "Based on Descarte's \"brain in a vat\" thought experiment",
					MovieId = 2
				},
				new Review()
				{
					ReviewId = 5,
					Rating = 3,
					Comments = "Very philosophical",
					MovieId = 2
				},
				new Review()
				{
					ReviewId = 6,
					Rating = 5,
					Comments = "Very violent but also very funny and clever",
					MovieId = 3
				}
			);

			// seed some castings:
			modelBuilder.Entity<Casting>().HasData(
				new Casting() { MovieId = 1, ActorId = 1, Role = "Rick Blaine" },
				new Casting() { MovieId = 1, ActorId = 2, Role = "Ilsa Lund" },
				new Casting() { MovieId = 2, ActorId = 3, Role = "Neo" },
				new Casting() { MovieId = 2, ActorId = 4, Role = "Trinity" },
				new Casting() { MovieId = 3, ActorId = 5, Role = "Vincet Vega" },
				new Casting() { MovieId = 3, ActorId = 6, Role = "Mia Wallace" }
			);

			modelBuilder.Entity<ProductionStudio>().HasData(new ProductionStudio() { ProductionStudioId = 1, Name = "MPC Ltd." });

			modelBuilder.Entity<MovieApiData>().HasData(
				new MovieApiData() { TimeOfOffer = new DateTime(2010, 03, 4), MovieId = 1, ProductionStudioId = 1 },
				new MovieApiData() { TimeOfOffer = new DateTime(2012, 07, 16), MovieId = 2, ProductionStudioId = 1 },
				new MovieApiData() { TimeOfOffer = new DateTime(2020, 09, 30), MovieId = 3, ProductionStudioId = 1 }
			);
		}
	}
}
