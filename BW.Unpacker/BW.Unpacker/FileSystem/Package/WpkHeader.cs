using System;

namespace BW.Unpacker
{
    class WpkHeader
    {
        public UInt32 dwMagic { get; set; } // SKPW (0x57504B53)
        public UInt32 dwUnknown { get; set; } // ? Same value is also at the end of entry table
        public Int32 dwVersion { get; set; } // 0 (2024), 1 (2025) -> has hash table after entry table
        public Int32 dwTotalFiles { get; set; }
        public Byte[] lpPadded { get; set; } // Always 06 0C 0C 0C 0C 0C 0C 0C 0C 0C 0C 0C 0C 0C 0C 0C (16 bytes) (Padded bytes???)
    }
}
