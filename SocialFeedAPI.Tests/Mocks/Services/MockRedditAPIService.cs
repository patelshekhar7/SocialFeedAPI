using SocialFeedAPI.Models;
using SocialFeedAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialFeedAPI.Tests.Mocks.Services
{
    internal class MockRedditAPIService : IRedditAPIService
    {
        public Task<List<SubRedditPost>> GetSubRedditPosts(string subRedditName)
        {
            return Task.FromResult(GetMockedSubRedditPosts());
        }

        private List<SubRedditPost> GetMockedSubRedditPosts()
        {
            SubRedditPost post1 = new SubRedditPost { Author = "Author1", Id = "abc1", Title = "Title1", UpVotes = 100 };
            SubRedditPost post2 = new SubRedditPost { Author = "Author1", Id = "abc2", Title = "Title2", UpVotes = 90 };
            SubRedditPost post3 = new SubRedditPost { Author = "Author2", Id = "abc2", Title = "Title2", UpVotes = 80 };
            SubRedditPost post4 = new SubRedditPost { Author = "Author2", Id = "abc2", Title = "Title2", UpVotes = 70 };
            SubRedditPost post5 = new SubRedditPost { Author = "Author2", Id = "abc2", Title = "Title2", UpVotes = 60 };
            SubRedditPost post6 = new SubRedditPost { Author = "Author3", Id = "abc2", Title = "Title2", UpVotes = 50 };
            SubRedditPost post7 = new SubRedditPost { Author = "Author4", Id = "abc2", Title = "Title2", UpVotes = 40 };
            SubRedditPost post8 = new SubRedditPost { Author = "Author5", Id = "abc2", Title = "Title2", UpVotes = 30 };
            SubRedditPost post9 = new SubRedditPost { Author = "Author6", Id = "abc2", Title = "Title2", UpVotes = 20 };
            SubRedditPost post10 = new SubRedditPost { Author = "Author6", Id = "abc2", Title = "Title2", UpVotes = 10 };
            

            return new List<SubRedditPost> { post1, post2, post3, post4, post5, post6, post7, post8, post9, post10 };
        }
    }
}
