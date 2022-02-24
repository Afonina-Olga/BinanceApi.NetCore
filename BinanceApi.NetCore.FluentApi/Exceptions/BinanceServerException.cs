using System;

namespace BinanceApi.NetCore.FluentApi.Exceptions
{
	/// <summary>
	/// This exception is used when the error occured on binance server side (request is valid)
	/// </summary>
	public class BinanceServerException : BinanceException
	{
		public BinanceServerException(
			string url,
			int statusCode)
				: base(url, statusCode, "Binance server exception") { }

		public BinanceServerException(
			string url,
			int statusCode,
			Exception innerException)
				: base(url, statusCode, "Binance server exception", innerException) { }

		public BinanceServerException(
			string url,
			int statusCode,
			BinanceError details)
				: base(url, statusCode, details, "Binance server exception") { }

		public BinanceServerException(
			string url,
			int statusCode,
			BinanceError details,
			Exception innerException)
				: base(url, statusCode, details, "Binance server exception", innerException) { }
	}
}
