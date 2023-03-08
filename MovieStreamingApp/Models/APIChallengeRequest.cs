namespace MovieProductionApp.Models
{
    public class APIChallengeRequest
    {
        public string challengeString { get; set; }

        public Guid responseGuid { get; set; } = Guid.Empty;
    }
}
