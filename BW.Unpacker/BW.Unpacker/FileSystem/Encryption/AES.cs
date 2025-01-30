using System;
using System.IO;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace BW.Unpacker
{
    class AES
    {
        [DllImport("libxor.dll", EntryPoint = "iXorDecrypt", CallingConvention = CallingConvention.Cdecl)]
        private static extern void iXorDecrypt(Byte[] lpBuffer, UInt32 dwDataSize, UInt32 dwActualSize, UInt32 dwBlockSize, Byte[] lpKey);

        public static Byte[] iGenerateKey(Byte[] lpBuffer)
        {
            Int32 dwDataSize = lpBuffer.Length - 8;

            Byte[] lpKey = new Byte[16];

            lpKey[0] = (Byte)(dwDataSize % 0xFD);
            lpKey[1] = (Byte)(lpBuffer[3] + dwDataSize);
            lpKey[2] = (Byte)(dwDataSize >> 8);
            lpKey[3] = (Byte)(dwDataSize >> 16);
            lpKey[4] = 0x6A;
            lpKey[5] = 0x6B;
            lpKey[6] = 0x2E;
            lpKey[7] = 0x7C;
            lpKey[8] = 0x30;
            lpKey[9] = 0x36;
            lpKey[10] = (Byte)(lpKey[1] ^ 0x33);
            lpKey[11] = (Byte)(lpKey[1] | 0x2E);
            lpKey[12] = 0x6E;
            lpKey[13] = 0x65;
            lpKey[14] = 0x74;
            lpKey[15] = 0x5C;

            return lpKey;
        }

        public static Byte[] iDecryptData(Byte[] lpBuffer)
        {
            var lpKey = iGenerateKey(lpBuffer);

            UInt32 dwDataSize = (UInt32)lpBuffer.Length - 8;
            UInt32 dwBlockSize = (UInt32)128 << (lpBuffer[2] - 1);

            if (dwBlockSize > dwDataSize)
                dwBlockSize = dwDataSize;

            UInt32 dwActualSize = dwBlockSize & 0xFFFFFFF0;

            Byte[] lpResult = new Byte[lpBuffer.Length - 8];
            Array.Copy(lpBuffer, 8, lpResult, 0, lpResult.Length);

            if (dwActualSize != 0)
            {
                RijndaelManaged TAES = new RijndaelManaged();

                TAES.KeySize = 128;
                TAES.BlockSize = 128;
                TAES.Key = lpKey;
                TAES.IV = new Byte[16];
                TAES.Mode = CipherMode.ECB;
                TAES.Padding = PaddingMode.None;

                Int32 dwBlockOffset = 0;
                Int32 dwBlockCount = (Int32)dwActualSize / 16;

                for (Int32 i = 0; i < dwBlockCount; i++)
                {
                    ICryptoTransform TICryptoTransform = TAES.CreateDecryptor();
                    TICryptoTransform.TransformBlock(lpResult, dwBlockOffset, 16, lpResult, dwBlockOffset);
                    dwBlockOffset += 16;
                    TICryptoTransform.Dispose();
                }

                TAES.Dispose();
            }

            iXorDecrypt(lpResult, dwDataSize, dwActualSize, dwBlockSize, lpKey);

            return lpResult;
        }
    }
}
