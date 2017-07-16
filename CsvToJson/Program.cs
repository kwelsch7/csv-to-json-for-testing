using System;
using System.IO;

namespace CsvToJson
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string csvPath = CheckArgs(args);

            using (FileStream fileStream = File.Open(csvPath, FileMode.Open, FileAccess.Read))
            {

            }
        }

        public static string CheckArgs(string[] args)
        {
            if(args.Length < 1)
            {
                Console.WriteLine("No arguments given. Exiting");
                Environment.Exit(1);
            }

            if(args[0].Substring(args[0].Length - 4) != ".csv")
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
    }
}