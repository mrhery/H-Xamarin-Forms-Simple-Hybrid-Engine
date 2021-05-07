using Xamarin.Forms;

namespace HXFSimpleHybrid
{
    public delegate bool ShouldTrustCertificate(ICustomCertificate certificate);

    public class HWebView : WebView
    {
        public ShouldTrustCertificate ShouldTrustUnknownCertificate { get; set; }
        public string Uri { get; set; }
    }

    public interface ICustomCertificate
    {
        string Host { get; }
        byte[] Hash { get; }
        string HashString { get; }
        byte[] PublicKey { get; }
        string PublicKeyString { get; }
    }
}
