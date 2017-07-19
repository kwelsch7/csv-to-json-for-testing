using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CsvToJson
{
    public class Program
    {
        public static int Main(string[] args)
        {
            // rather than using Environment.Exit() to blow up, always bubble it to the top (especially since it might be someone else using your code, not all one app like here)
            try
            {
                string csvPath = CheckArgs(args);

                // Filling allLines (with the StreamReader business below) should be pulled out into a method
                List<string> allLines = new List<string>();

                // This can be done with nested usings (and is preferred for readability) to the set of braces under the first using
                using (FileStream fileStream = File.Open(csvPath, FileMode.Open, FileAccess.Read))
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    // Josh made it sound like (but didn't explicitely say) there's a way to do this all at once/without a while loop
                    while (!reader.EndOfStream)
                    {
                        allLines.Add(reader.ReadLine());
                    }
                }

                string theJson = ToJson(allLines);

                BeautifyAndPrintJson(theJson);
            }
            // We could say that Main's only job is to call a bunch of other stuff, so one way of thinking of things is that it's all one try/catch block, the final point for success or explosions
            catch(ArgumentException caught)
            {
                Console.WriteLine(caught.Message);
                return 1;
            }
            // more specific exceptions, then finally the Pokemon exception printing the whole stack trace
            catch(Exception caught)
            {
                Console.WriteLine(caught);
                return -99;
            }

            return 0;
        }

        public static string CheckArgs(string[] args)
        {
            // Bubble up exceptions rather than bailing here
            if (args.Length < 1)
            {
                throw new ArgumentException("No arguments given. Exiting");
            }

            // There is an "EndsWith" method for strings
            if(args[0].Length < 4 || args[0].Substring(args[0].Length - 4) != ".csv")
            {
                Console.WriteLine("Given argument not a CSV file. Exiting");
                Environment.Exit(2);
            }

            if(!File.Exists(args[0]))
            {
                Console.WriteLine("Given file not found. Exiting");
                Environment.Exit(1);
            }

            return args[0];
        }

        public static string ToJson(List<string> lines)
        {
            if (lines.Count == 0)
            {
                return "[]";
            }

            StringBuilder theJson = new StringBuilder("[");
            // pull out ParseCSV into its own method
            string[] keys = lines[0].Split(',').Select(line => line.Trim()).ToArray();
            List<string[]> values = new List<string[]>();
            // Use linq to start at 1 instead of for loop
            for(int x = 1; x < lines.Count; x++)
            {
                values.Add(lines[x].Split(','));
            }

            foreach(var valueSet in values) // could pull out into method "GenerateLine" or something
            {
                if(valueSet.Length != keys.Length)
                {
                    // Exception, not exit (perhaps define my own specific kind)
                    Console.WriteLine("Given file not in correct format; all rows must have same number of columns. Exiting");
                    Environment.Exit(2);
                }

                theJson.Append("{");
                // could convert to a foreach and keep my own index
                for (int y = 0; y < keys.Length; y++)
                {
                    theJson.Append($@"""{ keys[y] }"":""{ valueSet[y] }""");
                    if(keys.Length - 1 != y)
                    {
                        theJson.Append(",");
                    }
                }
                theJson.Append("}"); // this would be an easy spot to add, say, a newline vs. having a complicated beautify method that comes back into this string later
                if (valueSet != values[values.Count - 1])
                {
                    theJson.Append(",");
                }
            }
            theJson.Append("]");

            return theJson.ToString();
        }

        public static void BeautifyAndPrintJson(string json) // could put this logic in the json builder, which could have an option for pretty/not pretty
        {
            string prependLine = "";
            for (int c = 0; c < json.Length; c++)
            {
                if(c > 0 && (json[c - 1] == '[' || json[c - 1] == '{' || json[c - 1] == ','))
                {
                    Console.Write(prependLine); // should make a pretty string, not constantly print to the console
                }
                Console.Write(json[c]);
                if (json[c] == '[' || json[c] == '{')
                {
                    if(!(c == json.Length) && json[c + 1] != '}' && json[c + 1] != ']')  // don't have these hard-coded characters everywhere
                    {
                        Console.Write("\r\n");
                        prependLine += "  ";
                    }
                }
                else if(c < json.Length - 1 && (json[c + 1] == '}' || json[c + 1] == ']'))
                {
                    if(prependLine.Length > 1)
                    {
                        Console.Write("\r\n");
                        prependLine = prependLine.Substring(0, prependLine.Length - 2);
                        Console.Write(prependLine);
                    }
                }

                if(json[c] == ':')
                {
                    Console.Write(" ");
                }
                if(json[c] == ',')
                {
                    Console.Write(prependLine);
                    Console.Write(Environment.NewLine); // "\r\n" is platform dependent
                    // Side notes: Console.Write(Environment.NewLine) == Console.WriteLine()
                    // Path also has a platform independent version of path/to/file.txt vs path\to\file.txt
                    // Path.DirectorySeparatorChar;
                }
            }
            Console.WriteLine();
        }
    }
}