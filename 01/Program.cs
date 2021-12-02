// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\r\n")
    .Select(l => int.Parse(l))
    .ToImmutableList();

// create pairs ...
// with a "skew" [0,1]; [1,2]; etc...
var part1 = input.Take(input.Count() - 1).Zip(input.Skip(1));
Console.WriteLine($"P1: {part1.Count(zip => zip.First < zip.Second)}");

// create pairs ...
// a+b+c < b+c+d === a < d
// "skip" the intermediaries and no need for "sums"
var part2 = input.Take(input.Count() - 3).Zip(input.Skip(3));
Console.WriteLine($"P2: {part2.Count(zip => zip.First < zip.Second)}");
