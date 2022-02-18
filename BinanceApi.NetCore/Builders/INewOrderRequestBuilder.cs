namespace BinanceApi.NetCore.Builders
{
	public interface INewOrderRequestBuilder
	{
		/// <summary>
		/// Set a symbol
		/// </summary>
		INewOrderRequestBuilder WithSimbol(string name);

		/// <summary>
		/// Set OrderSide BUY
		/// </summary>
		INewOrderRequestBuilder ForBuy();

		/// <summary>
		/// Set OrderSide Sell
		/// </summary>
		INewOrderRequestBuilder ForSell();

		/// <summary>
		/// Set order type
		/// </summary>
		TOrderType SetOrderType<TOrderType>() where TOrderType : INewOrderTypeBaseRequestBuilder, new();

		/// <summary>
		/// Set an unique id among open orders. Automatically generated if not sent.
		/// </summary>
		INewOrderRequestBuilder SetNewClientOrderId(string orderId);

		/// <summary>
		/// RecvWindow may be sent to specify the number of milliseconds after timestamp the request is valid for. 
		/// If recvWindow is not sent, it defaults to 5000.
		/// The value cannot be greater than 60000
		/// </summary>
		INewOrderRequestBuilder SetReciveWindow(long recvWindow);
	}
}