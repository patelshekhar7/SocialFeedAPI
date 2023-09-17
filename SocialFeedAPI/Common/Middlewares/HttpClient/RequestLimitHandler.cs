using SocialFeedAPI.Common.Exceptions;
using System.Net.Http.Headers;

namespace SocialFeedAPI.Common.Middlewares.HttpClient
{
    public class RequestLimitHandler : DelegatingHandler
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        public RequestLimitHandler(IConfiguration configuration, ILogger<RequestLimitHandler> logger)
        {
            this._configuration = configuration;
            this._logger = logger;
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

        private void LogMessage (LogLevel logLevel, string message)
        {
            this._logger.Log(logLevel, message);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {  
            if (rateLimitRemaining != null)
            {
                if (this.rateLimitRemaining.Value < 1)
                {          
                    throw new RateLimitExceededException("Rate Limit Reached", rateLimitReset.Value);
                }
            }

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
                if (headers.TryGetValues(this.RequestLimitResetHeaderName, out rateLimitResetValues))
                {
                    this.rateLimitReset = Convert.ToInt32(Convert.ToDouble(rateLimitResetValues.First()));
                }

                string logMessage = string.Format("Rate Limit Remaining {0}", 
                                                   rateLimitRemaining != null ? rateLimitRemaining.Value.ToString() : "");
                this.LogMessage(LogLevel.Information, logMessage);
            }

            return responseMessage;

        }

    }
}
