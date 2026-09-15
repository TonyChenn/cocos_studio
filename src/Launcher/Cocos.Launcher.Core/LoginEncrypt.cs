using System;
using System.Security.Cryptography;
using System.Text;

namespace Cocos.Launcher.Core
{
	public class LoginEncrypt
	{
		public static byte[] Encrypt(byte[] original, byte[] key)
		{
			return new TripleDESCryptoServiceProvider
			{
				Key = LoginEncrypt.MakeMD5(key),
				Mode = CipherMode.ECB
			}.CreateEncryptor().TransformFinalBlock(original, 0, original.Length);
		}

		public static byte[] Decrypt(byte[] encrypted, byte[] key)
		{
			return new TripleDESCryptoServiceProvider
			{
				Key = LoginEncrypt.MakeMD5(key),
				Mode = CipherMode.ECB
			}.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
		}

		public static byte[] MakeMD5(byte[] original)
		{
			MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider();
			return md5CryptoServiceProvider.ComputeHash(original);
		}

		public static string Encrypt(string original, string key)
		{
			byte[] bytes = Encoding.Default.GetBytes(original);
			byte[] bytes2 = Encoding.Default.GetBytes(key);
			return Convert.ToBase64String(LoginEncrypt.Encrypt(bytes, bytes2));
		}

		public static string Decrypt(string encrypted, string key, Encoding encoding)
		{
			byte[] encrypted2 = Convert.FromBase64String(encrypted);
			byte[] bytes = Encoding.Default.GetBytes(key);
			return encoding.GetString(LoginEncrypt.Decrypt(encrypted2, bytes));
		}

		public const string key = "E99F9354-BC29-48A2-9839-F3D0DD83CCE5";
	}
}
