namespace AD.Shared.Cryptography
{
	public interface IEncryptionService
	{
		string Encrypt(string value);
		string Decrypt(string encryptedValue);
		string EncryptionKey { get; set; }
	}
}
