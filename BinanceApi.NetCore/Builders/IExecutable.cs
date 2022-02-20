using BinanceApi.NetCore.Domain.Response;

namespace BinanceApi.NetCore.Builders
{
	public interface IExecutable
	{
		/// <summary>
		/// Execute post request
		/// </summary>
		INewOrderMarketRequestBuilder ExecuteAsync<TResponse>() where TResponse : NewOrderResponseAsk;

		/// <summary>
		/// Execute post request
		/// </summary>
		INewOrderMarketRequestBuilder ExecuteAsync();
	}
}
