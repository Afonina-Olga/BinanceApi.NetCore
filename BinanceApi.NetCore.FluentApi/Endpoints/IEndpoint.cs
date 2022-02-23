namespace BinanceApi.NetCore.FluentApi.Endpoints
{
	public interface IEndpoint 
	{
		string BaseUrl { get; }

		string Version { get; }
	}
}
