using Newtonsoft.Json;

namespace BinanceApi.NetCore.FluentApi.Exceptions
{
	public class BinanceError
	{
		public int Code { get; set; }

		[JsonProperty(PropertyName = "msg")]
		public string Message { get; set; }

		public override string ToString()
		{
			return $"{Code}: {Message}";
		}

		public BinanceError(int code, string message)
		{
			Code = code;
			Message = message;
		}
	}
}
