using BinanceApi.NetCore.Domain.Response;

namespace BinanceApi.NetCore.FluentApi.Builders
{
	public interface INewOrderMarketRequest : INewOrderTypeBaseRequest
	{
		/// <summary>
		/// Set quoteOrderQty param
		/// </summary>
		/// <param name="quoteOrderQty">Specifies the amount the user wants to spend (when buying) or receive (when selling) of the quote asset</param>
		IExecutable<NewOrderResponseAsk> WithQuoteOrderQty(decimal quoteOrderQty);
	}
}
