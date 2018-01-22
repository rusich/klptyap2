using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            string input_file;
            string file_content = "";

            if (args.Length == 0)
                input_file = "input.txt";
            else
                input_file = args[0];
            try
            {
                file_content = File.ReadAllText(input_file);
                Regex r = new Regex(
                @"\s*class\s+                 
                (?<name>[a-zA-Z_]\w*)\s*<\s*
                (?=                    
                    (?>                
                    (?<param>[a-zA-Z_]\w*)+\s*         
                    (
                    (\s*,\s*(?<param>[a-zA-Z_]\w*))+\s*       
                    |                 
                    \s*<\s*(?<param>[a-zA-Z_]\w*)+\s*  (?<DEPTH>)  
                    | 
                    \s*>\s* (?<-DEPTH>)
                    )*
                    )*                 
                    (?(DEPTH)(?!))     
                    \s*>\s*$                  
                )",
    RegexOptions.IgnorePatternWhitespace);
                Match m = r.Match(file_content);
                int matchCount = 0;
                while (m.Success)
                {
                    Console.WriteLine("Match {0}", ++matchCount);
                    for (int i = 1; i < m.Groups.Count; i++)
                    {
                        Group g = m.Groups[i];
                        Console.WriteLine("   Gorup {0} = '{1}'", i, g.Value);
                        for (int j = 0; j < g.Captures.Count; j++)
                        {
                            Capture c = g.Captures[j];
                            Console.WriteLine("      Capture {0} = '{1}', position = {2}, lenght = {3}",
                                j, c, c.Index, c.Length);
                        }
                    }

                    m = m.NextMatch();
                }

                m = r.Match(file_content);
                if (m.Success)
                {
                    string classname = m.Groups["name"].Value;
                    Console.WriteLine(classname);
                    foreach (var param in m.Groups["param"].Captures)
                    {
                        Console.WriteLine(param);
                        if(classname==param.ToString())
                        {
                            Console.WriteLine("Ошибка. Параметр не моежт иметь такое же имя как у класса {0}", param.ToString());
                            Console.ReadKey();
                            Environment.Exit(-1);
                        }
                    }
                    Console.WriteLine("end");
                }
                else Console.WriteLine("Ошибка синтаксиса");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.Write("\nНажмите любую клавишу...");
                Console.ReadKey();
                Environment.Exit(-1);
            }
        }
    }
}
