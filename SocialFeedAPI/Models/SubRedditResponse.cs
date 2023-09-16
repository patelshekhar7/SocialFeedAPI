using Newtonsoft.Json;

namespace SocialFeedAPI.Models
{
    /*
     * Represents the class structure matching to Reddit's API Response 
     */
    [Serializable]
    public class SubRedditResponse
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("data")]
        public SubRedditResponseData Data { get; set; }
    }

    [Serializable]
    public class SubRedditResponseData
    {
        [JsonProperty("children")]
        public List<SubredditChild> Children { get; set; }
    }

    [Serializable]
    public class SubredditChild
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("data")]
        public SubredditData Data { get; set; }
    }

    [Serializable]
    public class SubredditData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("ups")]
        public int Ups { get; set; }

        [JsonProperty("upvote_ratio")]
        public double UpvoteRatio { get; set; }
    }
}
