using System;
using System.IO;
using System.Reflection;

namespace BW.Unpacker
{
    class Utils
    {
        public static UInt32 iAlignUInt32(UInt32 dwValue, UInt32 dwAlignSize)
        {
            if (dwValue == 0) {
                return dwValue;
            }

            return dwValue + ((dwAlignSize - (dwValue % dwAlignSize)) % dwAlignSize);
        }

        public static String iGetStringFromBytes(Byte[] m_Bytes)
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

        public static String iGetApplicationPath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        public static String iGetApplicationVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public static void iSetInfo(String m_String)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(m_String);
            Console.ResetColor();
        }

        public static void iSetError(String m_String)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(m_String + "!");
            Console.ResetColor();
        }

        public static void iSetWarning(String m_String)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(m_String + "!");
            Console.ResetColor();
        }

        public static String iCheckArgumentsPath(String m_Arg)
        {
            if (m_Arg.EndsWith("\\") == false)
            {
                m_Arg = m_Arg + @"\";
            }
            return m_Arg;
        }

        public static void iCreateDirectory(String m_Directory)
        {
            if (!Directory.Exists(Path.GetDirectoryName(m_Directory)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(m_Directory));
            }
        }
    }
}
