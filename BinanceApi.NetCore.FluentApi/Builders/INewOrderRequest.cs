using System;

using BinanceApi.NetCore.FluentApi.Requests;

namespace BinanceApi.NetCore.FluentApi.Builders
{
	public interface INewOrderRequest
	{
		/// <summary>
		/// Set a symbol
		/// </summary>
		INewOrderRequest WithSimbol(string name);

		/// <summary>
		/// Set OrderSide BUY
		/// </summary>
		INewOrderRequest ForBuy();

		/// <summary>
		/// Set OrderSide SELL
		/// </summary>
		INewOrderRequest ForSell();

		/// <summary>
		/// Additional options for a request
		/// </summary>
		INewOrderRequest AdvancedOptions(Action<NewOrderAdvancedOptions> config);

		/// <summary>
		/// Set order type == MARKET
		/// </summary>
		INewOrderMarketRequest SetMarketOrderType();
	}
}