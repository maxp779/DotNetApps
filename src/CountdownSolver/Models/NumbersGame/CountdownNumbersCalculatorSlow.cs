using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CountdownSolver.Models
{
    public class CountdownNumbersCalculatorSlow
    {
        private static IProducerConsumerCollection<Stack<string>> exactCalculations;
        private List<string> operands;
        private string target;
        bool findAllSolutions;
        IProducerConsumerCollection<string> infixSolutions = new ConcurrentBag<string>();
        IProducerConsumerCollection<string> postfixSolutions = new ConcurrentBag<string>();

        public CountdownNumbersCalculatorSlow(List<string> operands, string target, bool findAllSolutions = true)
        {
            this.operands = operands;
            this.target = target;
            this.findAllSolutions = findAllSolutions;
        }

        public List<string> calculate()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            //collect all operand permutations
            List<List<string>> operandLists = getAllOperandPermutations();
            
            //reorder operands for maximum efficiency
            List<List<string>> orderedOperandLists = reorderOperandPermutations(operandLists);

            //create list of threads to deal with operand permutations
            List<Thread> threadList = createThreadList(orderedOperandLists);

            //start each thread
            foreach (Thread aThread in threadList)
            {
                aThread.Start();
            }

            //wait for each thread to finish
            foreach (Thread aThread in threadList)
            {
                aThread.Join();
            }
            var elapsedMs = watch.ElapsedMilliseconds;
            return infixSolutions.ToList<string>();
        }

        /// <summary>
        /// Reorders the operands to maximize thread efficiency.
        /// 
        /// By default the operand permutation lists tend to be ordered by their size. Lists of size
        /// 2 end up at the start and size 6 at the end. The result is threads working on the end of the list
        /// have a lot more workto do  with the larger lists. Threads at the start would finish quickly as 
        /// their operand lists would only be size 2.
        /// 
        /// This method reorders like so:
        /// from: [2,2,2,3,3,3,4,4,4,5,5,5,6,6,6]
        /// to: [2,3,4,5,6,2,3,4,5,6,2,3,4,5,6]
        /// 
        /// Assuming those numbers represent a list's size.
        /// </summary>
        /// <param name="inputList"></param>
        /// <returns></returns>
        private List<List<string>> reorderOperandPermutations(ICollection<List<string>> inputList)
        {
            HashSet<List<string>> unorderedOperandLists = new HashSet<List<string>>(inputList);
            int minListSize = 2;
            int maxListSize = 2;

            foreach(ICollection<string> operandList in unorderedOperandLists)
            {
                if(operandList.Count > maxListSize)
                {
                    maxListSize = operandList.Count;
                }
            }

            List<List<string>> orderedOperandLists = new List<List<string>>();
            int currentSizeToAdd = minListSize;
            while (orderedOperandLists.Count < inputList.Count)
            {
                for(int index = 0; index < unorderedOperandLists.Count; index++)
                {
                    List<string> currentCollection = unorderedOperandLists.ElementAt(index);
                    if (currentCollection.Count == currentSizeToAdd)
                    {
                        orderedOperandLists.Add(currentCollection);
                        unorderedOperandLists.Remove(currentCollection);
                        break;
                    }
                }
                currentSizeToAdd++;
                if(currentSizeToAdd > maxListSize)
                {
                    currentSizeToAdd = minListSize;
                }
            }
            return orderedOperandLists;
        }

        private List<Thread> createThreadList(List<List<string>> operandLists)
        {
            int solutionsLimit = -1;
            if (!findAllSolutions)
            {
                solutionsLimit = calculateSolutionsLimit();
            }

            int processorCount = Environment.ProcessorCount;
            int range = operandLists.Count / processorCount;
            int currentMinIndex = 0;
            int currentMaxIndex = range;
            List<Thread> threadList = new List<Thread>();
            int threadNumber = 0;
            for (int count = 0; count < processorCount; count++)
            {
                //final thread will go to the end of the list to avoid the possibility of missing indexes due to the / operator when calculating the range
                if (count == (processorCount - 1))
                {
                    currentMinIndex = currentMaxIndex;
                    currentMaxIndex = operandLists.Count - 1;
                }
                NumbersCalculatorThread currentNumbersCalculatorThread = new NumbersCalculatorThread(currentMinIndex, currentMaxIndex, infixSolutions, postfixSolutions, operandLists, target, solutionsLimit);
                Thread thread = new Thread(delegate ()
                {
                    currentNumbersCalculatorThread.start();
                });
                thread.Name = "NumbersCalculatorThread:" + threadNumber;
                threadList.Add(thread);
                currentMinIndex = currentMaxIndex;
                currentMaxIndex += range;
                threadNumber++;
            }
            return threadList;
        }

        private List<List<string>> getAllOperandPermutations()
        {
            List<List<string>> operandLists = new List<List<string>>();
            OperandPermutator operandPermutator;
            for (int operandListSize = 2; operandListSize <= operands.Count; operandListSize++)
            {
                operandPermutator = new OperandPermutator(operands, operandListSize);

                while (operandPermutator.nextOperandPermutation())
                {
                    operandLists.Add(operandPermutator.getCurrentOperandPermutation());
                }
            }

            //remove duplicates, this is possible if duplicate numbers were supplied i.e [100, 10, 36, 36, 4, 9]
            List<List<string>> operandListsNoDuplicates = new List<List<string>>();
            bool addList;
            foreach(List<string> outerList in operandLists)
            {
                addList = true;
                foreach (List<string> innerList in operandListsNoDuplicates)
                {
                    if(outerList.SequenceEqual(innerList))
                    {
                        addList = false;
                    }
                }

                if(addList)
                {
                    operandListsNoDuplicates.Add(outerList);
                }
            }
            return operandListsNoDuplicates;
        }

        private int calculateSolutionsLimit()
        {
            int processorCount = Environment.ProcessorCount;
            if(processorCount < 4)
            {
                return 10;
            }
            else if(processorCount < 8)
            {
                return 20;
            }
            else if(processorCount < 16)
            {
                return 30;
            }
            return -1; //-1 = find all solutions        
        }
    }
}
