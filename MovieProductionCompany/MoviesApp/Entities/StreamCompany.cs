using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesApp.Entities
{
	public class StreamCompanyInfo
	{
		public string? Name { get; set; }
		public string webApiUrl { get; set; }

		public Guid StreamCompanyAuthId { get; set; }
		public virtual StreamCompanyAuth StreamCompanyAuth { get; set; }

		public ICollection<MovieApiData>? RegisteredMovies { get; set; }
	}

	public class StreamCompanyAuth
	{
		public Guid StreamCompanyAuthId { get; set; }

		public string challengeUrl { get; set; }

		public virtual StreamCompanyInfo StreamCompany { get; set; }
	}
}
