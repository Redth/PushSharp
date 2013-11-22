namespace PushSharp.Web.Domain.Concrete
{
    public class ApiPayLoadBase
    {
    	public ApiPayLoadBase()
    	{    		
    	}

        public ApiPayLoadBase(string key)
        {
            AuthenticationKey = key;
        }

        public string AuthenticationKey { get; set; }
    }
}