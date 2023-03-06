using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MoviesApp.Entities
{
	public class MovieApiData
	{
		//this represents the time the offer was first created
		public DateTime TimeOfOffer { get; set; }

		[Required(ErrorMessage = "Movie Required For Reference")]
		public int? MovieId { get; set; }
		[ValidateNever]
		public Movie Movie { get; set; } = null!;

		[Required(ErrorMessage = "Please specify a production studio.")]
		public int? ProductionStudioId { get; set; }
		[ValidateNever]
		public ProductionStudio ProductionStudio { get; set; } = null!;
	}
}
