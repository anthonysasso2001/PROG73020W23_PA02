using System.ComponentModel.DataAnnotations;

namespace MovieProductionApp.Entities
{
	public class StreamCompanyInfo
	{
		public int? StreamCompanyInfoId { get; set; }

		[Required(ErrorMessage = "Please enter a name.")]
		public string? Name { get; set; }
        [Required(ErrorMessage = "Please enter an API URL.")]
        public string webApiUrl { get; set; }
        [Required(ErrorMessage = "Please enter a Challenge URL.")]
        public string challengeUrl { get; set; }

        public bool verificationStatus { get; set; } = false;
        public Guid? StreamGUID { get; set; }
        public ICollection<MovieApiData>? RegisteredMovies { get; set; }
	}

	//was goin to use following but it didn't work very well?? idk why
	/*
	public class StreamCompanyAuth
	{
		[ForeignKey("StreamCompanyInfo")]
		public Guid StreamCompanyInfoId { get; set; }

		public string challengeUrl { get; set; }

		public virtual StreamCompanyInfo StreamCompany { get; set; }
	}
	//this was in dbcontext to issue key
	modelBuilder.Entity<StreamCompanyInfo>()
				.OwnsOne(i => i.StreamCompanyAuth)
				.WithOwner(a => a.StreamCompany)
				.HasForeignKey(i => i.StreamCompanyAuthId);
	*/
}
