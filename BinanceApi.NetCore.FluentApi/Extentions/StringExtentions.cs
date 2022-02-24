using System;
using Ardalis.GuardClauses;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using BinanceApi.NetCore.FluentApi.Exceptions;

namespace BinanceApi.NetCore.FluentApi.Extentions
{
	public static class StringExtentions
	{
		public static string JToken(this string json, string token)
		{
			return JObject
				.Parse(json)
				.SelectToken(token)
				.ToString();
		}

		public static T FromJson<T>(this string json)
		{
			if (json.IsNullOrEmpty())
				return default;

			return JsonConvert.DeserializeObject<T>(json);
		}

		public static bool IsNullOrEmpty(this string value) => string.IsNullOrEmpty(value);

		public static T DeserializeJson<T>(this string content, string requestUrl, int statusCode)
		{
			try
			{
				var contentObject =
					Guard
					.Against
					.NullOrEmpty(content, nameof(content), "Json string is null or empty")
					.FromJson<T>();

				if (contentObject == null)
					throw new BinanceClientException(
						requestUrl,
						statusCode,
						$"Failed to deselialize json string {content} from {requestUrl} to provided type {typeof(T)}");

				return contentObject;
			}
			catch (Exception ex)
			{
				throw new BinanceClientException(
					requestUrl,
					statusCode,
					$"Failed to deserialize json string {content} from {requestUrl}. Exception: {ex.Message}",
					ex);
			}
		}
	}
}
