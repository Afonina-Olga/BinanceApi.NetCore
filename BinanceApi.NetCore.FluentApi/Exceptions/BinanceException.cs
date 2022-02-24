using System;
using System.Runtime.Serialization;

namespace BinanceApi.NetCore.FluentApi.Exceptions
{
	/// <summary>
	/// Base binance exception
	/// </summary>
	[Serializable]
	public class BinanceException : Exception
	{
		/// <summary>
		/// Request url
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// Response status code
		/// </summary>
		public int StatusCode { get; set; }

		/// <summary>
		/// Error thrown by binance server with statusCode and message
		/// </summary>
		/// <see href="https://binance-docs.github.io/apidocs/spot/en/#error-codes"/>
		public BinanceError Details { get; set; }

		public BinanceException(
			string url,
			int statusCode)
		{
			Url = url;
			StatusCode = statusCode;
		}

		public BinanceException(
			string url,
			int statusCode,
			string message)
				: base(message)
		{
			Url = url;
			StatusCode = statusCode;
		}

		public BinanceException(
			string url,
			int statusCode,
			string message,
			Exception innerException)
				: base(message, innerException)
		{
			Url = url;
			StatusCode = statusCode;
		}

		public BinanceException(
			string url,
			int statusCode,
			BinanceError details)
				: this(url, statusCode)
		{
			Details = details;
		}

		public BinanceException(
			string url,
			int statusCode,
			BinanceError details,
			string message)
				: this(url, statusCode, message)
		{
			Details = details;
		}

		public BinanceException(
			string url,
			int statusCode,
			BinanceError details,
			string message,
			Exception innerException)
				: this(url, statusCode, message, innerException)
		{
			Details = details;
		}

		protected BinanceException(
		  SerializationInfo info,
		  StreamingContext context) : base(info, context)
		{
			if (info != null)
			{
				Url = info.GetString("Url");
				StatusCode = info.GetInt32("StatusCode");
				Details = info.GetValue("Details", typeof(BinanceError)) as BinanceError;
			}
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info, context);
			info.AddValue("Url", Url);
			info.AddValue("StatusCode", StatusCode);
			info.AddValue("Details", Details);
		}
	}
}
