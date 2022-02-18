using BinanceApi.NetCore.Endpoints;

namespace BinanceApi.NetCore
{
	public class BinanceClient
	{
		/// <summary>
		/// Set endpoint group
		/// </summary>
		public TEndpoint Using<TEndpoint>() where TEndpoint : IEndpoint, new()
		{
			return new TEndpoint();
		}
	}
}
