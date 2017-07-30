using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using vivego.Lightify.Models;

namespace vivego.Lightify.Playground
{
	internal class Program
	{
		private static void Main(string[] args) => MainAsync(args).Wait();

		private static async Task MainAsync(string[] args)
		{
			HttpClient client = MakeClient(args);
			foreach (Device device in await client.GetDevices())
			{
				Console.Out.WriteLine($"Blinking device: {device.Name}");

				bool success = await client.Blink(device);
				if (!success)
				{
					Console.Out.WriteLine($"Could not successfully blink: {device.Name}");
				}
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="args"></param>
		/// <param name="region">eu or us</param>
		/// <returns></returns>
		private static HttpClient MakeClient(string[] args,
			string region = "eu")
		{
			IConfigurationRoot configuration = new ConfigurationBuilder()
				.AddInMemoryCollection(new[]
				{
					new KeyValuePair<string, string>("userName", string.Empty), // Add your username here
					new KeyValuePair<string, string>("password", string.Empty), // Add your password here
					new KeyValuePair<string, string>("serialNumber", string.Empty) // Add your serialNumber here
				})
				.AddUserSecrets("project-8084c8e7-0000-0000-b266-b33f42dd88c0")
				.AddCommandLine(args)
				.Build();

			string userName = GetConfigurationValue(configuration, "userName");
			string password = GetConfigurationValue(configuration, "password");
			string serialNumber = GetConfigurationValue(configuration, "serialNumber");

			Uri uriBase = new Uri(string.Format(Constants.OsramGatewayEndpoints.s_OsramLightifyBaseUriTemplate, region));
			HttpClient client = LightifyClient
				.Create(uriBase, userName, password, serialNumber,
					new LoggerFactory()
						.AddConsole(LogLevel.Information));
			return client;
		}

		private static string GetConfigurationValue(IConfiguration configuration, string key)
		{
			string configurationValue = configuration[key];
			if (string.IsNullOrEmpty(configurationValue))
			{
				Console.Out.WriteLine("Enter {0}", key);
				return Console.ReadLine();
			}

			return configurationValue;
		}
	}
}