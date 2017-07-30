using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using vivego.Lightify.HttpHandlers;
using vivego.Lightify.Models;

namespace vivego.Lightify
{
	public static class HttpClientExtensions
	{
		/// <summary>
		///     Turns the specified device on
		/// </summary>
		public static Task<bool> DeviceTurnOn(this HttpClient httpClient, long deviceId) => httpClient.DeviceSet(deviceId, true);

		public static Task<bool> DeviceTurnOff(this HttpClient httpClient, long deviceId) => httpClient.DeviceSet(deviceId, false);

		public static Task<bool> DeviceColor(this HttpClient httpClient, long deviceId, Color color, int transitionTime = 0) => DeviceSet(httpClient, deviceId, color: color, transitionTime: transitionTime);

		public static async Task<bool> Blink(this HttpClient httpClient, 
			Device device,
			int times = 3,
			int transitionTime = 0,
			TimeSpan? blinkDelay = null)
		{
			if (device.Online <= 0)
			{
				return false;
			}

			bool isOn = device.On > 0;
			foreach (int _ in Enumerable.Range(0, times))
			{
				if (isOn)
				{
					bool success = await httpClient.DeviceTurnOff(device);
					if (!success)
					{
						return false;
					}
					
					await Task.Delay(blinkDelay.GetValueOrDefault(TimeSpan.FromSeconds(0.3)));
					await httpClient.DeviceTurnOn(device);
					await Task.Delay(blinkDelay.GetValueOrDefault(TimeSpan.FromSeconds(0.3)));
				}
				else
				{
					bool success = await httpClient.DeviceTurnOn(device);
					if (!success)
					{
						return false;
					}
					
					await Task.Delay(blinkDelay.GetValueOrDefault(TimeSpan.FromSeconds(0.3)));
					await httpClient.DeviceTurnOff(device);
					await Task.Delay(blinkDelay.GetValueOrDefault(TimeSpan.FromSeconds(0.3)));
				}
			}

			return true;
		}

		/// <summary>
		/// color	query	New Hex Color e.g. FF0000	 	 
		/// ctemp query   New color temperature e.g. 1000 to 8000	 	int
		/// hue query New hue e.g. 1.000 to 360.000	 	float
		/// idx query idx Index of the device according to the device table e.g. 1	 	int
		/// level   query level New dimming level e.g.valid range 0.000 to 1.000	 	float
		/// onoff   query onoff New On/Off state e.g. 0,1	 	int
		/// saturation  query New saturation e.g.valid range 0.000 to 1.000	 	float
		/// time    query time Transition time in 1/10th of a second e.g. 100	0	int
		/// </summary>
		/// <param name="httpClient"></param>
		/// <param name="deviceId"></param>
		/// <param name="onOff"></param>
		/// <param name="color"></param>
		/// <param name="colorTemperature"></param>
		/// <param name="transitionTime"></param>
		/// <returns></returns>
		public static async Task<bool> DeviceSet(this HttpClient httpClient, 
			long deviceId,
			bool? onOff = null,
			Color? color = null,
			long? colorTemperature = null,
			int transitionTime = 0)
		{
			List<Task<bool>> operations = new List<Task<bool>>();
			if (color.HasValue)
			{
				operations.Add(httpClient.DeviceSetOperation(deviceId,
					(Constants.QueryParameter.Color, $"{color.Value.R:X2}{color.Value.G:X2}{color.Value.B:X2}"),
					(Constants.QueryParameter.Time, transitionTime.ToString())));
			}

			if (onOff.HasValue)
			{
				operations.Add(httpClient.DeviceSetOperation(deviceId, 
					(Constants.QueryParameter.OnOff, onOff.Value ? "1" : "0")));
			}

			if (colorTemperature.HasValue)
			{
				operations.Add(httpClient.DeviceSetOperation(deviceId, 
					(Constants.QueryParameter.Ctemp, colorTemperature.Value.ToString())));
			}

			bool[] result = await Task
				.WhenAll(operations)
				.ConfigureAwait(false);
			return result.All(_ => _);
		}

		public static async Task<bool> DeviceSetOperation(this HttpClient httpClient,
			long deviceId,
			params (string ParamName, string ParamValue)[] parameters)
		{
			string url = Constants.ApiOperation.DeviceSet
				.WithQuery(Constants.QueryParameter.Idx, deviceId.ToString());
			foreach ((string ParamName, string ParamValue) valueTuple in parameters)
			{
				url = url.WithQuery(valueTuple.ParamName, valueTuple.ParamValue);
			}
				
			OperationResponse result = await httpClient
				.GetJsonAsync<OperationResponse>(url)
				.ConfigureAwait(false);
			return result != null && result.ReturnCode == 0;
		}

		public static Task<Device[]> GetDevices(this HttpClient httpClient)
		{
			return httpClient
				.GetJsonAsync<Device[]>(Constants.ApiOperation.Devices);
		}

		public static Task<SessionResponse> MakeSession(this HttpClient httpClient,
			string username,
			string password,
			string serialNumber)
		{
			SessionRequest sessionRequest = new SessionRequest
			{
				Username = username,
				Password = password,
				SerialNumber = serialNumber
			};

			return httpClient
				.PostJsonAsync<SessionRequest, SessionResponse>(Constants.ApiOperation.Session, sessionRequest);
		}

		public static async Task<TResponse> GetJsonAsync<TResponse>(this HttpClient httpClient, string requestUri)
		{
			HttpResponseMessage responseMessage = await httpClient
				.GetAsync(requestUri)
				.ConfigureAwait(false);
			if (responseMessage.IsSuccessStatusCode)
			{
				string serializedContent = await responseMessage
					.Content
					.ReadAsStringAsync()
					.ConfigureAwait(false);
				return JsonConvert.DeserializeObject<TResponse>(serializedContent);
			}

			return default(TResponse);
		}

		public static async Task<TResponse> PostJsonAsync<TRequest, TResponse>(this HttpClient httpClient, string requestUri,
			TRequest requestData)
		{
			string serialized = JsonConvert.SerializeObject(requestData);
			HttpResponseMessage responseMessage = await httpClient
				.PostAsync(requestUri, new StringContent(serialized, Encoding.UTF8, "application/json"))
				.ConfigureAwait(false);
			if (responseMessage.IsSuccessStatusCode)
			{
				string serializedContent = await responseMessage
					.Content
					.ReadAsStringAsync()
					.ConfigureAwait(false);
				return JsonConvert.DeserializeObject<TResponse>(serializedContent);
			}

			return default(TResponse);
		}
	}
}