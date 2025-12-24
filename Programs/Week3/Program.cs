using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Octo.Challenge._2025.Week3
{
    public class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string filePath = Path.Combine("Data", "OCTO-Coding-Challenge-2025-Week-3-Part-1-input.txt");

                var expression = File.ReadAllText(filePath);
                var tokens = expression.Split([' '], StringSplitOptions.RemoveEmptyEntries);
                int result = EvaluatePostfixExpression(tokens);
                Console.WriteLine($"Result: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static int EvaluatePostfixExpression(string[] tokens)
        {
            Stack<int> operandStack = new();

            foreach (string token in tokens)
            {
                if (IsOperator(token))
                {
                    int rightOperand = operandStack.Pop();
                    int leftOperand = operandStack.Pop();

                    int result = ApplyOperator(leftOperand, rightOperand, token);
                    operandStack.Push(result);
                }
                else
                {
                    operandStack.Push(int.Parse(token));
                }
            }

            return operandStack.Pop();
        }
        static bool IsOperator(string token)
        {
            return token == "+" || token == "-" || token == "*" || token == "/";
        }

        static int ApplyOperator(int left, int right, string op)
        {
            return op switch
            {
                "+" => left + right,
                "-" => left - right,
                "*" => left * right,
                "/" => left / right,
                _ => throw new InvalidOperationException($"Invalid operator: {op}")
            };
        }

    }

}
