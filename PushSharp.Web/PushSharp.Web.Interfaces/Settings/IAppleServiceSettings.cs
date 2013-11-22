namespace PushSharp.Web.Interfaces.Settings
{
    public interface IAppleServiceSettings : IServiceSettings
    {
        byte[] CertificateContents { get; }
        string CertificatePassword { get; }
    }
}