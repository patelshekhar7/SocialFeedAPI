using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using SocialFeedAPI.Common.Exceptions;
using SocialFeedAPI.Models;

namespace SocialFeedAPI.Services
{
    public class RedditAPIService : IRedditAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly int _requestInterval;
        private readonly string _apiToken;
        private readonly ILogger _logger;

        public RedditAPIService(IHttpClientFactory httpClientFactory, IConfiguration configuration, ILogger<RedditAPIService> logger) 
        {
            this._httpClientFactory = httpClientFactory;
            this._requestInterval = Convert.ToInt32(configuration["RedditAPISettings:HTTPRequestIntervalInMilliSeconds"]);
            this._apiToken = configuration["RedditAPISettings:Token"];
            this._logger = logger;
        }

        public async Task<List<SubRedditPost>> GetSubRedditPosts(string subRedditName)
        {
            try
            {
                string afterParam = "";
                List<SubRedditPost> posts = await InvokeSubRedditService(subRedditName, afterParam);

                if (posts.Count == 0) { return posts; }

                while (true)
                {
                    afterParam = posts[posts.Count - 1].Id;  //get Id of the last post in collection. That id will be used as afterParam
                    if (afterParam == null) { break; }

                    await Task.Delay(this._requestInterval); //adding delay for rate limit. 
                    List<SubRedditPost> previousPosts = await InvokeSubRedditService(subRedditName, afterParam);
                    if (previousPosts.Count > 0)
                    {
                        posts.AddRange(previousPosts);
                    }
                    else
                    {
                        break;
                    }
                }
                return posts;
            }
            catch (RateLimitExceededException re)
            {
                _logger.Log(LogLevel.Warning, "Rate Limit exception received from Reddit API. Limit Resetting In " + re.RateLimitResettingIn.ToString());
                //TODO: Retry after RateLimitResettingIn secs. 
                throw;
                
            }
            catch (HttpRequestException e)
            {
                string error = System.Environment.NewLine
                                + "Status Code: " + e.StatusCode
                                + System.Environment.NewLine
                                + "Message: " + e.Message
                                + System.Environment.NewLine
                                + "Exception: " + e.InnerException;

                _logger.Log(LogLevel.Error, "Reddit API returned an non-success response. Check Details below." + error);
                throw;
            }
            catch (Exception e)
            {
                string error = System.Environment.NewLine + e.Message + System.Environment.NewLine + e.StackTrace;
                _logger.Log(LogLevel.Error,"An error occured while retrieving posts from Reddit API. Check Exception below." + error);
                throw;
            }
        }

        private async Task<List<SubRedditPost>> InvokeSubRedditService(string subRedditName, string afterParam)
        {
            List<SubRedditPost> posts = new List<SubRedditPost>();

            HttpClient httpClient = _httpClientFactory.CreateClient("RedditAPIService");
            httpClient.DefaultRequestHeaders.Add(HeaderNames.Authorization, $"Bearer {_apiToken}");

            string endpointAddress = $"/r/{subRedditName}/top?t=all&limit=100&after={afterParam}";

            string httpResponseMessage = await httpClient.GetStringAsync(endpointAddress);

            if (httpResponseMessage != null)
            {
                SubRedditResponse subRedditResponse = JsonConvert.DeserializeObject<SubRedditResponse>(httpResponseMessage);

                if (subRedditResponse.Data != null && subRedditResponse.Data.Children != null)
                {
                    foreach (var childData in subRedditResponse.Data.Children)
                    {
                        posts.Add(new SubRedditPost
                        {
                            Id = string.Format("{0}_{1}", childData.Kind, childData.Data.Id),
                            Title = childData.Data.Title,
                            Author = childData.Data.Author,
                            UpVotes = childData.Data.Ups
                        }
                        );
                    }
                }
            }
            return posts;
        }
    }
}
