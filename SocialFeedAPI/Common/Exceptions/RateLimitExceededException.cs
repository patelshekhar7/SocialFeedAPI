namespace SocialFeedAPI.Common.Exceptions
{
    public class RateLimitExceededException : Exception
    {
        public int RateLimitResettingIn { get; set; }
        public RateLimitExceededException(string message, int ratelimitResettingIn) : base(message) 
        { 
            this.RateLimitResettingIn = ratelimitResettingIn; ;
        }
    }
}
