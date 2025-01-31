using System;

namespace BW.Unpacker
{
    class XOR
    {
        public static Byte[] iDeobfuscateData(Byte[] lpBuffer)
        {
            Int32 j = 0;
            Int32 dwBlockSize = 64;

            if (lpBuffer.Length < dwBlockSize)
                dwBlockSize = lpBuffer.Length;

            Byte[] lpTemp = new Byte[dwBlockSize];

            for (Int32 i = dwBlockSize; i > 0; i--, j++)
            {
                lpTemp[j] = (Byte)(lpBuffer[i - 1] ^ 0x5A);
            }

            Array.Copy(lpTemp, 0, lpBuffer, 0, lpTemp.Length);

            return lpBuffer;
        }

        //For python scripts only
        public static Byte[] iReverseData(Byte[] lpBuffer)
        {
            Int32 j = lpBuffer.Length - 1;

            for (Int32 i = 0; i < lpBuffer.Length / 2; i++, j--)
            {
                Byte bTemp1 = lpBuffer[i];
                Byte bTemp2 = lpBuffer[j];

                lpBuffer[i] = bTemp2;
                lpBuffer[j] = bTemp1;
            }

            return lpBuffer;
        }
    }
}
