// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split(',')
    .Select(v => double.Parse(v))
    .ToImmutableList();

var minFuel = double.MaxValue;
for (var i = input.Min(); i <= input.Max(); i++)
{
    var fuel = input.Aggregate((double)0, (acc, crab) =>
    {
        var distance = Math.Abs(crab - i);
        // Part 1
        // acc += distance;
        // Part 2
        acc += Math.Floor(distance * (distance + 1) / 2);

        return acc;
    });
    if (fuel < minFuel)
    {
        minFuel = fuel;
    }
}

Console.WriteLine($"Answer: {minFuel}");
