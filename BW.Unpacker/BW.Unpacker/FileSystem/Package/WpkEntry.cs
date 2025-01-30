using System;

namespace BW.Unpacker
{
    class WpkEntry
    {
        public Byte[] lpMd5Hash { get; set; } // MD5 of decrypted and decompressed file data
        public Int32 dwSize { get; set; }
        public UInt32 dwOffset { get; set; }
        public UInt16 wArchiveNum { get; set; } // if -1 (0xFF) = file on disk
        public UInt16 wFileIndex { get; set; } // for thd\*.thx ?
        public UInt32 dwCrc { get; set; } // hash of ???? only for version 1 (after entry table) - unused at this time
    }
}
