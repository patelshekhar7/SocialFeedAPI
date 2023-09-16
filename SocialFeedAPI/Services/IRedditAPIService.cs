using SocialFeedAPI.Models;

namespace SocialFeedAPI.Services
{
    public interface IRedditAPIService
    {
        public Task<List<SubRedditPost>> GetSubRedditPosts(string subRedditName);
    }
}
