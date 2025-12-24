using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Numerics;

namespace Octo.Challenge._2025.Week2
{
    class Program
    {
        static void Main()
        {
            try
            {
                string filePath = Path.Combine("Data", "OCTO-Coding-Challenge-2025-Week-2-Part-1-input.txt");

                var pairs = LoadData(filePath);

                int searchMin = 0;
                    
                int searchMax = Int32.MaxValue;

                int secretNumber = FindSecretNumber(pairs, searchMin, searchMax);

                Console.WriteLine($"{secretNumber} (binary: {Convert.ToString(secretNumber, 2)})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static Dictionary<int, int> LoadData(string file)
        {
            return File.ReadLines(file)
                       .Select(line => line.Split("->"))
                       .Where(parts => parts.Length == 2 &&
                                       int.TryParse(parts[0], out _) &&
                                       int.TryParse(parts[1], out _))
                       .ToDictionary(
                            p => int.Parse(p[0]),
                            p => int.Parse(p[1]));
        }

        static int HammingDistance(int a, int b)
        {
            return BitOperations.PopCount((uint)(a ^ b));
        }

        static bool IsValid(int candidate, Dictionary<int, int> pairs)
        {
            foreach (var (number, expectedDist) in pairs)
            {
                if (HammingDistance(number, candidate) != expectedDist)
                    return false;
            }
            return true;
        }

        static int FindSecretNumber(Dictionary<int, int> pairs, int searchMin, int searchMax)
        {
            for (int candidate = searchMin; candidate <= searchMax; candidate++)
            {
                if (IsValid(candidate, pairs))
                    return candidate;
            }

            return -1;
        }
    }
}
