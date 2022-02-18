namespace BinanceApi.NetCore
{
	public class Configuration
	{
		public string ApiKey { get; set; }

		public string SecretKey { get; set; }

		public int DefaultReceiveWindow { get; set; } = 5000;

		public bool UseBinanceServerTime { get; set; } = true;

		public bool UseCache { get; set; } = false;
	}
}
