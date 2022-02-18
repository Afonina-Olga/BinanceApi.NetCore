namespace BinanceApi.NetCore.Domain
{
	public enum TimeInForce
	{
		/// <summary>
		/// Good till cancelled
		/// </summary>
		GTC,

		/// <summary>
		/// Fill or Kill
		/// </summary>
		FOK,

		/// <summary>
		/// Immediate or Cancel
		/// </summary>
		IOC
	}
}
