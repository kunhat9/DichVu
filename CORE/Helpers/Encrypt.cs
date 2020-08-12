using System;
using System.Security.Cryptography;
using System.Text;

namespace CORE.Helpers
{
    public static class Encrypt
    {
        public static string GetMD5Hash(string rawString)
        {
            UnicodeEncoding encode = new UnicodeEncoding();
            //Chuyển kiểu chuổi thành kiểu byte
            Byte[] passwordBytes = encode.GetBytes(rawString);
            //mã hóa chuỗi đã chuyển
            Byte[] hash;
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            hash = md5.ComputeHash(passwordBytes);
            //tạo đối tượng StringBuilder (làm việc với kiểu dữ liệu lớn)
            StringBuilder sb = new StringBuilder();
            foreach (byte outputByte in hash)
            {
                //sb.Append(outputByte.ToString("x2").ToUpper());
                sb.Append(outputByte.ToString("0").ToUpper());
                //nếu bạn muốn các chữ cái in thường thay vì in hoa thì bạn thay chữ "X" in hoa 
                //trong "X2" thành "x"
            }
            return sb.ToString();
        }

        public static string GetMd5Hash2(string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}