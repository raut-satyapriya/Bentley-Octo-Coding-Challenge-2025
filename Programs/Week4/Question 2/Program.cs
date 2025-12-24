namespace Octo.Challenge._2025.Week4.Problem2
{
    public class Program
    {
        static void Main(string[] args)
        {
            string filePath = Path.Combine("Data", "OCTO-Coding-Challenge-2025-Week-4-Part-2-input.txt");

            var edges = ReadGraphFromFile(filePath);
            BuildGraph(edges, out var graph, out var inDegree);

            var allOrders = new List<List<string>>();
            AllTopologicalSorts(graph, inDegree, new List<string>(), allOrders);

            Console.WriteLine($"Total valid sequences: {allOrders.Count}\n");

            foreach (var order in allOrders)
            {
                Console.WriteLine(string.Join(" -> ", order));
            }
        }

        static void AllTopologicalSorts(
    Dictionary<string, List<string>> graph,
    Dictionary<string, int> inDegree,
    List<string> current,
    List<List<string>> result)
        {
            bool foundCandidate = false;

            foreach (var node in inDegree.Keys.OrderBy(x => x))
            {
                if (inDegree[node] == 0 && !current.Contains(node))
                {
                    // Choose
                    current.Add(node);
                    foreach (var neighbor in graph[node])
                        inDegree[neighbor]--;

                    inDegree[node] = -1; // mark as used

                    // Explore
                    AllTopologicalSorts(graph, inDegree, current, result);

                    // Undo
                    inDegree[node] = 0;
                    foreach (var neighbor in graph[node])
                        inDegree[neighbor]++;

                    current.RemoveAt(current.Count - 1);
                    foundCandidate = true;
                }
            }

            // If no node can be picked, we have a complete ordering
            if (!foundCandidate && current.Count == graph.Count)
            {
                result.Add(new List<string>(current));
            }
        }

        static void BuildGraph(
    List<(string From, string To)> edges,
    out Dictionary<string, List<string>> graph,
    out Dictionary<string, int> inDegree)
        {
            graph = new Dictionary<string, List<string>>();
            inDegree = new Dictionary<string, int>();

            foreach (var (from, to) in edges)
            {
                if (!graph.ContainsKey(from))
                    graph[from] = new List<string>();
                if (!graph.ContainsKey(to))
                    graph[to] = new List<string>();

                graph[from].Add(to);

                inDegree.TryAdd(from, 0);
                inDegree[to] = inDegree.GetValueOrDefault(to) + 1;
            }
        }

        static List<(string From, string To)> ReadGraphFromFile(string filePath)
        {
            var edges = new List<(string, string)>();

            foreach (var line in File.ReadLines(filePath))
            {
                if (!line.Contains("->"))
                    continue;

                // Example line: "0" -> "3";
                var parts = line.Split("->", StringSplitOptions.TrimEntries);
                if (parts.Length != 2)
                    continue;

                string from = parts[0].Trim().Trim('"');
                string to = parts[1].Replace(";", "").Trim().Trim('"');

                edges.Add((from, to));
            }

            return edges;
        }


    }
}
