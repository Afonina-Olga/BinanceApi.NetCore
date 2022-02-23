namespace BinanceApi.NetCore.Domain
{
	/// <summary>
	/// The response JSON for a new order
	/// ACK, RESULT, or FULL; MARKET and LIMIT order types default to FULL, all other orders default to ACK.
	/// </summary>
	public enum NewOrderResponseType
	{
		ASK,
		RESULT,
		FULL
	}
}
