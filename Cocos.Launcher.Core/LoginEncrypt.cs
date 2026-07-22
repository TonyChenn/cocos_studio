using System;
using System.Security.Cryptography;
using System.Text;

namespace Cocos.Launcher.Core
{
	// Token: 0x02000054 RID: 84
	public class LoginEncrypt
	{
		// Token: 0x060002C5 RID: 709 RVA: 0x0000B0FC File Offset: 0x000092FC
		public static byte[] Encrypt(byte[] original, byte[] key)
		{
			return new TripleDESCryptoServiceProvider
			{
				Key = LoginEncrypt.MakeMD5(key),
				Mode = CipherMode.ECB
			}.CreateEncryptor().TransformFinalBlock(original, 0, original.Length);
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000B134 File Offset: 0x00009334
		public static byte[] Decrypt(byte[] encrypted, byte[] key)
		{
			return new TripleDESCryptoServiceProvider
			{
				Key = LoginEncrypt.MakeMD5(key),
				Mode = CipherMode.ECB
			}.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000B16C File Offset: 0x0000936C
		public static byte[] MakeMD5(byte[] original)
		{
			MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider();
			return md5CryptoServiceProvider.ComputeHash(original);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000B18C File Offset: 0x0000938C
		public static string Encrypt(string original, string key)
		{
			byte[] bytes = Encoding.Default.GetBytes(original);
			byte[] bytes2 = Encoding.Default.GetBytes(key);
			return Convert.ToBase64String(LoginEncrypt.Encrypt(bytes, bytes2));
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000B1C0 File Offset: 0x000093C0
		public static string Decrypt(string encrypted, string key, Encoding encoding)
		{
			byte[] encrypted2 = Convert.FromBase64String(encrypted);
			byte[] bytes = Encoding.Default.GetBytes(key);
			return encoding.GetString(LoginEncrypt.Decrypt(encrypted2, bytes));
		}

		// Token: 0x0400010A RID: 266
		public const string key = "E99F9354-BC29-48A2-9839-F3D0DD83CCE5";
	}
}
