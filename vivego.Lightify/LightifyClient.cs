using System;
using System.Net.Http;

using Microsoft.Extensions.Logging;

using vivego.Lightify.Authentication;
using vivego.Lightify.HttpHandlers;

namespace vivego.Lightify
{
	public class LightifyClient
	{
		public static HttpClient Create(Uri baseUri,
			string username,
			string password,
			string serialNumber,
			ILoggerFactory loggerFactory)
		{
			return ClientBuilder
				.Create(baseUri)
				.WithHandler(inner => new LoggingHandler(loggerFactory, inner))
				.WithHandler(inner =>
				{
					LightifyAuthenticator authenticator =
						new LightifyAuthenticator(baseUri, username, password, serialNumber, loggerFactory);
					AuthenticationHandler authenticationHandler = new AuthenticationHandler(authenticator, inner);
					return authenticationHandler;
				})
				.Build();
		}
	}
}