using BinanceApi.NetCore.FluentApi.Endpoints;

namespace BinanceApi.NetCore.FluentApi
{
	public class BinanceClient
	{
		public SpotAccountTradeEndpoints UsingSpotAccountTradeEndpoints()
		{
			return new SpotAccountTradeEndpoints();
		}
	}
}
