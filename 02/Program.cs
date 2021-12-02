// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\r\n")
    .Select(l => new { dir = l.Split(' ')[0], amt = int.Parse(l.Split(' ')[1]) })
    .ToArray();

var sw = new Stopwatch();

sw.Start();
var part1 = input.Aggregate((
    pos: 0, depth: 0),
    (acc, instr) => instr.dir switch
    {
        "forward" => (acc.pos += instr.amt, acc.depth),
        "down" => (acc.pos, acc.depth += instr.amt),
        "up" => (acc.pos, acc.depth -= instr.amt),
        _ => acc
    },
    result => result.pos * result.depth
);
sw.Stop();
Console.WriteLine($"[{sw.Elapsed}] P1: {part1}");

sw.Reset();
sw.Start();
var part2 = input.Aggregate(
    (pos: 0, depth: 0, aim: 0),
    (acc, insr) => insr.dir switch
    {
        "forward" => (acc.pos += insr.amt, acc.depth += insr.amt * acc.aim, acc.aim),
        "down" => (acc.pos, acc.depth, acc.aim += insr.amt),
        "up" => (acc.pos, acc.depth, acc.aim -= insr.amt),
        _ => acc
    },
    result => result.pos * result.depth
);
sw.Stop();

Console.WriteLine($"[{sw.Elapsed}] P2: {part2}");