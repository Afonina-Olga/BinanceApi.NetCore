namespace BinanceApi.NetCore.Endpoints
{
	public interface IEndpoint 
	{
		string BaseUrl { get; }

		string Version { get; }
	}
}
