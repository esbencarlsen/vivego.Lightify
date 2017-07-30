using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

using vivego.Lightify.Authentication;

namespace vivego.Lightify.HttpHandlers
{
	public class AuthenticationHandler : DelegatingHandler
	{
		private readonly IAuthenticator _authenticator;
		private string _authToken;

		public AuthenticationHandler(IAuthenticator authenticator,
			HttpMessageHandler innerHandler) : base(innerHandler)
		{
			_authenticator = authenticator;
		}

		protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
			CancellationToken cancellationToken)
		{
			if (_authToken == null)
			{
				_authToken = await _authenticator.GetAuthToken();
			}

			request.Headers.Remove(Constants.HttpHeaders.Authorization);
			request.Headers.TryAddWithoutValidation(Constants.HttpHeaders.Authorization, _authToken);

			// Try once
			HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

			// Unauthorized? Then create new session and log in again.
			if (response.StatusCode == HttpStatusCode.Unauthorized)
			{
				// Create new session
				_authToken = await _authenticator.GetAuthToken();

				request.Headers.Remove(Constants.HttpHeaders.Authorization);
				request.Headers.TryAddWithoutValidation(Constants.HttpHeaders.Authorization, _authToken);

				// Try again
				response = await base.SendAsync(request, cancellationToken);
			}

			return response;
		}
	}
}