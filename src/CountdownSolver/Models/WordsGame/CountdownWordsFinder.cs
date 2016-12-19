using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CountdownSolver.Models
{
    public class CountdownWordsFinder
    {
        private static ICollection<string> dictionary;
        private ICollection<char> letters;
        private IProducerConsumerCollection<string> wordsFound;

        public static void populateWordList()
        {
            dictionary = File.ReadLines(@"Wordlist\words3.txt").ToList().AsReadOnly();
        }

        public ICollection<string> findAllWords(string inputLetters)
        {
            letters = inputLetters.ToList().AsReadOnly();
            startThreads();
            return wordsFound.ToList();
        }

        private void startThreads()
        {
            int processorCount = Environment.ProcessorCount;
            int range = dictionary.Count / processorCount;
            int currentMinIndex = 0;
            int currentMaxIndex = range;
            wordsFound = new ConcurrentBag<string>();
            List<Thread> threadList = new List<Thread>();
            for (int count = 0; count < processorCount; count++)
            {
                //final thread will go to the end of the list to avoid the possibility of missing indexes due to the / operator
                if (count == (processorCount -1))
                {
                    currentMaxIndex = dictionary.Count - 1;
                }
         
                WordFinderThread currentWordFinder = new WordFinderThread(currentMinIndex, currentMaxIndex, ref wordsFound, ref letters, ref dictionary);
                threadList.Add(new Thread(() => currentWordFinder.start()));
                currentMinIndex = currentMaxIndex;
                currentMaxIndex += range;           
            }

            foreach (Thread aThread in threadList)
            {
                aThread.Start();
            }

            foreach (Thread aThread in threadList)
            {
                aThread.Join();
            }
        }
    }
}
