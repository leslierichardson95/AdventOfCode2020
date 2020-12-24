using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Day18
{
    class Program
    {
        static List<string> postfixExpressions;
        static void Main(string[] args)
        {
            string[] expressions = File.ReadAllLines("C:/Users/lerich/OneDrive - Microsoft/AdventOfCode/Day18/input.txt");
            for (int i = 0; i < expressions.Length; i++)
            {
                expressions[i] = expressions[i].Replace(" ", "");
            }
            ConvertToPostfix(expressions);
            Console.WriteLine($"Total Sum: {TotalSum()}");
        }

        static void ConvertToPostfix(string[] expressions)
        {
            Queue<char> outputBuffer = new Queue<char>();
            Stack<char> operators = new Stack<char>();
            postfixExpressions = new List<string>();

            Dictionary<char, int> opPrecedents = new Dictionary<char, int>(){ { '+', 1 }, { '*', 0 } };

            foreach (string expression in expressions)
            {
                for (int i = 0; i < expression.Length; i++)
                {
                    char token = expression[i];
                    if (token == '*' || token == '+')
                    {
                        if (operators.Count != 0 && (operators.Peek() != '(' && operators.Peek() != ')'))
                        {
                            int value;
                            opPrecedents.TryGetValue(operators.Peek(), out value);
                            while (operators.Count != 0 && (opPrecedents[token] <= value) && 
                                (operators.Peek() != '(' && operators.Peek() != ')'))
                            {
                                char op = operators.Pop();
                                outputBuffer.Enqueue(op);
                                if (operators.Count != 0) opPrecedents.TryGetValue(operators.Peek(), out value);
                            }  
                        }
                        operators.Push(token);
                    }
                    else if (token == '(') operators.Push(token);
                    else if (token == ')')
                    {
                        while (token != '(')
                        {
                            token = operators.Pop();
                            if (token == '(') break;
                            outputBuffer.Enqueue(token);
                        }
                    }
                    else outputBuffer.Enqueue(token);
                }

                while (operators.Count != 0) outputBuffer.Enqueue(operators.Pop());

                string postfix = "";
                int length = outputBuffer.Count;
                for (int i = 0; i < length; i++)
                {
                    postfix += outputBuffer.Dequeue();
                }
                postfixExpressions.Add(postfix);
            }
        }

        static long TotalSum()
        {
            Stack<long> operands = new Stack<long>();
            long sum = 0;

            foreach (string postFix in postfixExpressions)
            {
                for (int i = 0; i < postFix.Length; i++)
                {
                    char token = postFix[i];
                    if (token != '+' && token != '*')
                    {
                        operands.Push(long.Parse(postFix[i].ToString()));
                    }
                    else
                    {
                        long operand1 = operands.Pop();
                        long operand2 = operands.Pop();

                        if (token == '+')
                        {
                            operands.Push(operand1 + operand2);
                        }
                        else if (token == '*')
                        {
                            operands.Push(operand1 * operand2);
                        }
                    }
                }
                sum += operands.Pop();
            }
           
            return sum;
        }
    }
}
