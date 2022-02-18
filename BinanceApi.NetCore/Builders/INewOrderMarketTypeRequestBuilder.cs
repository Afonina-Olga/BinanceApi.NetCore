namespace BinanceApi.NetCore.Builders
{
	public interface INewOrderMarketTypeRequestBuilder : INewOrderTypeBaseRequestBuilder
	{
		/// <summary>
		/// Set quoteOrderQty param
		/// </summary>
		/// <param name="quoteOrderQty">Specifies the amount the user wants to spend (when buying) or receive (when selling) of the quote asset</param>
		void SetQuoteOrderQty(decimal quoteOrderQty);

		/// <summary>
		/// Set icebergQty parameter
		/// </summary>
		/// <param name="icebergQty"></param>
		void SetIcebergQty(decimal icebergQty);
	}
}
