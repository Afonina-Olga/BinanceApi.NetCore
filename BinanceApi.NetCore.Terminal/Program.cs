using System;
using System.Threading.Tasks;
using BinanceApi.NetCore.Domain.Response;
using BinanceApi.NetCore.Endpoints;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BinanceApi.NetCore.Terminal
{
	class Program
	{
		private static IHost _hosting;

		public static IHost Hosting => _hosting ??= CreateHostBuilder(Environment.GetCommandLineArgs()).Build();

		public static IServiceProvider Services => Hosting.Services;

		public static IHostBuilder CreateHostBuilder(string[] args) => Host
			.CreateDefaultBuilder(args)
			.ConfigureServices(ConfigureServices);

		private static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
		{
			//services.AddBinanceClient(client =>
			//{
			//	client
			//		.UseApiKeys("apiKey", "secretKey")
			//		.AsSingleton();
			//});

			//services.AddBinanceClient(); // Все настройки по умолчанию, ключи будут установлены в процессе работы
			//services.AddBinanceClient(client => // Установка настроек, использую BinanceClient
			//{

			//});
			//services.AddBinanceClient<MyBinanceClient>();
			//services.AddBinanceClient<MyBinanceClient>(client => { 
			//// настройки
			//});

			services
				.AddHttpClient<BinanceHttpClientProvider>()
				.ConfigureHttpClient((serviceProvider, httpClient) =>
				{
					httpClient.BaseAddress = new Uri("https://api.binance.com");
				});
			//services.AddSingleton(<IBinanceClient, BinanceClient>();
		}

		public static async Task Main(string[] args)
		{
			using var host = Hosting;
			await host.StartAsync();

			var result1 = new BinanceClient()
				.Using<Trade>()
				.CreateNewOrder()
				.WithSimbol("")
				.ForBuy()
				.SetNewClientOrderId("")
				.SetReciveWindow(6000)
				.SetOrderType<MarketOrderType>();

			// Using(x=> x.Endpoints.Trade)
			await host.StopAsync();

			Console.WriteLine("Завершено!");
			Console.ReadLine();
		}
	}
}
