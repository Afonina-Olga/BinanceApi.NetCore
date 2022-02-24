using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Caching.Memory;

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
		private MemoryCache _memoryCache;
		private readonly object _lockObject = new object();
		private TimeSpan _defaultCacheTime = new TimeSpan(0, 30, 0);

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
				_apiKey = Guard.Against.NullOrEmpty(value, "api key", "Api key is null or empty");

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
				_httpClient.DefaultRequestHeaders.TryAddWithoutValidation(ApiHeader, new[] { _apiKey });
			}
		}

		#endregion

		public bool CacheEnabled { get; set; }

		public TimeSpan? CacheTime { get; set; }

		#region Ctor

		public readonly HttpClient _httpClient;

		public BinanceRequestExecutor(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_memoryCache = new MemoryCache(new MemoryCacheOptions());
		}

		#endregion

		#region ExecuteRequest

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
			bool isSignedRequest = false,
			bool useCache = false)
				where T : class
		{
			var memoryCacheKey = requestUri.AbsoluteUri.ToMemoryCacheKey<T>();

			// CacheEnabled - all requests will use cach
			// UseCache - use cache for specific requests (pass as parameter)
			if (CacheEnabled || useCache)
			{
				if (ContainsInMemoryCache(memoryCacheKey))
				{
					var cachedItem = GetFromMemoryCache<T>(memoryCacheKey);

					if (cachedItem != null)
						return cachedItem;
				}
			}

			var uri = ConfigureUri(requestUri, parameters, isSignedRequest);

			var responseMessage = requestType switch
			{
				HttpRequestType.POST => await _httpClient.PostAsync(uri, null).ConfigureAwait(false),
				HttpRequestType.GET => await _httpClient.GetAsync(uri).ConfigureAwait(false),
				HttpRequestType.DELETE => await _httpClient.DeleteAsync(uri).ConfigureAwait(false),
				HttpRequestType.PUT => await _httpClient.PutAsync(uri, null).ConfigureAwait(false),
				_ => throw new ArgumentException("HttpRequestType is unknown")
			};

			var responseResult = await
				HandleHttpResponse<T>(responseMessage, requestUri.AbsoluteUri)
				.ConfigureAwait(false);

			return responseResult;
		}

		#endregion

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
				var messageObject = typeof(T) == typeof(string) ?
					(T)(object)messageJson :
					messageJson.DeserializeJson<T>(requestUrl, statusCode);

				var memoryCacheKey = requestUrl.ToMemoryCacheKey<T>();
				RemoveFromMemoryCache(memoryCacheKey);
				AddInMemoryCache(messageObject, memoryCacheKey, CacheTime);
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

		#region Memory cache

		/// <summary>
		/// Remove an item from the cache
		/// </summary>
		/// <param name="key">The key to identify the item in the cache</param>
		public void RemoveFromMemoryCache(string key)
		{
			if (!ContainsInMemoryCache(key)) return;

			lock (_lockObject)
			{
				if (ContainsInMemoryCache(key))
				{
					_memoryCache.Remove(key.ToLower());
				}
			}
		}

		/// <summary>
		/// Retrieve an item from the cache
		/// </summary>
		/// <typeparam name="T">The type of the item</typeparam>
		/// <param name="key">The key to identify the cache item</param>
		/// <returns>Item from the cache</returns>
		public T GetFromMemoryCache<T>(string key) where T : class
		{
			var cachedItem = _memoryCache.Get(
				Guard
				.Against
				.NullOrEmpty(key, nameof(key), "Memory cache key is null or empty")
				.ToLower());
			return cachedItem as T;
		}

		/// <summary>
		/// Check if the cache contains an item
		/// </summary>
		/// <param name="key">The key to identify the cache entry</param>
		/// <returns>If memory cache contains item</returns>
		public bool ContainsInMemoryCache(string key)
		{
			if (!key.IsNullOrEmpty())
				return _memoryCache.Get(key.ToLower()) != null;

			return false;
		}

		/// <summary>
		/// Add an item to the cache
		/// </summary>
		/// <typeparam name="T">Type of item to be added</typeparam>
		/// <param name="iyem">The item to add</param>
		/// <param name="key">The key to identify the cache item</param>
		/// <param name="expiry">When the cache should expire</param>
		public void AddInMemoryCache<T>(T item, string key, TimeSpan? expiry = null) where T : class
		{
			if (ContainsInMemoryCache(key)) return;

			if (expiry == null)
				expiry = _defaultCacheTime;

			lock (_lockObject)
			{
				_memoryCache.Set(
					key.ToLower(),
					item,
					new DateTimeOffset(DateTime.UtcNow.Add(expiry.Value)));
			}
		}

		#endregion
	}
}