using System;

namespace BinanceApi.NetCore.FluentApi.Exceptions
{
	/// <summary>
	/// This exception is used when binance server response is success but errors occured during handle this response
	/// </summary>
	public class BinanceClientException : BinanceException
	{
		public BinanceClientException(
			string url,
			int statusCode,
			string message)
				: base(url, statusCode, "Binance client exception (response is success)") { }

		public BinanceClientException(
			string url,
			int statusCode,
			string message,
			Exception innerException)
				: base(url, statusCode, "Binance client exception (response is success)", innerException) { }

		public BinanceClientException(
			string url,
			int statusCode,
			BinanceError details,
			string message)
				: base(url, statusCode, details, "Binance client exception (response is success)") { }

		public BinanceClientException(
			string url,
			int statusCode,
			BinanceError details,
			string message,
			Exception innerException)
				: base(url, statusCode, details, "Binance client exception (response is success)", innerException) { }
	}
}
