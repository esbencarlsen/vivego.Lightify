using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

namespace vivego.Lightify.HttpHandlers
{
	public class LoggingHandler : DelegatingHandler
	{
		private readonly ILogger _logger;

		public LoggingHandler(ILoggerFactory loggerFactory,
			HttpMessageHandler innerHandler) : base(innerHandler)
		{
			_logger = loggerFactory.CreateLogger<LoggingHandler>();
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
			CancellationToken cancellationToken)
		{
			_logger.LogDebug("Sending : {0}", request);
			try
			{
				HttpResponseMessage httpResponseMessage = await base.SendAsync(request, cancellationToken);
				_logger.LogDebug("Received : {0}", httpResponseMessage);
				return httpResponseMessage;
			}
			catch (Exception exception)
			{
				_logger.LogError(new EventId(), exception, "Exception while sendingrequest {0}", request);
				throw;
			}
		}
	}
}