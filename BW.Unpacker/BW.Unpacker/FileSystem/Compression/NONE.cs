using System;

namespace BW.Unpacker
{
    class NONE
    {
        public static Byte[] iDecompress(Byte[] lpBuffer, Int32 dwOffset = 12)
        {
            Byte[] lpTemp = new Byte[lpBuffer.Length - 4];

            Array.Copy(lpBuffer, 4, lpTemp, 0, lpTemp.Length);

            return lpTemp;
        }
    }
}
