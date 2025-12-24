using System.Collections.Generic;

namespace Octo.Challenge._2025.Week4
{
    class Edge
    {
        public string To { get; init; }
        public int AllowedParity { get; init; }
    }

    class Program
    {
        const int EVEN = 0;
        const int ODD = 1;

        static void Main()
        {
            string path = Path.Combine("Data", "OCTO-Coding-Challenge-2025-Week-4-Part-1-input.txt");

            var graph = BuildGraphFromDotFile(path);

            int numberOfShortestPaths = CountShortestPaths(graph, "AA", "ZZ");

            Console.WriteLine($"Number of shortest paths: {numberOfShortestPaths}");
        }

        static Dictionary<string, List<Edge>> BuildGraphFromDotFile(string filePath)
        {
            var graph = new Dictionary<string, List<Edge>>();

            void AddEdge(string a, string b, int parity)
            {
                graph.TryAdd(a, new List<Edge>());
                graph.TryAdd(b, new List<Edge>());

                graph[a].Add(new Edge { To = b, AllowedParity = parity });
                graph[b].Add(new Edge { To = a, AllowedParity = parity });
            }

            foreach (var rawLine in File.ReadLines(filePath))
            {
                string line = rawLine.Trim();

                if (!line.Contains("--"))
                    continue;

                var parts = line.Split("--", StringSplitOptions.TrimEntries);
                if (parts.Length != 2)
                    continue;

                string from = ExtractQuotedValue(parts[0]);
                string rest = parts[1];

                string to = ExtractQuotedValue(rest);

                int parity = rest.Contains("timestep=\"odd\"") ? ODD : EVEN;

                AddEdge(from, to, parity);
            }

            return graph;
        }

        static string ExtractQuotedValue(string text)
        {
            int firstQuote = text.IndexOf('"');
            int secondQuote = text.IndexOf('"', firstQuote + 1);

            if (firstQuote == -1 || secondQuote == -1)
                throw new FormatException($"Invalid DOT format: {text}");

            return text.Substring(firstQuote + 1, secondQuote - firstQuote - 1);
        }


        static int CountShortestPaths(
            Dictionary<string, List<Edge>> graph,
            string start,
            string target)
        {
            var queue = new Queue<(string node, int parity)>();

            var distance = new Dictionary<(string, int), int>();
            var ways = new Dictionary<(string, int), int>();

            queue.Enqueue((start, EVEN));
            distance[(start, EVEN)] = 0;
            ways[(start, EVEN)] = 1;

            int shortestDistance = int.MaxValue;

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                int currentDistance = distance[current];

                if (currentDistance > shortestDistance)
                    continue;

                if (current.node == target)
                {
                    shortestDistance = currentDistance;
                    continue;
                }

                int nextParity = 1 - current.parity;

                Relax(current, (current.node, nextParity), currentDistance + 1);

                foreach (var edge in graph[current.node])
                {
                    if (edge.AllowedParity == current.parity)
                    {
                        Relax(current, (edge.To, nextParity), currentDistance + 1);
                    }
                }
            }

            int totalWays = 0;
            foreach (int parity in new[] { EVEN, ODD })
            {
                var state = (target, parity);
                if (distance.TryGetValue(state, out int d) && d == shortestDistance)
                {
                    totalWays += ways[state];
                }
            }

            return totalWays;

            void Relax(
                (string node, int parity) fromState,
                (string node, int parity) toState,
                int newDistance)
            {
                if (!distance.ContainsKey(toState))
                {
                    distance[toState] = newDistance;
                    ways[toState] = ways[fromState];
                    queue.Enqueue(toState);
                }
                else if (distance[toState] == newDistance)
                {
                    ways[toState] += ways[fromState];
                }
            }
        }
    }
}
