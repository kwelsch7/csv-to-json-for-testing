using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsvToJson
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string csvPath = CheckArgs(args);
            List<string> allLines = new List<string>();

            using (FileStream fileStream = File.Open(csvPath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader reader = new StreamReader(fileStream))
                {
                    while(!reader.EndOfStream)
                    {
                        allLines.Add(reader.ReadLine());
                    }
                }
            }

            string theJson = ToJson(allLines);

            BeautifyAndPrintJson(theJson);
        }

        public static string CheckArgs(string[] args)
        {
            if(args.Length < 1)
            {
                Console.WriteLine("No arguments given. Exiting");
                Environment.Exit(1);
            }

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
            string[] keys = lines[0].Split(',');
            List<string[]> values = new List<string[]>();
            for(int x = 1; x < lines.Count; x++)
            {
                values.Add(lines[x].Split(','));
            }

            foreach(var valueSet in values)
            {
                if(valueSet.Length != keys.Length)
                {
                    Console.WriteLine("Given file not in correct format; all rows must have same number of columns. Exiting");
                    Environment.Exit(2);
                }

                theJson.Append("{");
                for (int y = 0; y < keys.Length; y++)
                {
                    theJson.Append($@"""{ keys[y].Trim() }"":""{ valueSet[y].Trim() }""");
                    if(keys.Length - 1 != y)
                    {
                        theJson.Append(",");
                    }
                }
                theJson.Append("}");
                if (valueSet != values[values.Count - 1])
                {
                    theJson.Append(",");
                }
            }
            theJson.Append("]");

            return theJson.ToString();
        }

        public static void BeautifyAndPrintJson(string json)
        {
            string prependLine = "";
            for (int c = 0; c < json.Length; c++)
            {
                if(c > 0 && (json[c - 1] == '[' || json[c - 1] == '{' || json[c - 1] == ','))
                {
                    Console.Write(prependLine);
                }
                Console.Write(json[c]);
                if (json[c] == '[' || json[c] == '{')
                {
                    if(!(c == json.Length) && json[c + 1] != '}' && json[c + 1] != ']')
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
                    Console.Write("\r\n");
                }
            }
            Console.WriteLine();
        }
    }
}