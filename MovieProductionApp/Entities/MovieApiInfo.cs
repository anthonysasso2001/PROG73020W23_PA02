using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace MovieProductionApp.Entities
{
	public class MovieApiInfo
	{
		//this represents the time the offer was first created
		public DateTime TimeOfOffer { get; set; }

		public string? StreamPartner { get; set; } = null!;

		public string Name { get; set; }

		public int? Year { get; set; }

		public int? Rating { get; set; }

		public string? GenreId { get; set; }

		public ProductionStudio? ProductionStudio { get; set; }
	}
}
