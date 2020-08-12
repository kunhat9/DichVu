using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WebClient.Helpers
{
    public class EncryptHelper
    {
        #region Private

        private static void CheckEncryptKey(string encryptKey)
        {
            if (string.IsNullOrEmpty(encryptKey) || encryptKey.Length < 10)
            {
                throw new Exception("Encrypt key Should has a minimum length of 10");
            }
        }

        #endregion

        #region Exposed

        /// <summary>
        /// Encrypt input text base on encrypt key.
        /// </summary>
        /// <param name="strText">Input text</param>
        /// <param name="encryptKey">Input encrypt key. Should has a minimum length of 10.</param>
        /// <returns>Encrypted text</returns>
        public static string Encrypt(string strText, string encryptKey)
        {
            CheckEncryptKey(encryptKey);

            try
            {
                Byte[] Key = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                Byte[] IV = Encoding.UTF8.GetBytes(StringHelper.Reverse(encryptKey).Substring(1, 8));

                Byte[] inputByteArray = Encoding.UTF8.GetBytes(strText);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(Key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                return Convert.ToBase64String(ms.ToArray());
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Decrypt input text base on encrypt key.
        /// </summary>
        /// <param name="strText">Input text</param>
        /// <param name="encryptKey">Input decrypt key. Should has a minimum length of 10.</param>
        /// <returns>Decrypted text</returns>
        public static string Decrypt(string strText, string encryptKey)
        {
            CheckEncryptKey(encryptKey);

            try
            {
                Byte[] Key = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                Byte[] IV = Encoding.UTF8.GetBytes(StringHelper.Reverse(encryptKey).Substring(1, 8));

                Byte[] inputByteArray = Convert.FromBase64String(strText);

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(Key, IV), CryptoStreamMode.Write);

                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion

        public static string EncryptBase64(string encodeString)
        {
            try
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(encodeString);
                return System.Convert.ToBase64String(plainTextBytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string DecryptBase64(string decodestring)
        {
            try
            {
                var base64EncodedBytes = System.Convert.FromBase64String(decodestring);
                return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string GetMD5Hash(string rawString)
        {
            UnicodeEncoding encode = new UnicodeEncoding();
            Byte[] passwordBytes = encode.GetBytes(rawString);
            Byte[] hash;
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            hash = md5.ComputeHash(passwordBytes);

            StringBuilder sb = new StringBuilder();
            foreach (byte outputByte in hash)
            {
                sb.Append(outputByte.ToString("x2").ToUpper());
            }

            return sb.ToString();
        }
    }
}