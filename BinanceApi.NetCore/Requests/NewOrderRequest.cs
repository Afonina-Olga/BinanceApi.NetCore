using System;
using Ardalis.GuardClauses;

using BinanceApi.NetCore.Builders;
using BinanceApi.NetCore.Domain;

namespace BinanceApi.NetCore.Requests
{
	public class NewOrderRequest : INewOrderRequestBuilder
	{
		#region Fields

		protected string _symbol;
		protected OrderSide? _orderSide;
		protected OrderType? _orderType;
		protected string _orderId;
		protected long _recvWindow;

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

		public TOrderType SetOrderType<TOrderType>() 
			where TOrderType : INewOrderTypeBaseRequestBuilder, new()
		{
			return new TOrderType();
		}

		public INewOrderRequestBuilder AdvancedOptions(Action<NewOrderAdvancedOptions> config)
		{
			var configuration = new NewOrderAdvancedOptions();
			config?.Invoke(configuration);
			_recvWindow = Guard.Against.OutOfRange(configuration.ReciveWindow, nameof(configuration.ReciveWindow), 5000, 60000, "Receive window must be between 5000 and 60 000");
			_orderId = Guard.Against.NullOrEmpty(configuration.NewClientOrderId, nameof(configuration.NewClientOrderId), "OrderId is null or empty");
			return this;
		}
	}
}
