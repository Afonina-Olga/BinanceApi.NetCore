using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using Ardalis.GuardClauses;
using Microsoft.Extensions.DependencyInjection.Extensions;

using BinanceApi.NetCore.DependencyInjection;
using BinanceApi.NetCore.FluentApi;
using BinanceApi.NetCore.FluentApi.Settings;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class BinanceClientServiceCollectionExtensions
	{
		public static IServiceCollection AddBinanceClient(
			this IServiceCollection services,
			Action<BinanceClientConfiguration> configuration)
		{
			Guard.Against.Null(services, nameof(services));
			Guard.Against.Null(configuration, nameof(configuration));

			var config = GetConfiguration(configuration);

			// Add BinanceClient as Singleton
			services.TryAdd(new ServiceDescriptor(
				typeof(BinanceClient),
				typeof(BinanceClient),
				ServiceLifetime.Singleton));

			// Add BinanceClientSettings as Singleton
			services.TryAddSingleton(new BinanceClientSettings());

			services
				.AddHttpClient<BinanceRequestExecutor>()
				.ConfigureHttpClient((serviceProvider, httpClient) =>
				{
					httpClient
						.DefaultRequestHeaders
						.Accept
						.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				})
				.ConfigurePrimaryHttpMessageHandler(() =>
				{
					return new HttpClientHandler()
					{
						AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
					};
				})
				.SetHandlerLifetime(config.HttpMessageHandlerLifeTime);

			return services;
		}

		/// <summary>
		/// Use defaul configuration settings
		/// </summary>
		public static IServiceCollection AddBinanceClient(this IServiceCollection services)
			=> AddDefaultBinanceClient(services);

		public static IServiceCollection AddBinanceClient<TService>(this IServiceCollection services)
			where TService : class
		{
			services.TryAddSingleton<TService>();
			return AddDefaultBinanceClient(services);
		}

		public static IServiceCollection AddBinanceClient<TService, TImplementation>(this IServiceCollection services)
			where TService : class
			where TImplementation : class, TService
		{
			services.TryAddSingleton<TService, TImplementation>();
			return AddDefaultBinanceClient(services);
		}

		public static IServiceCollection AddBinanceClient<TService>(
			this IServiceCollection services,
			Action<BinanceClientConfiguration> configuration)
			where TService : class
		{
			services.TryAddSingleton<TService>();
			return services.AddBinanceClient(configuration);
		}

		public static IServiceCollection AddBinanceClient<TService, TImplementation>(
			this IServiceCollection services,
			Action<BinanceClientConfiguration> configuration)
			where TService : class
			where TImplementation : class, TService
		{
			services.TryAddSingleton<TService, TImplementation>();
			return services.AddBinanceClient(configuration);
		}

		private static IServiceCollection AddDefaultBinanceClient(IServiceCollection services) =>
			services.AddBinanceClient((configuration) => new BinanceClientConfiguration());

		private static BinanceClientConfiguration GetConfiguration(Action<BinanceClientConfiguration> configuration)
		{
			var config = new BinanceClientConfiguration();
			configuration?.Invoke(config);

			return config;
		}
	}
}
