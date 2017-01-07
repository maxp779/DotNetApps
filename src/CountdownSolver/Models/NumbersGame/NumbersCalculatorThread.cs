using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CountdownSolver.Models
{
    public class NumbersCalculatorThread
    {
        //passed in externally
        int minIndex;
        int maxIndex;
        IProducerConsumerCollection<string> outputInfixSolutions;
        IProducerConsumerCollection<string> outputPostfixSolutions;
        List<List<string>> operandList;
        int target;

        //created in this thread
        List<List<string>> currentPostfixSolutions;
        List<string> currentClosestCalculation;
        int currentClosestTarget;
        bool findMoreSolutions = true;
        int solutionsLimit;

        public NumbersCalculatorThread(int minIndex, int maxIndex, IProducerConsumerCollection<string> infixSolutionsCollection, IProducerConsumerCollection<string> postfixSolutionsCollection, 
            List<List<string>> operandList, string target, int solutionsLimit = -1)
        {
            this.minIndex = minIndex;
            this.maxIndex = maxIndex;
            this.outputInfixSolutions = infixSolutionsCollection;
            this.outputPostfixSolutions = postfixSolutionsCollection;

            this.operandList = operandList;
            this.target = int.Parse(target);
            currentPostfixSolutions = new List<List<string>>();
            this.solutionsLimit = solutionsLimit;
        }

        public void start()
        {
            iterateThroughOperands();
        }

        /// <summary>
        /// Iterate through each operand permutation
        /// </summary>
        private void iterateThroughOperands()
        {
            int currentIndex = minIndex;
            while (currentIndex < maxIndex && findMoreSolutions)
            {
                ICollection<string> currentOperandList = operandList.ElementAt(currentIndex);
                iterateThroughAllOperators(currentOperandList);
                currentIndex++;
            }

            convertToInfix(currentPostfixSolutions, outputInfixSolutions);
        }

        /// <summary>
        /// Iterate through each operator set with the current operand permutation
        /// </summary>
        /// <param name="currentOperandList"></param>
        private void iterateThroughAllOperators(ICollection<string> currentOperandList)
        {
            OperatorPermutator operatorPermutator = new OperatorPermutator(currentOperandList.Count-1); //if 6 operands we need 5 operators etc so always -1
            while (operatorPermutator.nextOperatorPermutation() && findMoreSolutions)
            {
                ICollection<string> currentOperators = operatorPermutator.getCurrentOperatorPermutation();
                iterateThroughAllCalculations(currentOperandList, currentOperators);
            }
        }
    
        /// <summary>
        /// Iterate through each possible valid calculation permutation with the current operator and operand permutations
        /// </summary>
        /// <param name="currentOperandList"></param>
        /// <param name="currentOperators"></param>
        private void iterateThroughAllCalculations(ICollection<string> currentOperandList, ICollection<string> currentOperators)
        {
            CalculationPermutator calculationPermutator = new CalculationPermutator(currentOperandList, currentOperators);
            while (calculationPermutator.nextCalculationPermutation() && findMoreSolutions)
            {
                List<string> currentCalculation = calculationPermutator.getCurrentCalculationPermutation();
                solveCalculation(currentCalculation);
            }
            
            //for performance reasons it is best to limit the amount of solutions to find in cases where processor count is low
            if (currentPostfixSolutions.Count >= solutionsLimit && solutionsLimit != -1)
            {
                findMoreSolutions = false;
            }
        }

        /// <summary>
        /// Calculate if the current calculation permutation hits the target or not.
        /// If it does not, check to see if it beats the current closest calculation.
        /// </summary>
        /// <param name="currentCalculationStack"></param>
        private void solveCalculation(List<string> currentCalculation)
        {
            currentCalculation.Reverse();
            Stack<string> currentCalculationStack = new Stack<string>(currentCalculation);
            Stack<int> currentInts = new Stack<int>();

            bool calculating = true;
            bool validCalculation = true;
            while (calculating && validCalculation)
            {
                if (currentCalculationStack.Count == 0 && currentInts.Count == 1)
                {
                    calculating = false;
                }
                else
                {
                    string currentStackString = currentCalculationStack.Pop();
                    int currentStackNumber;
                    if (int.TryParse(currentStackString, out currentStackNumber))
                    {
                        currentInts.Push(currentStackNumber);
                    }
                    else if (currentStackString == "+")
                    {
                        add(currentInts);
                    }
                    else if (currentStackString == "-")
                    {
                        //it is invalid if negative numbers are used at any point
                        validCalculation = subtract(currentInts);
                    }
                    else if (currentStackString == "*")
                    {
                        multiply(currentInts);
                    }
                    else if (currentStackString == "/")
                    {
                        //it is invalid if the numbers do not divide evenly
                        validCalculation = divide(currentInts);
                    }
                }
            }

            //result will be the final element in the stack
            if (validCalculation)
            {
                //int result = int.Parse(currentCalculationStack.Pop());
                int result = currentInts.Pop();
                if (result == target)
                {
                    currentCalculation.Reverse();
                    currentPostfixSolutions.Add(currentCalculation);
                    outputPostfixSolutions.TryAdd(listToString(currentCalculation));
                }
                else //it does not hit the target so check if it beats the current closest calculation
                {
                    //Math.Abs removes the issue with negative numbers
                    int currentDifference = target - result;
                    int closestDifference = target - currentClosestTarget;
                    if (Math.Abs(currentDifference) < Math.Abs(closestDifference) || currentClosestTarget == 0)
                    {
                        currentClosestTarget = result;
                        currentCalculation.Reverse();
                        currentClosestCalculation = currentCalculation;
                    }
                }
            }
        }

        private void add(Stack<int> currentInts)
        {
            int secondOperand = currentInts.Pop();
            int firstOperand = currentInts.Pop();
            int result = firstOperand + secondOperand;
            currentInts.Push(result);
        }
        private bool subtract(Stack<int> currentInts)
        {
            int secondOperand = currentInts.Pop();
            int firstOperand = currentInts.Pop();
            int result = firstOperand - secondOperand;
            if (result >= 0)
            {
                currentInts.Push(result);
                return true;
            }
            else
            {
                return false;
            }
        }
        private void multiply(Stack<int> currentInts)
        {
            int secondOperand = currentInts.Pop();
            int firstOperand = currentInts.Pop();
            int result = firstOperand * secondOperand;
            currentInts.Push(result);
        }
        private bool divide(Stack<int> currentInts)
        {
            int secondOperand = currentInts.Pop();
            int firstOperand = currentInts.Pop();

            if (firstOperand == 0 || secondOperand == 0)
            {
                return false;
            }

            if (firstOperand % secondOperand == 0)
            {
                int result = firstOperand / secondOperand;
                currentInts.Push(result);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void convertToInfix(List<List<string>> postfixSolutions, IProducerConsumerCollection<string> infixSolutions)
        {
            //List<string> infixSolutions = new List<string>();
            foreach (List<string> postfixSolution in postfixSolutions)
            {
                Stack<string> infixStack = new Stack<string>();
                for (int index = 0; index < postfixSolution.Count; index++)
                {
                    string currentElement = postfixSolution.ElementAt(index);
                    if (!isOperator(currentElement))
                    {
                        infixStack.Push(currentElement);
                    }
                    else
                    {
                        if (infixStack.Count < 2)
                        {
                            //invalid postfix expression
                        }
                        else
                        {
                            string first = infixStack.Pop();
                            string second = infixStack.Pop();
                            string expression = "(" + second + " " + currentElement + " " + first + ")";
                            infixStack.Push(expression);
                        }
                    }
                }
                if (infixStack.Count == 1)
                {
                    string finalExpression = infixStack.Pop();
                    finalExpression = finalExpression.Remove(0, 1);
                    finalExpression = finalExpression.Remove(finalExpression.Length - 1, 1);
                    infixSolutions.TryAdd(finalExpression);
                }
                else
                {
                    //invalid postfix expression
                }
            }
        }

        private string[] operators = new string[] { "+", "-", "*", "/" };
        private bool isOperator(string input)
        {
            return operators.Contains(input);
        }

        private string listToString(List<string> input)
        {
            StringBuilder output = new StringBuilder();

            foreach(string aString in input)
            {
                output.Append(aString);
            }
            return output.ToString();
        }
    }
}
