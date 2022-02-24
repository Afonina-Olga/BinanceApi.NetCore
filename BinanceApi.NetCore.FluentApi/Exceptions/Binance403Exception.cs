using System;

namespace BinanceApi.NetCore.FluentApi.Exceptions
{
	/// <summary>
	/// This exception is used to process response from server with 403 status code (because of possible ban)
	/// </summary>
	public class Binance403Exception : BinanceException
	{
		public Binance403Exception(
			string url,
			int statusCode)
				: base(url, statusCode, "Binance 403 exception") { }

		public Binance403Exception(
			string url,
			int statusCode,
			Exception innerException)
				: base(url, statusCode, "Binance 403 exception", innerException) { }

		public Binance403Exception(
			string url,
			int statusCode,
			BinanceError details)
				: base(url, statusCode, details, "Binance 403 exception") { }

		public Binance403Exception(
			string url,
			int statusCode,
			BinanceError details,
			Exception innerException)
				: base(url, statusCode, details, "Binance 403 exception", innerException) { }
	}
}
