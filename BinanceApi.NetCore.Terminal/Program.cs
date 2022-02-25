using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
			services.AddBinanceClient(client =>
			{
				client.ServerTimeEnabled = true;
			});

			//services.AddBinanceClient();

			//services.AddBinanceClient(); // Все настройки по умолчанию, ключи будут установлены в процессе работы
			//services.AddBinanceClient(client => // Установка настроек, использую BinanceClient
			//{

			//});
			//services.AddBinanceClient<MyBinanceClient>();
			//services.AddBinanceClient<MyBinanceClient>(client => { 
			//// настройки
			//});

		}

		public static async Task Main(string[] args)
		{
			using var host = Hosting;
			await host.StartAsync();

			var client = host.Services.GetRequiredService<BinanceClient>();
			client
				.UsingSpotAccountTradeEndpoints()
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

			await host.StopAsync();

			Console.WriteLine("Завершено!");
			Console.ReadLine();

			//var p = Policy
			//	.Handle<HttpRequestException>()
			//	.Or<BinanceException>()
			//	.WaitAndRetryAsync(retryAttempts, retryAttempt =>
			//		TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) +
			//		TimeSpan.FromMilliseconds(jitter.Next(0, 1000)));
		}
	}
}
