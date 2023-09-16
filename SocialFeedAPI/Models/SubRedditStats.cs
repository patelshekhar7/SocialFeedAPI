namespace SocialFeedAPI.Models
{
    public class SubRedditPostStat
    {
        public string Title { get; set; }
        public int UpVotes { get; set; }
    }

    public class SubRedditAuthorStat
    {
        public string Author { get; set; }
        public int TotalPosts { get; set; }
    }


    [Serializable]
    public class SubRedditStats
    {
     
        public List<SubRedditPostStat> TopPosts { get; set; }
        public List<SubRedditAuthorStat> TopAuthors { get; set; }

        public static SubRedditStats GetStats(List<SubRedditPost> posts, int topN)
        {
            var topPosts = posts.OrderByDescending(p => p.UpVotes)
                            .Take(topN)
                            .Select(p => new SubRedditPostStat { Title = p.Title, UpVotes = p.UpVotes })
                            .ToList();

            var topAuthors = posts.GroupBy(p => p.Author)
                             .Select(item => new SubRedditAuthorStat { Author = item.Key, TotalPosts = item.Count() })
                             .OrderByDescending(i => i.TotalPosts)
                             .Take(topN)
                             .ToList();
            
            return new SubRedditStats {  TopPosts = topPosts, TopAuthors = topAuthors};
        }
    }
}
