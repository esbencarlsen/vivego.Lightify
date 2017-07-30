using System;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using vivego.Lightify.HttpHandlers;
using vivego.Lightify.Models;

namespace vivego.Lightify.Authentication
{
	public class LightifyAuthenticator : IAuthenticator
	{
		private readonly Uri _baseUri;
		private readonly string _password;
		private readonly string _serialNumber;
		private readonly ILoggerFactory _loggerFactory;
		private readonly string _username;

		public LightifyAuthenticator(Uri baseUri,
			string username,
			string password,
			string serialNumber,
			ILoggerFactory loggerFactory)
		{
			_baseUri = baseUri;
			_username = username;
			_password = password;
			_serialNumber = serialNumber;
			_loggerFactory = loggerFactory;
		}

		public async Task<string> GetAuthToken()
		{
			using (System.Net.Http.HttpClient httpClient = ClientBuilder
				.Create(_baseUri)
				.WithHandler(innerHandler => new LoggingHandler(_loggerFactory, innerHandler))
				.Build())
			{
				SessionResponse response = await httpClient
					.MakeSession(_username, _password, _serialNumber)
					.ConfigureAwait(false);

				return response.SecurityToken;
			}
		}
	}
}