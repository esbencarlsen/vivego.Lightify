using System.Net;

namespace vivego.Lightify.HttpHandlers
{
	public static class QueryParamsHelper
	{
		public static string WithQuery(this string uri, string paramName, string paramValue)
		{
			paramValue = WebUtility.UrlEncode(paramValue);
			if (!uri.Contains("?"))
			{
				return $"{uri}?{paramName}={paramValue}";
			}

			return $"{uri}&{paramName}={paramValue}";
		}
	}
}