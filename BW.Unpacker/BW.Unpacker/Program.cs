using System;
using System.IO;

namespace BW.Unpacker
{
    class Program
    {
        private static String m_Title = "Beyond the World Unpacker";

        static void Main(String[] args)
        {
            Console.Title = m_Title;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(m_Title);
            Console.WriteLine("(c) 2025 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    BW.Unpacker <m_IdxFile> <m_Directory>\n");
                Console.WriteLine("    m_IdxFile - Source of IDX file");
                Console.WriteLine("    m_Directory - Destination directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    BW.Unpacker E:\\Games\\BW\\assets\\res\\3d_ui.idx D:\\Unpacked");
                Console.ResetColor();
                return;
            }

            String m_IndexFile = args[0];
            String m_Output = Utils.iCheckArgumentsPath(args[1]);

            if (!File.Exists(m_IndexFile))
            {
                Utils.iSetError("[ERROR]: Input file -> " + m_IndexFile + " <- does not exist");
                return;
            }

            WpkUnpack.iDoIt(m_IndexFile, m_Output);
        }
    }
}
