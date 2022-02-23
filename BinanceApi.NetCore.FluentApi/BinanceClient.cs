using BinanceApi.NetCore.FluentApi.Endpoints;

namespace BinanceApi.NetCore.FluentApi
{
	public class BinanceClient
	{
		/// <summary>
		/// Set endpoint group
		/// </summary>
		public TEndpoint Endpoint<TEndpoint>() where TEndpoint : IEndpoint, new()
		{
			return new TEndpoint();
		}
	}
}
