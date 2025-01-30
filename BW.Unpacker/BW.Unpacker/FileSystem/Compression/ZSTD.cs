using System;
using System.IO;
using System.IO.Compression;

using Zstandard.Net;

namespace BW.Unpacker
{
    class ZSTD
    {
        public static Byte[] iDecompress(Byte[] lpSrcBuffer, Int32 dwOffset = 4)
        {
            Byte[] lpDstBuffer;
            using (MemoryStream TSrcStream = new MemoryStream(lpSrcBuffer) { Position = dwOffset })
            {
                using (var TZstandardStream = new ZstandardStream(TSrcStream, CompressionMode.Decompress))
                using (var TDstStream = new MemoryStream())
                {
                    TZstandardStream.CopyTo(TDstStream);
                    lpDstBuffer = TDstStream.ToArray();
                }
            }
            return lpDstBuffer;
        }
    }
}
