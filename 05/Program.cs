// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\r\n")
    .Select(l =>
    {
        var split = l.Split(" -> ");
        var start = split[0].Split(",");
        var end = split[1].Split(",");
        return (
            start: (x: int.Parse(start[0]), y: int.Parse(start[1])),
            end: (x: int.Parse(end[0]), y: int.Parse(end[1]))
        );
    })
    .ToImmutableList();

var dict = new Dictionary<(int x, int y), uint>();
foreach (var l in input)
{
    if (l.start.x == l.end.x ||
        l.start.y == l.end.y ||
        // diags
        Math.Abs(l.start.x - l.end.x) == Math.Abs(l.start.y - l.end.y))
    {
        var point = l.start;
        while (true)
        {
            if (!dict.ContainsKey(point))
            {
                dict.Add(point, 1);
            }
            dict[point]++;

            if (point == l.end)
            {
                break;
            }

            point = Math.Sign(l.end.x - point.x) switch
            {
                -1 => (--point.x, point.y),
                1 => (++point.x, point.y),
                _ => point,
            };

            point = Math.Sign(l.end.y - point.y) switch
            {
                -1 => (point.x, --point.y),
                1 => (point.x, ++point.y),
                _ => point
            };
        }
    }
}

var overlaps = dict.Count(kv => kv.Value > 1);
Console.WriteLine($"Result: {overlaps}");
