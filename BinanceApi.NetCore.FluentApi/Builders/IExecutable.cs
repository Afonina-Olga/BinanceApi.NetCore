namespace BinanceApi.NetCore.FluentApi.Builders
{
	public interface IExecutable<TResponse> where TResponse : class, new()
	{
		/// <summary>
		/// Execute post request
		/// </summary>
		TResponse ExecuteAsync();
	}
}
