using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CountdownSolver.Models
{
    public class WordFinderThread
    {
        private int minIndex;
        private int maxIndex;
        private IProducerConsumerCollection<string> wordsFound;
        private ICollection<char> letters;
        private ICollection<string> dictionary;

        public WordFinderThread(int minIndex, int maxIndex, ref IProducerConsumerCollection<string> wordsFound, ref ICollection<char> letters, ref ICollection<string> dictionary)
        {
            this.minIndex = minIndex;
            this.maxIndex = maxIndex;
            this.wordsFound = wordsFound;
            this.letters = letters;
            this.dictionary = dictionary;
        }

        public void start()
        {
            bool addWord;
            char[] currentWordArray;

            for (int index = minIndex; index < maxIndex; index++)
            {
                string currentWord = dictionary.ElementAt(index);
                currentWordArray = currentWord.ToCharArray();
                List<char> currentLetters = letters.ToList();

                addWord = true;

                foreach (char currentLetter in currentWordArray)
                {
                    if (currentLetters.Contains(currentLetter))
                    {
                        currentLetters.Remove(currentLetter);
                    }
                    else
                    {
                        addWord = false;
                        break;
                    }
                }

                if (addWord)
                {
                    wordsFound.TryAdd(currentWord);
                }
            }
        }
    }
}
