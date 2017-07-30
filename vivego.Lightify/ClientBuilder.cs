using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace vivego.Lightify
{
	public class ClientBuilder
	{
		public List<Func<HttpMessageHandler, HttpMessageHandler>> Pipeline =
			new List<Func<HttpMessageHandler, HttpMessageHandler>>();

		public Uri BaseUri { get; set; }

		private ClientBuilder()
		{
		}

		public static ClientBuilder Create(Uri baseUri)
		{
			return new ClientBuilder
			{
				BaseUri = baseUri
			};
		}

		public ClientBuilder WithHandler(Func<HttpMessageHandler, HttpMessageHandler> delegatingHandlerFactory)
		{
			Pipeline.Add(delegatingHandlerFactory);
			return this;
		}

		public HttpClient Build()
		{
			HttpClientHandler clientHandler = new HttpClientHandler
			{
				AllowAutoRedirect = false
			};

			HttpMessageHandler current = Pipeline.Aggregate<Func<HttpMessageHandler, HttpMessageHandler>, HttpMessageHandler>(
				clientHandler,
				(current1, func) => func(current1));

			HttpClient httpClient = new HttpClient(current)
			{
				BaseAddress = BaseUri
			};
			return httpClient;
		}
	}
}