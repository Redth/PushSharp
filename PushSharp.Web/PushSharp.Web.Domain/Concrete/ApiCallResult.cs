
using PushSharp.Web.Interfaces;

namespace PushSharp.Web.Domain.Concrete
{
	public class ApiCallResult : OperationResult, IApiCallResult
	{
        public ApiCallResult(bool isSuccessful, string message) : base(isSuccessful, message)
        {
        }
    }
}
