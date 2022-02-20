namespace BinanceApi.NetCore.Builders
{
	public interface INewOrderMarketRequestBuilder : INewOrderTypeBaseRequestBuilder
	{
		/// <summary>
		/// Set quoteOrderQty param
		/// </summary>
		/// <param name="quoteOrderQty">Specifies the amount the user wants to spend (when buying) or receive (when selling) of the quote asset</param>
		IExecutable WithQuoteOrderQty(decimal quoteOrderQty);
	}
}
