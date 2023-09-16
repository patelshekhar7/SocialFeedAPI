namespace SocialFeedAPI.Models
{
    [Serializable]
    public class SubRedditPost
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int UpVotes { get; set; }
    }

}
