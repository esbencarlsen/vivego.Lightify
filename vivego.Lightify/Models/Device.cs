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

using System.Collections.Generic;

using Newtonsoft.Json;

namespace vivego.Lightify.Models
{
	/*
	* 
	{
		"deviceId": 1,
		"deviceType": "LIGHT",
		"manufacturer": "OSRAM",
		"modelName": "Classic A60 RGBW",
		"name": "A60RGBW 01",
		"groupList": [ 1 ],
		"bmpClusters": ["onOff", "Level", "Color", "Temperature" ],
		"online": 1,
		"on": 1,
		"brightnessLevel": 1,
		"hue": 0, "saturation": 0,
		"temperature": 2702,
		"firmwareVersion": "01020412",
		"color": "FFFFFF"
	}
	*
	*/

	public class Device
	{
		public Device()
		{
			GroupList = new List<long>();
			BmpClusters = new List<string>();
		}

		[JsonProperty("deviceId")]
		public long DeviceId { get; set; }

		[JsonProperty("deviceType")]
		public string DeviceType { get; set; }

		[JsonProperty("manufacturer")]
		public string Manufacturer { get; set; }

		[JsonProperty("modelName")]
		public string ModelName { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("groupList")]
		public IList<long> GroupList { get; set; }

		[JsonProperty("bmpClusters")]
		public IList<string> BmpClusters { get; set; }

		[JsonProperty("online")]
		public long Online { get; set; }

		[JsonProperty("on")]
		public long On { get; set; }

		[JsonProperty("brightnessLevel")]
		public decimal BrightnessLevel { get; set; }

		[JsonProperty("hue")]
		public decimal Hue { get; set; }

		[JsonProperty("saturation")]
		public decimal Saturation { get; set; }

		[JsonProperty("temperature")]
		public long Temperature { get; set; }

		[JsonProperty("firmwareVersion")]
		public string FirmwareVersion { get; set; }

		[JsonProperty("color")]
		public string Colour { get; set; }

		public static implicit operator long(Device device)
		{
			return device.DeviceId;
		}
	}
}