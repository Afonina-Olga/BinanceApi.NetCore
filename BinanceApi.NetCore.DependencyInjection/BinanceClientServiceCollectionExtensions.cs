using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

using Polly;
using Polly.Extensions.Http;
using Ardalis.GuardClauses;

using BinanceApi.NetCore;
using BinanceApi.NetCore.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using BinanceApi.NetCore.FluentApi;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class BinanceClientServiceCollectionExtensions
	{
		public static IServiceCollection AddBinanceClient(
			this IServiceCollection services,
			Action<IBinanceClientConfiguration> configuration)
		{
			Guard.Against.Null(services, nameof(services));
			Guard.Against.Null(configuration, nameof(configuration));

			var config = GetConfiguration(configuration);

			services.TryAdd(new ServiceDescriptor(
				typeof(BinanceClient),
				typeof(BinanceClient),
				config.Lifetime));

			services
				.AddHttpClient<BinanceRequestExecutor>()
				.ConfigureHttpClient((serviceProvider, httpClient) =>
				{
					httpClient.BaseAddress = new Uri("https://api.binance.com");
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
				.SetHandlerLifetime(TimeSpan.FromMinutes(5))
				.AddPolicyHandler(GetRetryPolicy(config.RetryAttempts));

			return services;
		}

		public static IServiceCollection AddBinanceClient(this IServiceCollection services)
		{
			// TODO
			return services;
		}

		private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy(int retryAttempts)
		{
			var jitter = new Random();
			return
				HttpPolicyExtensions
					.HandleTransientHttpError()
					.WaitAndRetryAsync(retryAttempts, retryAttempt =>
						TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) +
						TimeSpan.FromMilliseconds(jitter.Next(0, 1000)));
		}

		private static BinanceClientConfiguration GetConfiguration(Action<IBinanceClientConfiguration> configuration)
		{
			var config = new BinanceClientConfiguration();
			configuration?.Invoke(config);

			return config;
		}
	}
}
