using System;
using System.IO;
using System.Collections.Generic;

namespace BW.Unpacker
{
    class WpkUnpack
    {
        private static List<WpkEntry> m_EntryTable = new List<WpkEntry>();

        private static Byte[] iDecryptResData(Byte[] lpBuffer)
        {
            lpBuffer = AES.iDecryptData(lpBuffer);
            lpBuffer = XOR.iDeobfuscateData(lpBuffer);
            lpBuffer = NEOX.iDecompress(lpBuffer);

            return lpBuffer;
        }

        private static Byte[] iDecryptPythonData(Byte[] lpBuffer)
        {
            lpBuffer = ROTOR.iDecryptData(lpBuffer);

            if (lpBuffer[0] == 0x78 && lpBuffer[1] == 0xDA)
                lpBuffer = ZLIB.iDecompress(lpBuffer, 2);

            lpBuffer = XOR.iReverseData(lpBuffer);
			
            //Python 2.7.x header
            Byte[] lpResult = { 0x03, 0xF3, 0x0D, 0x0A, 0x00, 0x00, 0x00, 0x00 };

            Array.Resize(ref lpResult, lpResult.Length + lpBuffer.Length);
            Array.Copy(lpBuffer, 0, lpResult, 8, lpBuffer.Length);

            return lpBuffer;
        }

        public static void iDoIt(String m_IdxFile, String m_DstFolder)
        {
            using (FileStream TIdxStream = File.OpenRead(m_IdxFile))
            {
                var m_Header = new WpkHeader();

                m_Header.dwMagic = TIdxStream.ReadUInt32();

                if (m_Header.dwMagic != 0x57504B53)
                {
                    throw new Exception("[ERROR]: Invalid first magic of IDX index file!");
                }

                m_Header.dwUnknown = TIdxStream.ReadUInt32();
                m_Header.dwVersion = TIdxStream.ReadInt32();

                if (m_Header.dwVersion != 0 && m_Header.dwVersion != 1)
                {
                    throw new Exception("[ERROR]: Invalid IDX index file!");
                }

                m_Header.dwTotalFiles = TIdxStream.ReadInt32();
                m_Header.lpPadded = TIdxStream.ReadBytes(16);

                m_EntryTable.Clear();
                for (Int32 i = 0; i < m_Header.dwTotalFiles; i++)
                {
                    var m_Entry = new WpkEntry();

                    m_Entry.lpMd5Hash = TIdxStream.ReadBytes(16);
                    m_Entry.dwSize = TIdxStream.ReadInt32();
                    m_Entry.dwOffset = TIdxStream.ReadUInt32();
                    m_Entry.wArchiveNum = TIdxStream.ReadUInt16();
                    m_Entry.wFileIndex = TIdxStream.ReadUInt16();

                    m_EntryTable.Add(m_Entry);
                }

                UInt32 dwCheck = TIdxStream.ReadUInt32();

                if (m_Header.dwUnknown == dwCheck)
                {
                    if (m_Header.dwVersion == 1)
                    {
                        foreach (var m_Entry in m_EntryTable)
                        {
                            m_Entry.dwCrc = TIdxStream.ReadUInt32();
                        }
                    }
                }

                TIdxStream.Dispose();
            }

            foreach (var m_Entry in m_EntryTable)
            {
                String m_LocalPath = Path.GetDirectoryName(m_IdxFile) + @"\" + Path.GetFileNameWithoutExtension(m_IdxFile);

                if (m_LocalPath.StartsWith("\\"))
                {
                    m_LocalPath = Utils.iGetApplicationPath() + m_LocalPath;
                }

                String m_FileName = Utils.iGetStringFromBytes(m_Entry.lpMd5Hash).ToLower();
                String m_FullPath = m_DstFolder + m_FileName;

                if (m_Entry.wArchiveNum == 0xFF)
                {
                    Utils.iSetWarning("[DECRYPTING]: " + m_FileName + " local file");
                    Utils.iCreateDirectory(m_FullPath);

                    if (!File.Exists(m_LocalPath + @"\" + m_FileName))
                    {
                        Utils.iSetError("[ERROR]: Unable to decrypt local file -> " + m_LocalPath + @"\" + m_FileName + " <- does not exist");
                        
                        continue;
                    }

                    var lpBuffer = File.ReadAllBytes(m_LocalPath + @"\" + m_FileName);

                    lpBuffer = iDecryptResData(lpBuffer);

                    if (Path.GetFileNameWithoutExtension(m_IdxFile) == "script")
                    {
                        lpBuffer = iDecryptPythonData(lpBuffer);
                    }

                    File.WriteAllBytes(m_FullPath, lpBuffer);

                    continue;
                }
                else
                {
                    String m_ArchiveName = m_LocalPath + String.Format("{0}.wpk", m_Entry.wArchiveNum);

                    Utils.iSetInfo("[UNPACKING]: " + m_FileName);
                    Utils.iCreateDirectory(m_FullPath);

                    using (FileStream TWpkStream = File.OpenRead(m_ArchiveName))
                    {
                        TWpkStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);

                        var lpBuffer = TWpkStream.ReadBytes(m_Entry.dwSize);

                        //Encrypted resource types (BW uses only AC type)
                        //41 43 01 00 (AC - AES Crypt)
                        //58 43 01 00 (XC - XOR Crypt)

                        UInt32 dwMagic = BitConverter.ToUInt32(lpBuffer, 0);
                        if (dwMagic == 0x14341)
                        {
                            lpBuffer = iDecryptResData(lpBuffer);

                            if (Path.GetFileNameWithoutExtension(m_IdxFile) == "script")
                            {
                                //Resource list (Android - not encrypted) (iOS - ??)
                                if (m_FileName != "b7839921abbc655e02e61967455dae28")
                                {
                                    lpBuffer = iDecryptPythonData(lpBuffer);
                                }
                            }
                        }

                        File.WriteAllBytes(m_FullPath, lpBuffer);

                        TWpkStream.Dispose();
                    }
                }
            }
        }
    }
}
