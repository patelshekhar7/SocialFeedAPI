using SocialFeedAPI.Common.Exceptions;
using System.Net.Http.Headers;

namespace SocialFeedAPI.Common.Middlewares.HttpClient
{
    public class RequestLimitHandler : DelegatingHandler
    {
        private readonly IConfiguration _configuration;
        public RequestLimitHandler(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        protected string RequestLimitRemainingHeaderName
        {
            get
            {
                return "x-ratelimit-remaining";
            }
        }

        protected string RequestLimitResetHeaderName
        {
            get
            {
                return "x-ratelimit-reset";
            }
        }

        private int? rateLimitRemaining = null;
        private int? rateLimitReset = null;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Console.WriteLine("Rate Limit Remaining {0}", rateLimitRemaining != null ? rateLimitRemaining.Value.ToString() : "");
           
            if (rateLimitRemaining != null)
            {
                if (this.rateLimitRemaining.Value < 1)
                {
                    throw new RateLimitExceededException("Rate Limit Reached", rateLimitReset.Value);
                }
            }

            //add token to request headers


            var responseMessage = await base.SendAsync(request, cancellationToken);

            if (responseMessage.IsSuccessStatusCode)
            {
                HttpHeaders headers = responseMessage.Headers;

                IEnumerable<string> rateLimitRemainingValues;

                if (headers.TryGetValues(this.RequestLimitRemainingHeaderName, out rateLimitRemainingValues))
                {
                    this.rateLimitRemaining = Convert.ToInt32(Convert.ToDouble(rateLimitRemainingValues.First()));
                }

                IEnumerable<string> rateLimitResetValues;
                if (headers.TryGetValues("x-ratelimit-reset", out rateLimitResetValues))
                {
                    this.rateLimitReset = Convert.ToInt32(Convert.ToDouble(rateLimitResetValues.First()));
                }

                Console.WriteLine("Rate Limit Remaining {0}", rateLimitRemaining != null ? rateLimitRemaining.Value.ToString() : "");
                
            }

            return responseMessage;

        }

    }
}
