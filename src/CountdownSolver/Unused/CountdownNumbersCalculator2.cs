using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountdownSolver.Unused
{
    public class CountdownNumbersCalculator2
    {
        public static List<string> calculate(List<int> numbers, int targetNumber, int threadCount = 1)
        {
            string[] operators = new string[4] { "+", "-", "*", "/" };
            int operationsRequired = numbers.Count - 1;

            //we will use a base 4 number system since there are 4 operators
            int[] currentOperatorIndexes = new int[operationsRequired];

            //fill array with 0's
            for (int index = 0; index < currentOperatorIndexes.Length; index++)
            {
                currentOperatorIndexes[index] = 0;
            }

            int currentClosestTarget = 0;
            List<string> currentClosestOperationsList = new List<string>();

            bool calculating = true;
            bool discardResult = false;
            List<string> currentOperationsList = new List<string>();
            List<int> currentNumbersList = new List<int>(numbers);
            int loopcount = 0;
            while (calculating)
            {
                //do the calculation
                //int currentNumber = numbers.ElementAt(0);
                int currentNumber = 0;
                int nextNumber = 0;

                for (int numberIndex = 0; numberIndex < numbers.Count; numberIndex++)
                {
                    discardResult = false;
                    currentOperationsList.Clear();
                    currentNumbersList = new List<int>(numbers);

                    currentNumber = currentNumbersList[numberIndex];
                    currentNumbersList.Remove(currentNumber);

                    //if nextNumber index is out of bounds go back to start of array
                    if (currentNumbersList.Count == 0)
                    {
                        break;
                    }
                    else
                    {
                        nextNumber = currentNumbersList.ElementAt(numberIndex % currentNumbersList.Count);
                    }
                    currentNumbersList.Remove(nextNumber);


                    for (int operatorIndex = 0; operatorIndex < currentOperatorIndexes.Length; operatorIndex++)
                    {
                        string currentOperator = operators[currentOperatorIndexes[operatorIndex]];

                        if (currentOperator == "+")
                        {
                            currentOperationsList.Add(currentNumber.ToString());
                            currentOperationsList.Add("+");
                            currentOperationsList.Add(nextNumber.ToString());
                            currentOperationsList.Add("=");
                            currentNumber = add(currentNumber, nextNumber);
                            currentOperationsList.Add(currentNumber.ToString());
                        }
                        else if (currentOperator == "-")
                        {
                            currentOperationsList.Add(currentNumber.ToString());
                            currentOperationsList.Add("-");
                            currentOperationsList.Add(nextNumber.ToString());
                            currentOperationsList.Add("=");
                            currentNumber = subtract(currentNumber, nextNumber);
                            currentOperationsList.Add(currentNumber.ToString());

                        }
                        else if (currentOperator == "*")
                        {
                            currentOperationsList.Add(currentNumber.ToString());
                            currentOperationsList.Add("*");
                            currentOperationsList.Add(nextNumber.ToString());
                            currentOperationsList.Add("="); currentNumber = multiply(currentNumber, nextNumber);
                            currentOperationsList.Add(currentNumber.ToString());

                        }
                        else if (currentOperator == "/")
                        {
                            currentOperationsList.Add(currentNumber.ToString());
                            currentOperationsList.Add("/");
                            currentOperationsList.Add(nextNumber.ToString());
                            currentOperationsList.Add("="); currentNumber = divide(currentNumber, nextNumber);
                            currentOperationsList.Add(currentNumber.ToString());

                            //if math rounding was done discard this result
                            if (currentNumber % nextNumber > 0)
                            {
                                discardResult = true;
                            }

                        }

                        //if nextNumber index is out of bounds go back to start of array
                        if (currentNumbersList.Count == 0)
                        {
                            break;
                        }
                        else
                        {
                            nextNumber = currentNumbersList.ElementAt(numberIndex % currentNumbersList.Count);
                        }
                        currentNumbersList.Remove(nextNumber);
                    }

                    //if we have the exact target
                    if (currentNumber == targetNumber && !discardResult)
                    {
                        calculating = false;
                        currentClosestTarget = currentNumber;
                        currentClosestOperationsList = new List<string>(currentOperationsList);
                    }//else if we are closer to the target than before update the currentClosest variables
                    else if (Math.Abs(targetNumber - currentNumber) < Math.Abs(targetNumber - currentClosestTarget) && !discardResult)
                    {
                        currentClosestTarget = currentNumber;
                        currentClosestOperationsList = new List<string>(currentOperationsList);
                    }
                }

                //increase the base4 currentOperatorIndexes array by one, or if at the maximum number we are finished and end the calculation
                bool increaseNextIndex = false;
                for (int index = currentOperatorIndexes.Length - 1; index >= 0; index--)
                {
                    if (increaseNextIndex)
                    {
                        currentOperatorIndexes[index] = currentOperatorIndexes[index] + 1;
                        increaseNextIndex = false;
                    }

                    if (index == currentOperatorIndexes.Length - 1)
                    {
                        currentOperatorIndexes[index] = currentOperatorIndexes[index] + 1;
                    }

                    if (currentOperatorIndexes[index] > 3 && index != 0)
                    {
                        currentOperatorIndexes[index] = 0;
                        increaseNextIndex = true;
                    }

                    if (currentOperatorIndexes[index] > 3 && index == 0)
                    {
                        calculating = false;
                    }
                }
                loopcount++;
            }

            //List<string> outputList = new List<string>();

            //for (int index = 0; index < numbers.Count; index++)
            //{
            //    outputList.Add(numbers[index].ToString());

            //    if (index <= currentClosestOperationsList.Count-1)
            //    { 
            //        outputList.Add(currentClosestOperationsList[index]);
            //    }
            //    else
            //    {
            //        outputList.Add("=");
            //        outputList.Add(currentClosestTarget.ToString());
            //    }
            //}

            return currentClosestOperationsList;

            //long maxNumber = 1;
            //for(int count = 0; count < operationsRequired; count++)
            //{
            //    maxNumber = maxNumber * operationsRequired;
            //}

            //long numberRangeForThreads = (maxNumber / threadCount);
            //bool addOneToFinalRange = (maxNumber % threadCount) == 1;

            //long threadStartNumber = 0;
            //long threadEndNumber = numberRangeForThreads;
            //for (int count = 0; count < threadCount; count++)
            //{
            //    //here we start a thread with threadStartNumber and threadEndNumber

            //    threadStartNumber = threadEndNumber;
            //    threadEndNumber = threadEndNumber + numberRangeForThreads;
            //}


            //string maxNumberBase4 = DecimalToArbitrarySystem(maxNumber, 4);



        }


        private static int add(int firstInt, int secondInt)
        {
            return firstInt + secondInt;
        }
        private static int subtract(int firstInt, int secondInt)
        {
            return firstInt - secondInt;
        }
        private static int multiply(int firstInt, int secondInt)
        {
            return firstInt * secondInt;
        }
        private static int divide(int firstInt, int secondInt)
        {
            return firstInt / secondInt;
        }
        
        private static void threadedCalculatorMethod(long startNumber, long endNumber, ConcurrentBag<string> solutions)
        {

        }


        /// <summary>
        /// Converts the given decimal number to the numeral system with the
        /// specified radix (in the range [2, 36]).
        /// </summary>
        /// <param name="decimalNumber">The number to convert.</param>
        /// <param name="radix">The radix of the destination numeral system (in the range [2, 36]).</param>
        /// <returns></returns>
        private static string DecimalToArbitrarySystem(long decimalNumber, int radix)
        {
            const int BitsInLong = 64;
            const string Digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            if (radix < 2 || radix > Digits.Length)
                throw new ArgumentException("The radix must be >= 2 and <= " + Digits.Length.ToString());

            if (decimalNumber == 0)
                return "0";

            int index = BitsInLong - 1;
            long currentNumber = Math.Abs(decimalNumber);
            char[] charArray = new char[BitsInLong];

            while (currentNumber != 0)
            {
                int remainder = (int)(currentNumber % radix);
                charArray[index--] = Digits[remainder];
                currentNumber = currentNumber / radix;
            }

            string result = new String(charArray, index + 1, BitsInLong - index - 1);
            if (decimalNumber < 0)
            {
                result = "-" + result;
            }

            return result;
        }
    }
}
