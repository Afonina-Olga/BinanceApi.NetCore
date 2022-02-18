namespace BinanceApi.NetCore.Builders
{
	/// <summary>
	/// Base interface for all order type builders
	/// </summary>
	public interface INewOrderTypeBaseRequestBuilder
	{
		/// <summary>
		/// Set quantity
		/// </summary>
		INewOrderMarketTypeRequestBuilder SetQuantity(decimal quantity);
	}
}
