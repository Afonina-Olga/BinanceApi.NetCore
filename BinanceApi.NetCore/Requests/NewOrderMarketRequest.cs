using Ardalis.GuardClauses;

using BinanceApi.NetCore.Builders;
using BinanceApi.NetCore.Domain.Response;

namespace BinanceApi.NetCore.Requests
{
	public class NewOrderMarketRequest :
		NewOrderRequest,
		INewOrderTypeBaseRequestBuilder,
		INewOrderMarketRequestBuilder,
		IExecutable
	{
		#region Fields

		private decimal _quantity;
		private decimal _quoteOrderQty;

		#endregion

		public IExecutable WithQuantity(decimal quantity)
		{
			_quantity = Guard.Against.NegativeOrZero(quantity, nameof(quantity), "Quantity must be more than zero");
			return this;
		}

		public IExecutable WithQuoteOrderQty(decimal quoteOrderQty)
		{
			_quoteOrderQty = Guard.Against.NegativeOrZero(quoteOrderQty, nameof(quoteOrderQty), "QuoteOrderQty must be grather than zero");
			return this;
		}

		public void ExecuteAsync<TResponse>() where TResponse : NewOrderResponseAsk
		{
			throw new System.NotImplementedException();
		}

		public void ExecuteAsync()
		{
			throw new System.NotImplementedException();
		}
	}
}
