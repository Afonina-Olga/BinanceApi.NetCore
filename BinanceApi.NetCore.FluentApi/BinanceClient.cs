using Ardalis.GuardClauses;
using BinanceApi.NetCore.FluentApi.Endpoints;
using BinanceApi.NetCore.FluentApi.Settings;

namespace BinanceApi.NetCore.FluentApi
{
	public class BinanceClient
	{
		private readonly BinanceRequestExecutor _executor;

		public BinanceClientSettings Settings { get; set; }

		public BinanceClient(BinanceRequestExecutor executor, BinanceClientSettings settings)
		{
			_executor = executor;
			Settings = settings;
		}
		
		public TEndpoint Using<TEndpoint>() where TEndpoint : IEndpoint, new()
		{
			return new TEndpoint();
		}

		public SpotAccountTradeEndpoints UsingSpotAccountTradeEndpoints()
		{
			return new SpotAccountTradeEndpoints(_executor);
		}

		public void SetApiKeys(string apiKey,string secretKey)
		{
			_executor.ApiKey = Guard.Against.NullOrEmpty(apiKey, nameof(apiKey), "Api key is null or empty");
			_executor.SecretKey = Guard.Against.NullOrEmpty(secretKey, nameof(secretKey), "Secret key is null or empty");
		}
	}
}
