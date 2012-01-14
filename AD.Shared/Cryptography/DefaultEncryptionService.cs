using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AD.Shared.Cryptography
{
    public class DefaultEncryptionService : IEncryptionService
    {
        private readonly SymmetricAlgorithm _cryptoProvider;
        private byte[] _encryptionKeyBytes;

        public DefaultEncryptionService()
        {
            _cryptoProvider = new AesCryptoServiceProvider();
            EncryptionKey = "379BFF6A4B754ca8";
        }

        public string EncryptionKey { get; set; }

        public string Encrypt(string valueToEncrypt)
        {
            var bytes = GetEncryptionKeyBytes();
            using (var memoryStream = new MemoryStream())
            {
                ICryptoTransform encryptor = _cryptoProvider.CreateEncryptor(bytes, bytes);
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (var writer = new StreamWriter(cryptoStream))
                    {
                        writer.Write(valueToEncrypt);
                        writer.Flush();
                        cryptoStream.FlushFinalBlock();
                        return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
                    }
                }
            }
        }

        private byte[] GetEncryptionKeyBytes()
        {
            return _encryptionKeyBytes ?? (_encryptionKeyBytes = Encoding.ASCII.GetBytes(EncryptionKey));
        }

        public string Decrypt(string encryptedValue)
        {
            var bytes = GetEncryptionKeyBytes();
            using (var memoryStream = new MemoryStream(Convert.FromBase64String(encryptedValue)))
            {
                ICryptoTransform decryptor = _cryptoProvider.CreateDecryptor(bytes, bytes);
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (var reader = new StreamReader(cryptoStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }
    }
}

