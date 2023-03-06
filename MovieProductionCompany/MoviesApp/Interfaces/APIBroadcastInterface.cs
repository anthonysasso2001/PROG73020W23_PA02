using MoviesApp.Entities;

namespace MoviesApp.Interfaces
{
	public interface APIBroadcastInterface
	{
		ProductionCompany thisCompany { get; }
		List<StreamCompany> streamers { get; }
	}
}
