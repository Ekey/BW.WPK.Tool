using System;
using System.Text;
using System.Security.Cryptography;

namespace BW.Unpacker
{
    class MD5
    {
        private static String iGetStringFromBytes(Byte[] m_Bytes)
        {
            Char[] lpBytesHex = new Char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            Char[] lpHexString = new Char[m_Bytes.Length * 2];
            Int32 dwIndex = 0;

            foreach (Byte bByte in m_Bytes)
            {
                lpHexString[dwIndex++] = lpBytesHex[bByte >> 4];
                lpHexString[dwIndex++] = lpBytesHex[bByte & 0x0F];
            }

            return new String(lpHexString);
        }

        public static Byte[] iGetHashBuffer(Byte[] lpBuffer)
        {
            MD5CryptoServiceProvider TMD5 = new MD5CryptoServiceProvider();

            var lpHash = TMD5.ComputeHash(lpBuffer);

            return lpHash;
        }

        public static String iGetHash(String m_String, Encoding m_Encoding)
        {
            MD5CryptoServiceProvider TMD5 = new MD5CryptoServiceProvider();

            var lpBuffer = m_Encoding.GetBytes(m_String);
            var lpHash = TMD5.ComputeHash(lpBuffer);

            return iGetStringFromBytes(lpHash).ToLower();
        }
    }
}
