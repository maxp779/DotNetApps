using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CountdownSolver.Models
{
    public class WordList
    {
        private static ICollection<string> wordList;

        public static void populateWordList()
        {
            wordList = File.ReadLines(@"Wordlist\words3.txt").ToList();
        }

        /// <summary>
        /// Finds all the words in the Enlish language that can be made with just the letters provided
        /// </summary>
        /// <param name="letters">input letters</param>
        /// <returns>a collection containing all the words which can be made with the input letters</returns>
        public static ICollection<string> findAllWords(string letters)
        {
            char[] lettersArray = letters.ToCharArray();
            ICollection<string> wordsFound = new List<string>();
            Boolean addWord;
            char[] currentWordArray;
            foreach (string currentWord in wordList)
            {
                currentWordArray = currentWord.ToCharArray();
                addWord = true;
                foreach (char currentLetter in currentWordArray)
                {
                    if(!lettersArray.Contains(currentLetter))
                    {
                        addWord = false;
                        break;
                    }
                }

                if(addWord)
                {
                    wordsFound.Add(currentWord);
                }
            }

            ICollection<string> output = removeUnsuitableWords(letters, wordsFound);
            return output;
        }

        /// <summary>
        /// This method removes words which do not account for the limited use of the letters.
        /// e.g
        /// if given the letters "h e l o"
        /// "hello" would not be a valid word as there is only a single "l" to be used
        /// "hole" would be valid as it does not violate countdowns use a letter only once rule
        /// </summary>
        /// <param name="letters"></param>
        /// <param name="wordsList"></param>
        /// <returns></returns>
        private static ICollection<string> removeUnsuitableWords(string letters, ICollection<string> wordsList)
        {
            ICollection<string> output = new List<string>(wordsList);
            foreach(string word in wordsList)
            {
                List<char> currentLetters = letters.ToList();
                char[] currentWordChars = word.ToCharArray();
                foreach(char currentChar in currentWordChars)
                {
                    if(currentLetters.Contains(currentChar))
                    {
                        currentLetters.Remove(currentChar);
                    }
                    else
                    {
                        output.Remove(word);
                    }
                }
            }
            return output;
        }
    }
}
