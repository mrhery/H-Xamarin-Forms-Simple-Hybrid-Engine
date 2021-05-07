using System;
using System.Text;
using Android.Net.Http;
using Java.Security;
using Java.Security.Cert;
using System.IO;

namespace HXFSimpleHybrid.Droid
{
    public class Certificate : HXFSimpleHybrid.ICustomCertificate
    {
        private const int SubjectPublicKeyInfoHeaderLength = 24;

        private readonly string _host;
        private readonly byte[] _hashBytes;
        private readonly byte[] _publicKeyBytes;

        private readonly Lazy<string> _hashString;
        private readonly Lazy<string> _publicKeyString;

        public Certificate(string host, SslCertificate certificate)
        {
            _host = host;

            var bundle = SslCertificate.SaveState(certificate);
            var bytes = bundle.GetByteArray("x509-certificate");
            var factory = CertificateFactory.GetInstance("X.509");
            var x509Certificate = factory.GenerateCertificate(new MemoryStream(bytes));
            var messageDigest = MessageDigest.GetInstance("SHA-1");
            messageDigest.Update(x509Certificate.GetEncoded());
            _hashBytes = messageDigest.Digest();

            var encodedBytes = x509Certificate.PublicKey.GetEncoded();

            var publicKeyLength = encodedBytes.Length - SubjectPublicKeyInfoHeaderLength;
            var publicKeyBytes = new byte[publicKeyLength];
            Array.Copy(encodedBytes, SubjectPublicKeyInfoHeaderLength, publicKeyBytes, 0, publicKeyLength);

            _publicKeyBytes = publicKeyBytes;

            _hashString = new Lazy<string>(() => ByteArrayToHexString(_hashBytes));
            _publicKeyString = new Lazy<string>(() => ByteArrayToHexString(_publicKeyBytes));
        }

        #region ICertificate implementation

        public string Host { get { return _host; } }

        public byte[] Hash { get { return _hashBytes; } }

        public string HashString { get { return _hashString.Value; } }

        public byte[] PublicKey { get { return _publicKeyBytes; } }

        public string PublicKeyString { get { return _publicKeyString.Value; } }

        #endregion

        private static string ByteArrayToHexString(byte[] bytes)
        {
            if (bytes == null)
            {
                return null;
            }

            var stringBuilder = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes)
            {
                stringBuilder.Append(b.ToString("X2"));
            }
            return stringBuilder.ToString();
        }
    }
}