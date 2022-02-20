using System.Net.Http;

namespace BinanceApi.NetCore
{
	public class BinanceHttpClientProvider
	{
		//private string _apiKey;
		//private string _secretKey;
		public readonly HttpClient _httpClient;

		public BinanceHttpClientProvider(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}
	}
}
