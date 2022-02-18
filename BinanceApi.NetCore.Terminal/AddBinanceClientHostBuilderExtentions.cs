using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace BinanceApi.NetCore.Terminal
{
	public static class AddBinanceClientHostBuilderExtentions
	{
		public static IHostBuilder AddBinanceClient(this IHostBuilder host)
		{
			return host.ConfigureServices(services =>
			{
				
				//services.AddBinanceClient(client =>
				//{
				//	BinanceApiKey = new BinanceApiKey()
				//	{
				//		Key = "",
				//		Secret = ""
				//	}
				//});
			});
		}
	}
}
