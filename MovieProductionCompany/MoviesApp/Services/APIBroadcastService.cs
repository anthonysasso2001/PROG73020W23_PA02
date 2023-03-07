using Microsoft.EntityFrameworkCore;
using MoviesApp.Entities;
using MoviesApp.Interfaces;

namespace MoviesApp.Services
{
	public class APIBroadcastService : APIBroadcastInterface
	{
		public APIBroadcastService(MovieDbContext movieDbContext)
		{
			
			thisCompany.ProductionStudio = movieDbContext.ProductionStudios.FirstOrDefault();

			//make all stream companies and subscribe them on construction
			var streamersInfo = movieDbContext.StreamCompanies
				.Select(i => new StreamCompanyInfo()
				{
					Name = i.Name,
					webApi = i.webApi
				}).ToList();
			foreach (var sInfo in streamersInfo)
				streamers.Add(new StreamCompany() { streamCompanyInfo = sInfo });

			streamers.ForEach(s => thisCompany.Subscribe(s));
		}

		public ProductionCompany thisCompany
		{
			get { return thisCompany; }
		}

		public List<StreamCompany> streamers
		{
			get { return streamers; }
		}
	}
}
