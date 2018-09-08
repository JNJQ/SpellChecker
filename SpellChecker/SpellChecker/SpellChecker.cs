using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellChecker
{
    /// <summary>
    /// SpellChecker - static class responsible for the input/output
    /// </summary>
    static class SpellChecker
    {
        /// <summary>
        /// SIMPLE EDIT - Insert or Delete character
        /// </summary>
        private const int SIMPLE_EDIT_COST = 1;
        private const int REPLACE_COST = 2 * SIMPLE_EDIT_COST;
        private const int PERMITTED_COUNT_OF_EDIT = 2;
        /// <summary>
        /// LevenshteinDistance - implementation of algorithm Levenshtein distance
        /// </summary>
        /// <param name="word">First word to compare</param>
        /// <param name="compareWord">Second word to compare</param>
        /// <returns>Count of edits necessary to convert the first word to the second or v.v.</returns>
        public static int LevenshteinDistance(string word, string compareWord)
        {
            if (word == null)
            {
                throw new ArgumentNullException("word");
            }
            if (compareWord == null)
            {
                throw new ArgumentNullException("compareWord");
            }

            var levenshteinMatrix = new int[word.Length + 1, compareWord.Length + 1];
            int diff;

            for (int i = 0; i <= word.Length; i++)
            {
                levenshteinMatrix[i, 0] = i;
            }
            for (int j = 0; j <= compareWord.Length; j++)
            {
                levenshteinMatrix[0, j] = j;
            }

            for (int i = 1; i <= word.Length; i++)
            {
                for (int j = 1; j <= compareWord.Length; j++)
                {
                    diff = (word[i - 1].Equals(compareWord[j - 1])) ? 0 : REPLACE_COST;

                    levenshteinMatrix[i, j] = Math.Min(levenshteinMatrix[i - 1, j - 1] + diff,
                        Math.Min(levenshteinMatrix[i - 1, j] + SIMPLE_EDIT_COST, levenshteinMatrix[i, j - 1] + SIMPLE_EDIT_COST));
                }
            }
            return levenshteinMatrix[word.Length, compareWord.Length];
        }
        /// <summary>
        /// SpellCheckText - corrects the input text using the dictionary
        /// </summary>
        /// <param name="dict">Dictionary</param>
        /// <param name="textToCheck">Compared text</param>
        /// <returns>Corrected text</returns>
        public static List<string[]> SpellCheckText(List<string> dict, List<string[]> textToCheck)
        {

            var tasks = new List<Task>();

            for (int i = 0; i < textToCheck.Count; i++)
            {
                for (int j = 0; j < textToCheck[i].Length; j++)
                {
                    var copyI = i;
                    var copyJ = j;
                    tasks.Add(Task.Factory.StartNew(() =>
                    {
                        int min = PERMITTED_COUNT_OF_EDIT + 1;
                        var checkedWord = new List<string>();

                        foreach (var dictWord in dict)
                        {
                            var res = LevenshteinDistance(textToCheck[copyI][copyJ].ToLower(), dictWord.ToLower());

                            // Remember the word if it differs from the dictionary word by less than two insert or deletes
                            if (Math.Abs(textToCheck[copyI][copyJ].Length - dictWord.Length) < PERMITTED_COUNT_OF_EDIT)
                            {
                                // If the dictionary word is closer than the previous one, we remember the new one
                                if (res < min)
                                {
                                    checkedWord.Clear();
                                    min = res;
                                    checkedWord.Add(dictWord);
                                }
                                // If the dictionary word is also close, like the previous one, we remember both
                                else if (res == min && res <= PERMITTED_COUNT_OF_EDIT)
                                {
                                    checkedWord.Add(dictWord);
                                }
                            }

                        }

                        // If no dictionary word is suitable, remember the word itself in the format
                        // {word?}
                        if (checkedWord.Count == 0)
                        {
                            checkedWord.Add("{" + textToCheck[copyI][copyJ] + "?}");
                        }

                        // Convert words in the format {word1 word2 ... wordN} if you remember a few words
                        // Save the result in checkedText
                        if (checkedWord.Count == 1)
                        {
                            textToCheck[copyI][copyJ] = checkedWord.First();
                        }
                        else
                        {
                            string resultWord = "{";
                            foreach (var el in checkedWord)
                            {
                                resultWord += el + " ";
                            }
                            textToCheck[copyI][copyJ] = resultWord.Substring(0, resultWord.Length - 1) + "}";
                        }

                    }));
                }
            }

            Task.WaitAll(tasks.ToArray());           
            return textToCheck;
        }
    }
}
