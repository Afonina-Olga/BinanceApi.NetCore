using Ardalis.GuardClauses;
using System;

namespace BinanceApi.NetCore.DependencyInjection
{
	public class BinanceClientConfiguration
	{
		#region ApiKey

		/// <summary>
		/// Binance api key
		/// </summary>
		private string _apiKey;
		public string ApiKey
		{
			private get { return _apiKey; }
			set
			{
				_apiKey = Guard.Against.NullOrEmpty(value, nameof(ApiKey), "Api key is null or empty");
			}
		}

		#endregion

		#region SecretKey

		/// <summary>
		/// Binance secret key
		/// </summary>
		private string _secretKey;
		public string SecretKey
		{
			private get { return _secretKey; }
			set
			{
				_secretKey = Guard.Against.NullOrEmpty(value, nameof(SecretKey), "Secret key is null or empty");
			}
		}

		#endregion

		#region ReceiveWindow

		/// <summary>
		/// Receive window for request (must be between 5000 and 60000)
		/// </summary>
		private int _ReceiveWindow = 5000;
		public int ReceiveWindow
		{
			get { return _ReceiveWindow; }
			set
			{
				_ReceiveWindow = Guard.Against.OutOfRange(
					value,
					nameof(ReceiveWindow),
					5000,
					60000,
					"RecvWindow must be between 5000 and 60000");
			}
		}

		#endregion

		/// <summary>
		/// Set httpMessageHandlerLifeTime for HttpClient
		/// </summary>
		public TimeSpan HttpMessageHandlerLifeTime { get; private set; } = TimeSpan.FromMinutes(5);

		/// <summary>
		/// Use binance server time in requestst
		/// </summary>
		public bool ServerTimeEnabled { get; set; } = true;

		/// <summary>
		/// Use memory cache for all requests
		/// </summary>
		public bool CacheEnabled { get; set; } = false;

		/// <summary>
		/// Set memory cache time
		/// </summary>
		public TimeSpan CacheTime { get; set; } = new TimeSpan(0, 30, 0);

		/// <summary>
		/// Number of requests for LimitSeconds (10 seconds for 10 requests etc)
		/// </summary>
		public int LimitRequests { get; set; } = 10;

		/// <summary>
		/// Number of seconds the for LimitRequests (10 seconds for 10 requests etc)
		/// </summary>
		public int LimitSeconds { get; set; } = 10;

		/// <summary>
		/// Set rate limit policy mode
		/// </summary>
		public bool RateLimitEnabled { get; set; } = true;

		/// <summary>
		/// Set timestampOffset for request (add milliseconds to timestamp)
		/// </summary>
		public long TimestampOffset { get; set; } = 1000;
	}
}
