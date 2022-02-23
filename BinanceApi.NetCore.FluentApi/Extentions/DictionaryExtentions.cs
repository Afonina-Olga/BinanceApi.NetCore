using Ardalis.GuardClauses;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BinanceApi.NetCore.FluentApi.Extentions
{
	public static class DictionaryExtentions
	{
		public static string ToQueryString(this Dictionary<string, string> parameters)
		{
			return string.Join("&",
				parameters.Select(parameter =>
				{
					var key = Guard.Against.NullOrEmpty(parameter.Key, nameof(parameter.Key), "Key is null or empty");
					var value = Guard.Against.NullOrEmpty(parameter.Value, nameof(parameter.Value), "Value is null or empty");
					return $"{parameter.Key}={HttpUtility.UrlEncode(parameter.Value)}";
				}));
		}
	}
}
