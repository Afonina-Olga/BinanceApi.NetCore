namespace BinanceApi.NetCore.Requests
{
	public class NewOrderAdvancedOptions
	{
		/// <summary>
		/// An unique id among open orders. Automatically generated if not sent.
		/// </summary>
		public string NewClientOrderId { get; set; }

		/// <summary>
		/// RecvWindow may be sent to specify the number of milliseconds after timestamp the request is valid for. 
		/// If recvWindow is not sent, it defaults to 5000.
		/// The value cannot be greater than 60000
		/// </summary>
		public long ReciveWindow { get; set; }
	}
}
