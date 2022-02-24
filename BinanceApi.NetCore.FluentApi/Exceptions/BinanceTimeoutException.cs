using System;

namespace BinanceApi.NetCore.FluentApi.Exceptions
{
	/// <summary>
	/// Binance timeout exception (request was valid, but timeout occured)
	/// </summary>
	/// <summary>
	public class BinanceTimeoutException : BinanceException
	{
		public BinanceTimeoutException(
			string url,
			int statusCode)
				: base(url, statusCode, "Binance timeout exception") { }

		public BinanceTimeoutException(
			string url,
			int statusCode,
			Exception innerException)
				: base(url, statusCode, "Binance timeout exception", innerException) { }

		public BinanceTimeoutException(
			string url,
			int statusCode,
			BinanceError details)
				: base(url, statusCode, details, "Binance timeout exception") { }

		public BinanceTimeoutException(
			string url,
			int statusCode,
			BinanceError details,
			Exception innerException)
				: base(url, statusCode, details, "Binance timeout exception", innerException) { }
	}
}
