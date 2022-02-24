using System;

namespace BinanceApi.NetCore.FluentApi.Exceptions
{
	/// <summary>
	/// This exception is used when malformed requests are sent to the binance server
	/// </summary>
	public class BinanceBadRequestException : BinanceException
	{
		public BinanceBadRequestException(
			string url,
			int statusCode)
				: base(url, statusCode, "Binance bad request") { }

		public BinanceBadRequestException(
			string url,
			int statusCode,
			Exception innerException)
				: base(url, statusCode, "Binance bad request", innerException) { }

		public BinanceBadRequestException(
			string url,
			int statusCode,
			BinanceError details)
				: base(url, statusCode, details, "Binance bad request") { }

		public BinanceBadRequestException(
			string url,
			int statusCode,
			BinanceError details,
			Exception innerException)
				: base(url, statusCode, details, "Binance bad request", innerException) { }
	}
}
