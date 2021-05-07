using System;
using Xamarin.Forms;

namespace HXFSimpleHybrid
{
    public delegate bool ShouldTrustCertificate(ICustomCertificate certificate);
    

    public class HWebView : WebView
    {
        Action<string> action;
        public ShouldTrustCertificate ShouldTrustUnknownCertificate { get; set; }
        public string Uri { get; set; }

        public void Invoking(Action<string> callback)
        {
            action = callback;
        }

        public void Cleanup()
        {
            action = null;
        }

        public void InvokeAction(string data)
        {
            if (action == null || data == null)
            {
                return;
            }
            action.Invoke(data);
        }
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
