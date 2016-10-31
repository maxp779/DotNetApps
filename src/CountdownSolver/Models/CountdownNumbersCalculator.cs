﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CountdownSolver.Models
{
    public class CountdownNumbersCalculator
    {
        private static List<int> values;
        private static char[] ops = { '+', '-', '/', '*' };

        public static HashSet<string> calculate(List<int> numbers, int targetNumber, int threadCount = 1)
        {
            values = numbers;
            HashSet<string> output = solve(targetNumber);
            return output;
        }

        private static HashSet<string> solve(int target)
        {
            HashSet<string> ans = new HashSet<string>();
            HashSet<int> set = new HashSet<int>();
            List<char> operands = new List<char>();

            foreach (int n in values)
            {
                set.Add(n);
                if (n == target)
                {
                    ans.Append('\n' + n.ToString());
                }
                looper(n, target, ans, set, operands);
                set.Remove(n);
            }
            return ans;
        }

        private static void looper(int total, int target, HashSet<string> ans,
                HashSet<int> set, List<char> operands)
        {
            foreach (int n in values)
            {
                if (set.Add(n))
                {
                    foreach (char op in ops)
                    {
                        operands.Add(op);
                        int cul = chrToValue(total, op, n);
                        if (cul == target)
                        {
                            string exp = opAlgoFormat(set, operands);
                            ans.Add(exp);
                        }
                        looper(cul, target, ans, set, operands);
                        operands.RemoveAt(operands.Count - 1);
                    }
                    set.Remove(n);
                }
            }
        }

        private static String opAlgoFormat(HashSet<int> set,
                List<char> values)
        {
            IEnumerator<int> iter = set.GetEnumerator();
            iter.MoveNext();
            string toString = Convert.ToString(iter.Current);
            int t = 0;
            char prev = ' ';
            while (iter.MoveNext())
            {
                int v = iter.Current;
                if ((values.ElementAt(t) == '/' || values.ElementAt(t) == '*') && (prev == '+' || prev == '-'))
                {
                    toString = "(" + toString + ")";
                }

                toString += " " + Convert.ToString(values.ElementAt(t)) + " " + Convert.ToString(v);
                prev = values.ElementAt(t++);
            }
            return toString;
        }

        private static int chrToValue(int n1, char op, int n2)
        {
            switch (op)
            {
                case '+':
                    return n1 + n2;
                case '-':
                    return n1 - n2;
                case '/':
                    if (n1 % n2 == 0)
                        return n1 / n2;
                    return int.MaxValue;
                case '*':
                    return n1 * n2;
            }
            throw new System.ArgumentException();
        }
    }
}
