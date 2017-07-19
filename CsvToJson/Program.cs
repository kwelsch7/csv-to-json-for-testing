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
            try
            {
                string csvPath = CheckArgs(args);

                List<string> allLines = ParseCsv(csvPath);

                string theJson = ToJson(allLines);

                BeautifyAndPrintJson(theJson);
            }


            catch(ArgumentException caughtArg)
            {
                Console.WriteLine(caughtArg.Message);
                Console.WriteLine("Exiting");
                return 1;
            }
            catch(FormatException caughtFormat)
            {
                Console.WriteLine(caughtFormat.Message);
                Console.WriteLine("Exiting");
                return 2;
            }
            catch(FileNotFoundException caughtFileNotFound)
            {
                Console.WriteLine(caughtFileNotFound.Message);
                Console.WriteLine("Exiting");
                return 1;
            }
            catch(IOException caughtIO)
            {
                Console.WriteLine(caughtIO.Message);
                Console.WriteLine("Exiting");
                return -1; // Probably want to actually look these up
            }
            catch(Exception caught)
            {
                Console.WriteLine(caught);
                Console.WriteLine("Exiting");
                return -99;
            }

            return 0;
        }

        public static string CheckArgs(string[] args)
        {
            if (args.Length < 1)
            {
                throw new ArgumentException("No arguments given.");
            }

            if(!args[0].EndsWith(".csv"))
            {
                throw new FormatException("Given argument not a CSV file.");
            }

            if(!File.Exists(args[0]))
            {
                throw new FileNotFoundException("Given file not found.");
            }

            return args[0];
        }

        public static List<string> ParseCsv(string csvPath)
        {
            List<string> allLines = new List<string>();

            using (FileStream fileStream = File.Open(csvPath, FileMode.Open, FileAccess.Read))
            using (StreamReader reader = new StreamReader(fileStream))
            {
                while (!reader.EndOfStream)
                {
                    allLines.Add(reader.ReadLine());
                }
            }

            return allLines;
        }

        public static string ToJson(List<string> lines)
        {
            if (lines.Count == 0)
            {
                return "[]";
            }

            string[] keys = lines[0].Split(',').Select(key => key.Trim()).ToArray();
            List<string[]> values = ParseValues(lines.Skip(1));

            StringBuilder theJson = new StringBuilder("[");
            foreach (var valueSet in values)
            {
                if(valueSet.Length != keys.Length)
                {
                    throw new FormatException("Given file not in correct format; all rows must have same number of columns.");
                }

                theJson.Append(AppendKeyValues(keys, valueSet));

                if (valueSet != values.Last())
                {
                    theJson.Append(",");
                }
            }
            theJson.Append("]");

            return theJson.ToString();
        }

        public static List<string[]> ParseValues(IEnumerable<string> lines)
        {
            List<string[]> values = new List<string[]>();
            foreach (string line in lines)
            {
                values.Add(line.Split(',').Select(value => value.Trim()).ToArray());
            }

            return values;
        }

        public static string AppendKeyValues(string[] keys, string[] valueSet)
        {
            StringBuilder keyValueObject = new StringBuilder("{");
            int matchingIndex = 0;

            foreach(string key in keys)
            {
                keyValueObject.Append($@"""{ key }"":""{ valueSet[matchingIndex] }""");
                if (key != keys.Last())
                {
                    keyValueObject.Append(",");
                }
                matchingIndex++;
            }
            keyValueObject.Append("}");

            return keyValueObject.ToString();
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