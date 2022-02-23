using BinanceApi.NetCore.Domain.Response;

namespace BinanceApi.NetCore.FluentApi.Builders
{
	/// <summary>
	/// Base interface for all order type builders
	/// </summary>
	public interface INewOrderTypeBaseRequest
	{
		/// <summary>
		/// Set quantity
		/// </summary>
		IExecutable<NewOrderResponseAsk> WithQuantity(decimal quantity);
	}
}
