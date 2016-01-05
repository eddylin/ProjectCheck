using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ProjectCheck
{
    class Program
    {
        static void Main(string[] args)
        {
            CheckCsprites();
        }

        public static void CheckCsprites()
        {
            DirectoryInfo dir = new DirectoryInfo(@"E:\csprite");
            var files = dir.GetFiles("*.csprite", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                CSprite csprite = new CSprite(file);
                csprite.ExportImages();
            }

            Console.WriteLine(files.Length);

            Console.ReadLine();
        }

        public static void CheckAnimaImages()
        {
            string path = @"E:\x1\trunk\src\mobile\scripts";
            DirectoryInfo dir = new DirectoryInfo(path);

            var files = dir.GetFiles("*.lua", SearchOption.AllDirectories);

            string pattern = @"GetUISpriteRes\( *[""'](.*)_x1[""'] *, *[""'](.*)[""']";

            foreach (var file in files)
            {
                StreamReader reader = new StreamReader(file.OpenRead());
                int line_num = 0;
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    line_num += 1;
                    if (Regex.IsMatch(line, pattern, RegexOptions.Singleline))
                    {
                        Console.WriteLine("{ 0} \t{1}", line_num, line);
                    }
                }
                reader.Close();
                reader.Dispose();

                Console.ReadLine();
            }
        }
    }
}
