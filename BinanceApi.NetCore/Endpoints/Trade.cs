using BinanceApi.NetCore.Builders;
using BinanceApi.NetCore.Requests;

namespace BinanceApi.NetCore.Endpoints
{
	/// <summary>
	/// Spot account / trade endpoints
	/// </summary>
	/// <see href="https://binance-docs.github.io/apidocs/spot/en/#spot-account-trade"/>
	public class Trade : IEndpoint
	{
		public string BaseUrl => "https://api.binance.com/api";

		public string Version => "v3";

		/// <summary>
		/// Test new order creation and signature/recvWindow long.
		/// Creates and validates a new order but does not send it into the matching engine.
		/// </summary
		/// <see href="https://binance-docs.github.io/apidocs/spot/en/#test-new-order-trade"/>
		public INewOrderRequestBuilder CreateNewOrderTest() => new NewOrderRequest();

		/// <summary>
		/// Check an order's status.
		/// </summary>
		/// <see href="https://binance-docs.github.io/apidocs/spot/en/#query-order-user_data"/>
		public void GetOrder() { }

		/// <summary>
		/// Send in a new order.
		/// </summary>
		/// <see href="https://binance-docs.github.io/apidocs/spot/en/#new-order-trade"/>
		public INewOrderRequestBuilder CreateNewOrder() => new NewOrderRequest();

		// Метод с параметрами?
		//public IExecutable CreateNewOrder(string param1, int param2) => new NewOrderRequest();

		/// <summary>
		/// Cancel an active order.
		/// </summary>
		/// <see href="https://binance-docs.github.io/apidocs/spot/en/#cancel-order-trade"/>
		public void CancelOrder() { }

		/// <summary>
		/// Get all open orders on a symbol. Careful when accessing this with no symbol.
		/// </summary>
		/// <see href="https://binance-docs.github.io/apidocs/spot/en/#current-open-orders-user_data"/>
		public void GetOpenOrders() { }
	}
}
