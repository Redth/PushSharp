namespace PushSharp.Web.Domain.Concrete
{
	public class OperationResult
	{
		public OperationResult()
		{			
		}

	    public OperationResult(bool successful, string message)
	    {
	        IsSuccessful = successful;
	        Message = message;
	    }

	    public bool IsSuccessful { get; set; }
		public string Message { get; set; }
	}
}
