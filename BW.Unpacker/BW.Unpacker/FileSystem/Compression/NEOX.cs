using System;

namespace BW.Unpacker
{
    class NEOX
    {
        public static Byte[] iDecompress(Byte[] lpBuffer, Int32 dwOffset = 0)
        {
            UInt32 dwMagic = BitConverter.ToUInt32(lpBuffer, dwOffset);

            switch (dwMagic)
            {
                case 0x5A4C4941: lpBuffer = ZLIB.iDecompress(lpBuffer); break; // AILZ (ZLIA)
                case 0x5A4C4942: lpBuffer = ZLIB.iDecompress(lpBuffer); break; // BILZ (ZLIB)
                case 0x4C5A3446: lpBuffer = LZ4F.iDecompress(lpBuffer); break; // F4ZL (FLZ4)
                case 0x5A535444: lpBuffer = ZSTD.iDecompress(lpBuffer); break; // DTSZ (ZSTD)
                case 0x4E4F4E45: lpBuffer = NONE.iDecompress(lpBuffer); break; // ENON (NONE)
            }

            return lpBuffer;
        }
    }
}
