using System;
using System.Runtime.InteropServices;

namespace BW.Unpacker
{
    class ROTOR
    {
        [DllImport("librotor.dll", EntryPoint = "iRtrDecrypt", CallingConvention = CallingConvention.Cdecl)]
        private static extern void iRtrDecrypt(Byte[] lpBuffer, Int32 dwDataSize);

        public static Byte[] iDecryptData(Byte[] lpBuffer)
        {
            iRtrDecrypt(lpBuffer, lpBuffer.Length);

            return lpBuffer;
        }
    }
}
