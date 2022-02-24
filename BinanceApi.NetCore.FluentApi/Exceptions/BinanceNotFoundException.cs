using System;

namespace BinanceApi.NetCore.FluentApi.Exceptions
{
	/// <summary>
	/// This exception is used when server returned NotFound (404) status 
	/// </summary>
	public class BinanceNotFoundException : BinanceException
	{
		public BinanceNotFoundException(
			string url,
			int statusCode)
				: base(url, statusCode, "Not found exception") { }

		public BinanceNotFoundException(
			string url,
			int statusCode,
			Exception innerException)
				: base(url, statusCode, "Not found exception", innerException) { }

		public BinanceNotFoundException(
			string url,
			int statusCode,
			BinanceError details)
				: base(url, statusCode, details, "Not found exception") { }

		public BinanceNotFoundException(
			string url,
			int statusCode,
			BinanceError details,
			Exception innerException)
				: base(url, statusCode, details, "Not found exception", innerException) { }
	}
}
