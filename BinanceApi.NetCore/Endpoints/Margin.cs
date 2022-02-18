using System;

namespace BinanceApi.NetCore.Endpoints
{
	public class Margin : IEndpoint
	{
		public string BaseUrl => throw new NotImplementedException();

		public string Version => "v1";
	}
}
