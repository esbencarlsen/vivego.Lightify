/**
 * Copyright 2016 d-fens GmbH
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;

namespace vivego.Lightify
{
	public class Constants
	{
		public static class OsramGatewayEndpoints
		{
			public static readonly string s_OsramLightifyBaseUriTemplate = "https://{0}.lightify-api.org/lightify/services/";
			
			public static readonly Uri s_OsramEuLightifyBaseUri = new Uri(string.Format(s_OsramLightifyBaseUriTemplate, "eu"));
			public static readonly Uri s_OsramUsLightifyBaseUri = new Uri(string.Format(s_OsramLightifyBaseUriTemplate, "us"));
		}

		public static class ApiOperation
		{
			public const string DeviceSet = "device/set";
			public const string Devices = "devices";
			public const string Gateway = "gateway";
			public const string GroupSet = "group/set";
			public const string Groups = "groups";
			public const string Session = "session";
			public const string Version = "version";
		}

		public static class QueryParameter
		{
			public const string Idx = "idx";
			public const string OnOff = "onoff";
			public const string Level = "level";
			public const string Time = "time";
			public const string Saturation = "saturation";
			public const string Ctemp = "ctemp";
			public const string Color = "color";
		}

		public static class Messages
		{
			public const string ClientNotLoggedIn = "Perform getToken before using the client.";
			public const string GetDevicesDuplicateNameError = "Duplicate name found.";
		}

		public class HttpHeaders
		{
			public const string Authorization = "Authorization";
		}
	}
}