// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;
using System.Diagnostics;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\r\n")
    .ToImmutableList();

var sw = new Stopwatch();
sw.Start();
// P1
sw.Stop();

Console.WriteLine($"[{sw.Elapsed}] P1: {0}");

sw.Reset();
sw.Start();
// P2
sw.Stop();

Console.WriteLine($"[{sw.Elapsed}] P1: {0}");
