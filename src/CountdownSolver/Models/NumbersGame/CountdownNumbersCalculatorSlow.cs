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

        private List<List<string>> reorderOperandPermutations(List<List<string>> inputList)
        {
            List<List<string>> unorderedOperandLists = new List<List<string>>(inputList);
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
                        unorderedOperandLists.RemoveAt(index);
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

        private void removeDuplicateOperandLists(ICollection<ICollection<string>> operandLists)
        {

            for(int index = 0; index < operandLists.Count; index++)
            {
                ICollection<string> currentList = operandLists.ElementAt(index);

                for (int index2 = 0; index2 < operandLists.Count; index2++)
                {

                    ICollection<string> currentCheckList = operandLists.ElementAt(index2);
                    if (currentList.Count == currentCheckList.Count)
                    {
                        bool remove = true;
                        foreach (string anOperand in currentCheckList)
                        {
                            if (!currentList.Contains(anOperand))
                            {
                                remove = false;
                            }
                        }

                        if (remove && index != index2)
                        {
                            operandLists.Remove(currentCheckList);
                        }
                    }
                }

            }

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
                    currentMaxIndex = operandLists.Count - 1;
                }
                NumbersCalculatorThread currentNumbersCalculatorThread = new NumbersCalculatorThread(currentMinIndex, currentMaxIndex, infixSolutions, operandLists, target, solutionsLimit);
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
            return operandLists;
        }

        //private ICollection<ICollection<string>> getAllOperandPermutations2()
        //{
        //    OperandPermutator2 operandPermutator = new OperandPermutator2(operands);
        //    ICollection<ICollection<string>> operandLists = operandPermutator.getOperandPermutations();
        //    return operandLists;
        //}

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
