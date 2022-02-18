using System;

namespace BinanceApi.NetCore.DependencyInjection
{
	public interface IBinanceClientConfiguration
	{
		/// <summary>
		/// AddSingleton BinanceClient
		/// </summary>
		BinanceClientConfiguration AsSingleton();

		/// <summary>
		/// AddScoped BinanceClient
		/// </summary>
		BinanceClientConfiguration AsScoped();

		/// <summary>
		/// AddTransient BinanceClient
		/// </summary>
		BinanceClientConfiguration AsTransient();

		/// <summary>
		/// Установить секретные ключи
		/// </summary>
		/// <param name="apiKey">Api key</param>
		/// <param name="secretKey">Secret key</param>
		/// <returns></returns>
		BinanceClientConfiguration UseApiKeys(string apiKey, string secretKey);

		/// <summary>
		/// Установить период, в течение которого HttpMessageHandler может быть использован повторно
		/// </summary>
		/// <param name="time">Время</param>
		/// <returns></returns>
		BinanceClientConfiguration SetHttpMessageHandlerLifeTime(TimeSpan time);

		/// <summary>
		/// Задать количество повторных попыток выполнить запрос при ошибке (HttpRequestException, 5XX (server errors), 408 (request timeout))
		/// </summary>
		BinanceClientConfiguration SetRequestRetryAttempsCount(int retryAttempts);
	}
}
