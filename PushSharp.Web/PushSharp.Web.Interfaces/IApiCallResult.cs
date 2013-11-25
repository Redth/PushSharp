namespace PushSharp.Web.Interfaces
{
	public interface IApiCallResult
	{
		bool IsSuccessful { get; set; }
		string Message { get; set; }
	}
}