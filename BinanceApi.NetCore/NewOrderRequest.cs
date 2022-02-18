using Ardalis.GuardClauses;

using BinanceApi.NetCore.Builders;
using BinanceApi.NetCore.Domain;

namespace BinanceApi.NetCore
{
	public class NewOrderRequest : INewOrderRequestBuilder
	{
		#region Fields

		private string _symbol;
		private OrderSide? _orderSide;
		private OrderType? _orderType;
		private string _orderId;
		private long _recvWindow;

		#endregion

		public INewOrderRequestBuilder WithSimbol(string name)
		{
			_symbol = Guard.Against.NullOrEmpty(name, nameof(name), "Symbol is null or empty");
			return this;
		}

		public INewOrderRequestBuilder ForBuy()
		{
			_orderSide = OrderSide.BUY;
			return this;
		}

		public INewOrderRequestBuilder ForSell()
		{
			_orderSide = OrderSide.SELL;
			return this;
		}

		public INewOrderRequestBuilder SetNewClientOrderId(string orderId)
		{
			_orderId = Guard.Against.NullOrEmpty(orderId, nameof(orderId), "OrderId is null or empty"); ;
			return this;
		}

		public INewOrderRequestBuilder SetReciveWindow(long recvWindow)
		{
			_recvWindow = Guard.Against.OutOfRange(recvWindow, nameof(recvWindow), 5000, 60000, "Receive window must be between 5000 and 60 000");
			return this;
		}

		public TOrderType SetOrderType<TOrderType>() 
			where TOrderType : INewOrderTypeBaseRequestBuilder, new()
		{
			return new TOrderType();
		}
	}
}
