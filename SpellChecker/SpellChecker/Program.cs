using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string inputPath = "";
            Console.WriteLine("Input:");
            Console.WriteLine("Write absolute/relative path to file or empty string for default path (input.txt)");
            inputPath = Console.ReadLine();
            if (inputPath.Equals(""))
            {
                inputPath = @"input.txt";
            }

            var outputPath = "";
            Console.WriteLine("Output:");
            Console.WriteLine("Write absolute/relative path to file or empty string for default path (output.txt)");
            outputPath = Console.ReadLine();
            if (outputPath.Equals(""))
            {
                outputPath = @"output.txt";
            }

            var dict = new List<string>();
            IOSystem.FileInputDictionary(inputPath, ref dict);

            Console.WriteLine();
            var timer = Stopwatch.StartNew();

            List<string[]> textToCheck = new List<string[]>();
            bool key = IOSystem.FileInputTextToCheck(inputPath, ref textToCheck);
            var checkedText = SpellChecker.SpellCheckText(dict, textToCheck);
            IOSystem.FileOutput(outputPath, checkedText, IOSystem.Mod.Overwrite);

            while (!key)
            {
                textToCheck = new List<string[]>();
                key = IOSystem.FileInputTextToCheck(inputPath, ref textToCheck);

                // Get the corrected text
                checkedText = SpellChecker.SpellCheckText(dict, textToCheck);
                IOSystem.FileOutput(outputPath, checkedText, IOSystem.Mod.Append);
            }
            IOSystem.CloseReadStream();

            timer.Stop();
            Console.WriteLine($"Execution time: {timer.ElapsedMilliseconds} ms");
            Console.WriteLine();

            Console.ReadKey();
        }
    }
}
