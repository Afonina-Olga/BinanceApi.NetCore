using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;

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
		/// Execute get request to the specific endpoint
		/// </summary>
		/// <param name="requestUri">Endpoint uri</param>
		/// <param name="parameters">Request parameters</param>
		/// <param name="isSignedRequest">Need signature for request?</param>
		/// <returns>HttpResponseMessage</returns>
		internal async Task<HttpResponseMessage> ExecuteGetRequest(Uri requestUri, Dictionary<string, string> parameters, bool isSignedRequest = false)
		{
			var uri = ConfigureUri(requestUri, parameters, isSignedRequest);
			return await _httpClient.GetAsync(uri).ConfigureAwait(false);
		}

		/// <summary>
		/// Execute post request to the specific endpoint
		/// </summary>
		/// <param name="requestUri">Endpoint uri</param>
		/// <param name="parameters">Request parameters</param>
		/// <param name="isSignedRequest">Need signature for request?</param>
		/// <returns>HttpResponseMessage</returns>
		internal async Task<HttpResponseMessage> ExecutePostRequest(Uri requestUri, Dictionary<string, string> parameters, bool isSignedRequest = false)
		{
			var uri = ConfigureUri(requestUri, parameters, isSignedRequest);
			return await _httpClient.PostAsync(uri, null).ConfigureAwait(false);
		}

		internal async Task<HttpResponseMessage> ExecutePutRequest(Uri requestUri, Dictionary<string, string> parameters, bool isSignedRequest = false)
		{
			var uri = ConfigureUri(requestUri, parameters, isSignedRequest);
			return await _httpClient.PutAsync(uri, null).ConfigureAwait(false);
		}

		internal async Task<HttpResponseMessage> ExecuteDeleteRequest(Uri requestUri, Dictionary<string, string> parameters, bool isSignedRequest = false)
		{
			var uri = ConfigureUri(requestUri, parameters, isSignedRequest);
			return await _httpClient.DeleteAsync(uri).ConfigureAwait(false);
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
	}
}
