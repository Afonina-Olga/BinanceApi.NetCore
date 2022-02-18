using Ardalis.GuardClauses;

using BinanceApi.NetCore.Builders;
using BinanceApi.NetCore.Domain;
using BinanceApi.NetCore.Domain.Response;

namespace BinanceApi.NetCore
{
	public class MarketOrderType : INewOrderTypeBaseRequestBuilder, INewOrderMarketTypeRequestBuilder
	{
		#region Fields

		private decimal _quantity;
		private decimal _quoteOrderQty;
		private TimeInForce _timeInForce;
		private decimal _icebergQty;

		#endregion

		public INewOrderMarketTypeRequestBuilder SetQuantity(decimal quantity)
		{
			_quantity = Guard.Against.NegativeOrZero(quantity, nameof(quantity), "Quantity must be more than zero");
			return this;
		}

		public INewOrderMarketTypeRequestBuilder SetQuoteOrderQty(decimal quoteOrderQty)
		{
			_quoteOrderQty = Guard.Against.NegativeOrZero(quoteOrderQty, nameof(quoteOrderQty), "QuoteOrderQty must be grather than zero");
			return this;
		}

		public INewOrderMarketTypeRequestBuilder SetIcebergQty(decimal icebergQty)
		{
			_icebergQty = Guard.Against.NegativeOrZero(icebergQty, nameof(icebergQty), "IcebergQty must be grather than zero");
			_timeInForce = TimeInForce.GTC;
			return this;
		}

		public INewOrderMarketTypeRequestBuilder ExecuteAsync<TResponse>()
			where TResponse : NewOrderResponseAsk
		{
			// Проверка если установлены оба значения qty 
			throw new System.NotImplementedException();
		}
	}
}
