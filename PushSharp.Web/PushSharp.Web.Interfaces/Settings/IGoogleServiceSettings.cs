namespace PushSharp.Web.Interfaces.Settings
{
    public interface IGoogleServiceSettings : IServiceSettings
    {
        string GoogleApiAccessKey { get; set; }
    }
}