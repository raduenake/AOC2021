// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\r\n")
    .Select(l =>
    {
        var s = l.Split("-");
        return (s[0], s[1]);
    })
    .ToImmutableList();

var adj = new Dictionary<string, List<string>>();
input.ForEach(v =>
{
    if (!adj.ContainsKey(v.Item1))
    {
        adj.Add(v.Item1, new List<string>() { v.Item2 });
    }
    else
    {
        adj[v.Item1].Add(v.Item2);
    }
    if (!adj.ContainsKey(v.Item2))
    {
        adj.Add(v.Item2, new List<string>() { v.Item1 });
    }
    else
    {
        adj[v.Item2].Add(v.Item1);
    }
});

var paths = (string current, List<string> seen) => { return 0; };
paths = (string current, List<string> seen) =>
{
    if (current == "end")
    {
        return 1;
    }
    if (current == current.ToLower() && seen.Contains(current))
    {
        return 0;
    }

    seen = (new List<string> { current }).Union(seen).ToList();
    var r = 0;
    foreach (var node in adj[current])
    {
        r += paths(node, seen);
    }
    return r;
};

var paths2 = (string current, List<string> seen, bool dup) => { return 0; };
paths2 = (string current, List<string> seen, bool dup) =>
{
    if (current == "end")
    {
        return 1;
    }
    if (current == "start" && seen.Any())
    {
        return 0;
    }
    if (current == current.ToLower() && seen.Contains(current))
    {
        if (!dup)
        {
            dup = true;
        }
        else
        {
            return 0;
        }
    }
    seen = (new List<string> { current }).Union(seen).ToList();

    var r = 0;
    foreach (var node in adj[current])
    {
        r += paths2(node, seen, dup);
    }
    return r;
};

var p1 = paths("start", new List<string>());
Console.WriteLine($"P1: {p1}");

var p2 = paths2("start", new List<string>(), false);
Console.WriteLine($"P2: {p2}");
