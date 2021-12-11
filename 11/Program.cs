// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\r\n")
    .Select(l => l.Select(c => int.Parse(c.ToString())).ToList())
    .ToList();

Func<(int i, int j), List<(int i, int j)>> getOctopusNeighbors = (point) =>
{
    var nbrs = new List<(int i, int j)>() {
        (point.i - 1, point.j - 1),
        (point.i - 1, point.j),
        (point.i - 1, point.j + 1),
        (point.i, point.j - 1),
        (point.i, point.j + 1),
        (point.i + 1, point.j - 1),
        (point.i + 1, point.j),
        (point.i + 1, point.j + 1)
    };

    return nbrs.Where(p => p.i >= 0 && p.i < input.Count() && p.j >= 0 && p.j < input.First().Count()).ToList();
};

Func<List<List<int>>, int> step = (stepInput) =>
{
    var flashing = new List<(int i, int j)>();
    for (int i = 0; i < stepInput.Count(); i++)
        for (int j = 0; j < stepInput.First().Count(); j++)
        {
            stepInput[i][j] += 1;
            if (stepInput[i][j] > 9)
            {
                flashing.Add((i, j));
            }
        }

    var flashed = new List<(int i, int j)>();
    while (flashing.Any())
    {
        flashed.AddRange(flashing);

        var flNbrs = flashing.SelectMany(octo => getOctopusNeighbors(octo)).ToList();
        flNbrs.ForEach(x => stepInput[x.i][x.j]++);

        flashing = new List<(int i, int j)>();
        for (int i = 0; i < stepInput.Count(); i++)
            for (int j = 0; j < stepInput.First().Count(); j++)
                if (stepInput[i][j] > 9 && !flashed.Contains((i, j)))
                {
                    flashing.Add((i, j));
                }
    }
    flashed.ForEach(octo => stepInput[octo.i][octo.j] = 0);

    return flashed.Count();
};

var sumFlashes = 0;
var p1 = input.Select(line => line.ToList()).ToList();
foreach (var _ in Enumerable.Range(0, 100))
{
    sumFlashes += step(p1);
}
Console.WriteLine($"P1: {sumFlashes}");

var p2 = input.Select(line => line.ToList()).ToList();
var iteration = 1;
while (true)
{
    if (step(p2) == p2.Count() * p2.First().Count())
    {
        break;
    }
    iteration++;
}

Console.WriteLine($"P2: {iteration}");
