using System;

namespace BinanceApi.NetCore.FluentApi.Settings
{
	public class BinanceClientSettings
	{
		public bool CacheEnabled { get; set; } = false;

		public TimeSpan CacheTime { get; set; } = new TimeSpan(0, 30, 0);

		public int LimitRequests { get; set; } = 10;

		public int LimitSeconds { get; set; } = 10;

		public bool RateLimitEnabled { get; set; } = true;

		public long TimestampOffset { get; set; } = 1000;

		public long ReceiveWindow { get; set; } = 5000;
	}
}
