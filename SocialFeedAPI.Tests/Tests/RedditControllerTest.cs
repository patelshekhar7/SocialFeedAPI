using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SocialFeedAPI.Controllers;
using SocialFeedAPI.Services;
using SocialFeedAPI.Tests.Mocks.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialFeedAPI.Tests.Tests
{
    public class RedditControllerTest
    {
        private IConfiguration _configuration;
        
        private IConfiguration GetConfiguration()
        {
            if(_configuration == null)
            {
                var builder = new ConfigurationBuilder().AddJsonFile($"appsettings.json", optional: false);
                _configuration = builder.Build();
            }
            return _configuration;
        }

        private ILogger<T> GetLogger<T>() where T : class
        {
            using (var logFactory = LoggerFactory.Create(builder => builder.AddConsole()))
            {
                return logFactory.CreateLogger<T>();
            }
        }


        [Fact]
        public void GetSubRedditStats_Success()
        {
            MockRedditAPIService mockAPIService = new MockRedditAPIService();
            IConfiguration configuration = GetConfiguration();
            ILogger<RedditController> logger = GetLogger<RedditController>();

            RedditController redditController = new RedditController(mockAPIService, configuration, logger);
            var postStats = redditController.Get("nfl").Result;

            int topN = Convert.ToInt32(configuration["RedditAPISettings:ReportTopNNumber"]);

            Assert.NotNull(postStats);
            Assert.True(postStats.TopPosts.Count == topN);
            Assert.True(postStats.TopAuthors.Count == topN);
        }
    }
}
