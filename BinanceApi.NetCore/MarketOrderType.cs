using Ardalis.GuardClauses;

using BinanceApi.NetCore.Builders;
using BinanceApi.NetCore.Domain;

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

		public void SetQuoteOrderQty(decimal quoteOrderQty)
		{
			_quoteOrderQty = Guard.Against.NegativeOrZero(quoteOrderQty, nameof(quoteOrderQty), "QuoteOrderQty must be grather than zero");
		}

		public void SetIcebergQty(decimal icebergQty)
		{
			_icebergQty = Guard.Against.NegativeOrZero(icebergQty, nameof(icebergQty), "IcebergQty must be grather than zero");
			_timeInForce = TimeInForce.GTC;
		}
	}
}
