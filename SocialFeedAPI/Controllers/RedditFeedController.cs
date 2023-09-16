using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using SocialFeedAPI.Models;
using SocialFeedAPI.Services;
using System.Text;

namespace SocialFeedAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RedditFeedController : Controller
    {
        private readonly IRedditAPIService _redditAPIService;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        public RedditFeedController(IRedditAPIService redditAPIService, IConfiguration configuration, ILogger<RedditFeedController> logger)
        {
            this._redditAPIService = redditAPIService;
            this._configuration = configuration;
            this._logger = logger;
        }

        /// <summary>
        /// Continueously reports top posts and author information for NFL, NBA and NHL subreddits from Reddit.
        /// </summary>
        /// <returns>Json data containing staticstics about NFL, NBA and NHL</returns>
        [HttpGet(Name = "GetRedditRunningFeed")]
        public async Task Get(CancellationToken cancellationToken)
        {
            try
            {
                Response.ContentType = "text/event-stream";

                List<string> subReddits = _configuration["RedditAPISettings:SubredditNames"].Split(",").ToList();

                while (!cancellationToken.IsCancellationRequested)
                {
                    foreach (string subReddit in subReddits)
                    {
                        this._logger.Log(LogLevel.Information, $"Fetching posts for SubReddit {subReddit} started");
                        List<SubRedditPost> redditPosts = await _redditAPIService.GetSubRedditPosts(subReddit);
                        this._logger.Log(LogLevel.Information, $"Fetch posts for SubReddit {subReddit} ended. Total posts: {redditPosts.Count.ToString()}");

                        //TODO: Persist results. For now reporting to WebAPI response. 
                        ReportPostStatistics(subReddit, redditPosts);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError("An error occured while processing request." + System.Environment.NewLine + e.ToString());
                throw new HttpRequestException("An error occured while processing the request", null, System.Net.HttpStatusCode.InternalServerError);
            }
        }

        private async Task ReportPostStatistics(string subReditName, List<SubRedditPost> redditPosts)
        {
         
            //Report messages to resonse in Json format for now.
            //Ideally we would want to return the results in Json
            var jsonPost = new { PostName = subReditName };
            await WriteToResponse(JsonConvert.SerializeObject(jsonPost));
            var stats = GetPostStatistics(redditPosts);
            await WriteToResponse(JsonConvert.SerializeObject(stats));
            
        }

        protected SubRedditStats GetPostStatistics(List<SubRedditPost> redditPosts)
        {
            int topN = Convert.ToInt32(_configuration["RedditAPISettings:ReportTopNNumber"]);
            return SubRedditStats.GetStats(redditPosts, topN);
        }


        private async Task WriteToResponse(string message)
        {
            try
            {
                var bytes = Encoding.ASCII.GetBytes($"data: {message}\n\n");
                await Response.Body.WriteAsync(bytes, 0, bytes.Length);
                await HttpContext.Response.Body.FlushAsync();
                Response.Body.Close();
            }
            catch (Exception ex)
            {
                this._logger.Log(LogLevel.Error, "Error occured trying to write response. Check Details: " + System.Environment.NewLine + ex.ToString());
            }
        }
    }
}
