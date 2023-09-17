using Microsoft.AspNetCore.Mvc;
using SocialFeedAPI.Models;
using SocialFeedAPI.Services;

namespace SocialFeedAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RedditController
    {
        private readonly IRedditAPIService _redditAPIService;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public RedditController(IRedditAPIService redditAPIService, IConfiguration configuration, ILogger<RedditController> logger)
        {
            this._redditAPIService = redditAPIService;
            this._configuration = configuration;
            this._logger = logger;
        }

        /// <summary>
        /// Retrieves top posts and author information by SubredditName from Reddit.
        /// </summary>
        /// <param name="subredditname"></param>
        /// <returns>Json containing staticstics about the given subreddit.</returns>
        [HttpGet("{subredditname}", Name = "GetRedditPosts")]
        public async Task<SubRedditStats> Get(string subredditname)
        {
            List<SubRedditPost> redditPosts = await _redditAPIService.GetSubRedditPosts(subredditname);
            return GetPostStatistics(redditPosts);
        }

        protected SubRedditStats GetPostStatistics(List<SubRedditPost> redditPosts)
        {
            int topN = Convert.ToInt32(_configuration["RedditAPISettings:ReportTopNNumber"]);
            return SubRedditStats.GetStats(redditPosts, topN);
        }

    }
}
