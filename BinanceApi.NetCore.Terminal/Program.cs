using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using BinanceApi.NetCore.FluentApi.Endpoints;
using BinanceApi.NetCore.FluentApi;

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
				.AddHttpClient<BinanceRequestExecutor>()
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

			new BinanceClient()
				.Endpoint<Trade>()
				.CreateNewOrder()
				.WithSimbol("BTCUSDT")
				.ForBuy()
				.AdvancedOptions(options =>
				{
					options.NewClientOrderId = "";
					options.ReciveWindow = 5000;
				})
				.SetMarketOrderType()
				.WithQuantity(20)
				.ExecuteAsync(); // ExecuteAsync(NewOrderResponseType.FULL)

			// AdditionalParameters только после ввода основных
			//new BinanceClient()
			//	.Using<Trade>()
			//	.CreateNewOrder(1, 2, 3) // параметры вместо method chaining
			//	.ExecuteAsync<NewOrderResponseFull>();

			await host.StopAsync();

			Console.WriteLine("Завершено!");
			Console.ReadLine();
		}
	}
}
