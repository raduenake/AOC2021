// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\r\n")
    .ToImmutableList();

var left = new List<string> { "(", "[", "{", "<" };
var right = new List<string> { ")", "]", "}", ">" };

Dictionary<string, int> score = new()
{
    [")"] = 3,
    ["]"] = 57,
    ["}"] = 1197,
    [">"] = 25137
};

var p1Total = 0;
var p2Scores = new List<long>();
foreach (var l in input)
{
    var ok = true;
    var stack = new Stack<string>();
    foreach (var c in l.Select(c => c.ToString()))
    {
        if (left.Contains(c))
        {
            stack.Push(c);
        }
        else
        {
            var last = stack.Pop();
            if (right.IndexOf(c) != left.IndexOf(last))
            {
                p1Total += score[c];
                ok = false;
                break;
            }
        }
    }
    if (ok)
    {
        var s = 0L;
        while (stack.Any())
        {
            s *= 5;
            var pScore = left.IndexOf(stack.Pop());
            s += pScore + 1;
        }
        p2Scores.Add(s);
    }
}

Console.WriteLine($"P1: {p1Total}");

p2Scores.Sort();
Console.WriteLine($"P2: {p2Scores[(int)Math.Floor(1.0 * p2Scores.Count() / 2)]}");
