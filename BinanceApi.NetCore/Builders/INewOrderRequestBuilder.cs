using System;

using BinanceApi.NetCore.Requests;

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
		/// Additional options for a request
		/// </summary>
		INewOrderRequestBuilder AdvancedOptions(Action<NewOrderAdvancedOptions> config);
	}
}