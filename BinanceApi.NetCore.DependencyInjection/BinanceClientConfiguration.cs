using Ardalis.GuardClauses;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BinanceApi.NetCore.DependencyInjection
{
	public class BinanceClientConfiguration : IBinanceClientConfiguration
	{
		public string ApiKey { get; private set; }

		public string SecretKey { get; private set; }

		public ServiceLifetime Lifetime { get; private set; } = ServiceLifetime.Singleton;

		public TimeSpan HttpMessageHandlerLifeTime { get; private set; } = TimeSpan.FromMinutes(5);

		public int RetryAttempts { get; private set; } = 5;

		public bool IsServerTime { get; set; } = true;

		public BinanceClientConfiguration AsSingleton()
		{
			Lifetime = ServiceLifetime.Singleton;
			return this;
		}

		public BinanceClientConfiguration AsScoped()
		{
			Lifetime = ServiceLifetime.Scoped;
			return this;
		}

		public BinanceClientConfiguration AsTransient()
		{
			Lifetime = ServiceLifetime.Transient;
			return this;
		}

		public BinanceClientConfiguration UseApiKeys(string apiKey, string secretKey)
		{
			ApiKey = Guard.Against.NullOrEmpty(apiKey, nameof(apiKey), "Api key is null or empty");
			SecretKey = Guard.Against.NullOrEmpty(secretKey, nameof(secretKey), "Secret key is null or empty");
			return this;
		}

		public BinanceClientConfiguration UseServerTime(bool useServerTime)
		{
			IsServerTime = true;
			return this;
		}

		public BinanceClientConfiguration SetHttpMessageHandlerLifeTime(TimeSpan time)
		{
			HttpMessageHandlerLifeTime = time;
			return this;
		}

		public BinanceClientConfiguration SetRequestRetryAttempsCount(int retryAttempts)
		{
			RetryAttempts = retryAttempts;
			return this;
		}
	}
}
