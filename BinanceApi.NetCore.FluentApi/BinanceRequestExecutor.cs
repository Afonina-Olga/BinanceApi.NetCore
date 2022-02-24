using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;

using BinanceApi.NetCore.FluentApi.Exceptions;
using BinanceApi.NetCore.FluentApi.Extentions;

namespace BinanceApi.NetCore.FluentApi
{
	public class BinanceRequestExecutor
	{
		#region Private fields

		private string _secretKey;
		private readonly long _timestampOffset = 1000;
		private readonly long _receiveWindow = 5000;

		#endregion

		#region ApiKey

		private string _apiKey;
		private readonly string ApiHeader = "X-MBX-APIKEY";
		private static readonly object LockObject = new object();

		public string ApiKey
		{
			get { return _apiKey; }
			set
			{
				var key = Guard.Against.NullOrEmpty(value, "api key", "Api key is null or empty");

				if (_httpClient.DefaultRequestHeaders.Contains(ApiHeader))
				{
					lock (LockObject)
					{
						if (_httpClient.DefaultRequestHeaders.Contains(ApiHeader))
						{
							_httpClient.DefaultRequestHeaders.Remove(ApiHeader);
						}
					}
				}
				_httpClient.DefaultRequestHeaders.TryAddWithoutValidation(ApiHeader, new[] { key });
			}
		}

		#endregion

		public readonly HttpClient _httpClient;

		public BinanceRequestExecutor(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		/// <summary>
		/// Execute generic request to the specific endpoint
		/// </summary>
		/// <param name="requestUri">Endpoint uri</param>
		/// <param name="requestType">Http request type (GET, POST, PUT, DELETE)</param>
		/// <param name="parameters">Request parameters</param>
		/// <param name="isSignedRequest">Need signature for request?</param>
		/// <returns>Deserialized object from json string</returns>
		internal async Task<T> ExecuteRequest<T>(
			Uri requestUri,
			HttpRequestType requestType,
			Dictionary<string, string> parameters,
			bool isSignedRequest = false)
				where T : class
		{
			var uri = ConfigureUri(requestUri, parameters, isSignedRequest);

			var responseMessage = requestType switch
			{
				HttpRequestType.POST => await _httpClient.PostAsync(uri, null).ConfigureAwait(false),
				HttpRequestType.GET => await _httpClient.GetAsync(uri).ConfigureAwait(false),
				HttpRequestType.DELETE => await _httpClient.DeleteAsync(uri).ConfigureAwait(false),
				HttpRequestType.PUT => await _httpClient.PutAsync(uri, null).ConfigureAwait(false),
				_ => throw new ArgumentException("HttpRequestType is unknown")
			};
			
			var responseResult = await HandleHttpResponse<T>(responseMessage, requestUri.ToString()).ConfigureAwait(false);
			return responseResult;
		}

		#region Configure url

		/// <summary>
		/// HMACSHA256 signature based on the API Secret and request parameters
		/// </summary>
		/// <param name="secretKey">Api secret key</param>
		/// <param name="totalParams">Request parameters</param>
		/// <returns>HMAC SHA256 signature string/returns>
		/// <see cref="https://binance-docs.github.io/apidocs/spot/en/#signed-trade-user_data-and-margin-endpoint-security"/>
		private string CreateHMACSignature(string secretKey, string totalParams)
		{
			var totalParamsBytes = Encoding.UTF8.GetBytes(totalParams);
			var secretKeysBytes = Encoding.UTF8.GetBytes(secretKey);
			using var hash = new HMACSHA256(secretKeysBytes);
			var computedHash = hash.ComputeHash(totalParamsBytes);
			return BitConverter.ToString(computedHash).Replace("-", "").ToLower();
		}

		/// <summary>
		/// Create uri with query parameters and signature
		/// </summary>
		/// <param name="endpoint">Endpoint uri</param>
		/// <param name="parameters">Query parameters collection</param>
		/// <returns>Configured uri</returns>
		private Uri ConfigureSignedUri(Uri endpoint, Dictionary<string, string> parameters)
		{
			var url = Guard.Against.Null(endpoint, nameof(endpoint), "Endpoint url is null");

			// TODO: Возможность использовать серверное время
			// и задавать timestampOffset через настройки
			var timestamp = DateTimeOffset.UtcNow.AddMilliseconds(_timestampOffset).ToUnixTimeMilliseconds().ToString();

			if (parameters == null)
				parameters = new Dictionary<string, string>();

			if (!parameters.TryAdd("timestamp", timestamp))
				throw new ArgumentException("Unable to add timestamp to params collection");

			if (!parameters.TryAdd("recvWindow", _receiveWindow.ToString()))
				throw new ArgumentException("Unable to add recvWindow to params collection");

			var queryString = parameters.ToQueryString();
			var signature = CreateHMACSignature(_secretKey, queryString);
			return new Uri($"{endpoint}?{queryString}&signature={signature}");
		}

		/// <summary>
		/// Configure uri
		/// </summary>
		/// <param name="requestUri">Endpoint uri</param>
		/// <param name="parameters">Request parameters</param>
		/// <param name="isSignedRequest">Is signed request</param>
		/// <returns>Configured uri</returns>
		private Uri ConfigureUri(Uri requestUri, Dictionary<string, string> parameters, bool isSignedRequest)
		{
			return isSignedRequest ? ConfigureSignedUri(requestUri, parameters) : requestUri;
		}

		#endregion

		#region HandleHttpResponse

		/// <summary>
		/// Handle http response (generic)
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		/// <param name="responseMessage">Response message from request to specific endpoint</param>
		/// <param name="requestUrl">Endpoint</param>
		/// <returns>Deserialized object from json response</returns>
		private async Task<T> HandleHttpResponse<T>(HttpResponseMessage responseMessage, string requestUrl) where T : class
		{
			var messageJson = await responseMessage.Content.ReadAsStringAsync();
			var statusCode = (int)responseMessage.StatusCode;

			if (responseMessage.IsSuccessStatusCode)
			{
				if (typeof(T) == typeof(string))
				{
					return (T)(object)messageJson;
				}
				else
				{
					return messageJson.DeserializeJson<T>(requestUrl, statusCode);
				}
			}

			var error = messageJson.DeserializeJson<BinanceError>(requestUrl, statusCode);

			// Process binance exception
			throw statusCode switch
			{
				(int)HttpStatusCode.GatewayTimeout => new BinanceTimeoutException(requestUrl, statusCode, error),
				(int)HttpStatusCode.NotFound => new BinanceNotFoundException(requestUrl, statusCode, error),
				(int)HttpStatusCode.Forbidden => new Binance403Exception(requestUrl, statusCode, error),
				int code when code >= 400 && code <= 500 => new BinanceBadRequestException(requestUrl, statusCode, error),
				int code when code > 500 => new BinanceServerException(requestUrl, statusCode, error),
				_ => new BinanceException(requestUrl, statusCode, error)
			};
		}

		#endregion
	}
}