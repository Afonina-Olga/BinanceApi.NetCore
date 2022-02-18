namespace BinanceApi.NetCore.Domain.Response
{
	public class NewOrderResponseFull : NewOrderResponseAsk
	{
		public string Symbol { get; set; }

		public int OrderId { get; set; }
	}
}
