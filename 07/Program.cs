// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split(',')
    .Select(v => double.Parse(v))
    .ToImmutableList();

var p1 = Enumerable.Range((int)input.Min(), (int)(input.Max() - input.Min() + 1))
    .Select(pos =>
        input.Aggregate((double)0, (acc, crab) => acc += Math.Abs(crab - pos))
    ).Min();

var p2 = Enumerable.Range((int)input.Min(), (int)(input.Max() - input.Min() + 1))
    .Select(pos =>
        input.Aggregate((double)0, (acc, crab) =>
        {
            var distance = Math.Abs(crab - pos);
            return acc += Math.Floor(distance * (distance + 1) / 2);
        })
    ).Min();

Console.WriteLine($"P1: {p1}\nP2: {p2}");
