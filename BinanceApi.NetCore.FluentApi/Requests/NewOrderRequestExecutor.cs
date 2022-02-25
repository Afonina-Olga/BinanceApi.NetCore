using System;
using Ardalis.GuardClauses;

using BinanceApi.NetCore.FluentApi.Builders;
using BinanceApi.NetCore.Domain;
using BinanceApi.NetCore.Domain.Response;
using System.Net.Http;

namespace BinanceApi.NetCore.FluentApi.Requests
{
	public class NewOrderRequestExecutor :
		INewOrderRequest,
		INewOrderMarketRequest,
		IExecutable<NewOrderResponseAsk>
	{
		#region Fields

		private readonly BinanceRequestExecutor _executor;
		private string _symbol;
		private OrderSide? _orderSide;
		private OrderType? _orderType;
		private NewOrderResponseType _newOrderResponseType = NewOrderResponseType.ASK;
		private string _orderId;
		private long _recvWindow;
		private decimal _quantity;
		private decimal _quoteOrderQty;

		#endregion

		public NewOrderRequestExecutor(BinanceRequestExecutor executor)
		{
			_executor = executor;
		}

		public INewOrderRequest WithSimbol(string name)
		{
			_symbol = Guard.Against.NullOrEmpty(name, nameof(name), "Symbol is null or empty");
			return this;
		}

		public INewOrderRequest ForBuy()
		{
			_orderSide = OrderSide.BUY;
			return this;
		}

		public INewOrderRequest ForSell()
		{
			_orderSide = OrderSide.SELL;
			return this;
		}

		public INewOrderMarketRequest SetMarketOrderType()
		{
			_orderType = OrderType.MARKET;
			return this;
		}

		public INewOrderRequest AdvancedOptions(Action<NewOrderAdvancedOptions> config)
		{
			var configuration = new NewOrderAdvancedOptions();
			config?.Invoke(configuration);
			_recvWindow = Guard.Against.OutOfRange(configuration.ReciveWindow, nameof(configuration.ReciveWindow), 5000, 60000, "Receive window must be between 5000 and 60 000");
			_orderId = Guard.Against.NullOrEmpty(configuration.NewClientOrderId, nameof(configuration.NewClientOrderId), "OrderId is null or empty");
			return this;
		}

		public IExecutable<NewOrderResponseAsk> WithQuoteOrderQty(decimal quoteOrderQty)
		{
			_quoteOrderQty = Guard.Against.NegativeOrZero(
				quoteOrderQty,
				nameof(quoteOrderQty),
				"QuoteOrderQty must be grather than zero");
			return this;
		}

		public IExecutable<NewOrderResponseAsk> WithQuantity(decimal quantity)
		{
			_quantity = Guard.Against.NegativeOrZero(quantity, nameof(quantity), "Quantity must be more than zero");
			return this;
		}

		// Вернуть экземпляр переданный
		// Можно вообще в интерфейс не выносить (возможно)
		public TResponse ExecuteAsync<TResponse>() where TResponse : new()
		{
			return new TResponse();
		}

		// Вернуть в зависимости от типа response
		public NewOrderResponseAsk ExecuteAsync()
		{
			return new NewOrderResponseAsk();
		}
	}
}
