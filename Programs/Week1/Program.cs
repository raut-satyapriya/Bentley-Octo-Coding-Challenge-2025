using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Octo.Challenge._2025.Week1
{
    class Program
    {
        static void Main()
        {
            string filePath = Path.Combine("Data", "OCTO-Coding-Challenge-2025-Week-1-Part-1-input.txt");

            var (mean, stddev) = CalculateMeanAndStdDev(filePath);

            double lower = mean - 2 * stddev;
            double upper = mean + 2 * stddev;

            double sumAwayFromMean = Sum(filePath, lower, upper);

            Console.WriteLine($"Sum greater than two standard deviations away from the mean: {sumAwayFromMean}");
        }

        static (double, double) CalculateMeanAndStdDev(string file)
        {
            long count = 0;
            double meanTemp = 0;
            double m2 = 0;

            foreach (var line in File.ReadLines(file))
            {
                if (double.TryParse(line, out double x))
                {
                    count++;
                    double delta = x - meanTemp;
                    meanTemp += delta / count;
                    m2 += delta * (x - meanTemp);
                }
            }

            var mean = meanTemp;
            var stddev = Math.Sqrt(m2 / count);

            return (mean, stddev);
        }

        static double Sum(string file, double lower, double upper)
        {
            double sum = 0;

            foreach (var line in File.ReadLines(file))
            {
                if (double.TryParse(line, out double x))
                {
                    if (x < lower || x > upper)
                        sum += x;
                }
            }

            return sum;
        }
    }
}
