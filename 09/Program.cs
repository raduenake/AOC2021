using System.Collections.Immutable;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\r\n")
    .Select(l => l.Select(c => int.Parse(c.ToString())).ToImmutableList())
    .ToImmutableList();

uint sumRisk = 0;
for (int i = 0; i < input.Count(); i++)
{
    for (int j = 0; j < input.First().Count(); j++)
    {
        var current = input[i][j];
        if (
            (i - 1 < 0 || current < input[i - 1][j]) &&
            (j - 1 < 0 || current < input[i][j - 1]) &&
            (i + 1 >= input.Count() || current < input[i + 1][j]) &&
            (j + 1 >= input.Count() || current < input[i][j + 1])
        )
        {
            sumRisk += (uint)current + 1;
        }
    }
}
Console.WriteLine($"P1: {sumRisk}");

List<(int i, int j)> visited = new List<(int, int)>();
Action<int, int> traverse = (i, j) => { };
traverse = (i, j) =>
{
    if (
        i < 0 || j < 0 ||
        i >= input.Count() ||
        j >= input.First().Count() ||
        visited.Contains((i, j)) ||
        input[i][j] == 9)
    {
        return;
    }

    visited.Add((i, j));
    traverse(i - 1, j);
    traverse(i + 1, j);
    traverse(i, j - 1);
    traverse(i, j + 1);
};

var history = new List<int>();
for (int i = 0; i < input.Count(); i++)
{
    for (int j = 0; j < input.First().Count(); j++)
    {
        if (!visited.Contains((i, j)) && input[i][j] != 9)
        {
            var prevCount = visited.Count();
            traverse(i, j);
            history.Add(visited.Count() - prevCount);
        }
    }
}
Console.WriteLine($"P1: {history.OrderBy(x => x).TakeLast(3).Aggregate(1L, (acc, n) => acc *= n)}");
