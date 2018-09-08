using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace SpellChecker
{
    ///<summary>
    ///IOSystem - class responsible for the input/output
    ///</summary>
    static class IOSystem
    {
        private const string BLOCK_END_STRING = "===";
        private const int LINES_COUNT = 100;
        private static char[] SPLIT_CHARACTERS =  { '\n', ' ' };
        private static string _path = "";
        private static StreamReader _streamReader;
        public enum Mod { Overwrite, Append };
        /// <summary>
        /// FileInputDictionary - method for read dictionary from file
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <param name="dict">Variable where written the dictionary</param>
        public static void FileInputDictionary(string path, ref List<string> dict)
        {
            try
            {
                if(!_path.Equals(path))
                {
                    if (_streamReader != null)
                    {
                        _streamReader.Close();
                    }
                    _path = path;
                    _streamReader = new StreamReader(path);
                }

                var str = "";
                while (!(str = _streamReader.ReadLine()).Equals(BLOCK_END_STRING))
                {
                    dict.AddRange(str.Split(SPLIT_CHARACTERS[1]));
                }   
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Такого файла не существует");
            }
        }
        /// <summary>
        /// FileInputTextToCheck - method for read text to check from file
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <param name="textToCheck">Variable where written the text to check</param>
        /// <returns>End of file or not</returns>
        public static bool FileInputTextToCheck(string path, ref List<string[]> textToCheck)
        {
            try
            {
                if (!_path.Equals(path))
                {
                    if (_streamReader != null)
                    {
                        _streamReader.Close();
                    }
                    _path = path;
                    _streamReader = new StreamReader(path);
                }

                var str = "";
                for (int i = 0; i < LINES_COUNT; i++)
                {
                    str = _streamReader.ReadLine();
                    if (str.Equals(BLOCK_END_STRING))
                    {
                        break;
                    }
                    textToCheck.Add(str.Split(SPLIT_CHARACTERS[1]));
                }
                if (str.Equals(BLOCK_END_STRING))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Такого файла не существует");
                return true;
            }
        }
        /// <summary>
        /// CloseREadStream - closes an open stream for reading 
        /// </summary>
        public static void CloseReadStream()
        {
            if (_streamReader != null)
            {
                _streamReader.Close();
            }
        }
        /// <summary>
        /// FileOutput - method for write output to file
        /// </summary>
        /// <param name="path">Path to file</param>
        /// <param name="outputText">Text to write</param>
        /// <param name="mode">Recording mode</param>
        public static void FileOutput(string path, List<string[]> outputText, Mod mode)
        {

            string outputString = "";

            for (int i = 0; i < outputText.Count; i++)
            {
                for (int j = 0; j < outputText[i].Length; j++)
                {
                    outputString += outputText[i][j] + " ";
                }
                outputString = outputString.Substring(0, outputString.Length - 1) + "\r\n";
            }

            if (mode == Mod.Overwrite)
            {
                outputString = outputString.Substring(0, outputString.Length - 2);
                File.WriteAllText(path, outputString);
            }
            else if (mode == Mod.Append)
            {
                File.AppendAllText(path, outputString);
            }


        }
        /// <summary>
        /// ConsoleInput - method for read input from console
        /// </summary>
        /// <param name="dict">Variable where written the dictionary</param>
        /// <param name="textToCheck">Variable where written the text for check</param>
        public static void ConsoleInput(ref List<string> dict, ref List<string[]> textToCheck)
        {
            string str;

            while (!(str = Console.ReadLine()).Equals(BLOCK_END_STRING))
            {
                dict.AddRange(str.Split(SPLIT_CHARACTERS[1]));
            }

            while (!(str = Console.ReadLine()).Equals(BLOCK_END_STRING))
            {
                textToCheck.Add(str.Split(SPLIT_CHARACTERS[1]));
            }
        }
        /// <summary>
        /// ConsoleOutput - method for write output to console
        /// </summary>
        /// <param name="outputText">Text to write</param>
        public static void ConsoleOutput(List<string[]> outputText)
        {
            foreach (var outputStr in outputText)
            {
                foreach (var outputWord in outputStr)
                {
                    Console.Write(outputWord + " ");
                }
                Console.Write("\n");
            }
            Console.ReadKey();
        }
    }
}
