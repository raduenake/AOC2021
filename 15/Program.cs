using System.Collections.Immutable;
using System.Collections.Concurrent;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\r\n")
    .Select(l => l.Select(c => int.Parse(c.ToString())))
    .ToImmutableList();

var cavern = input
    .SelectMany((line, i) => line.Select((val, j) => (pos: (x: i, y: j), val)))
    .ToImmutableDictionary(kv => kv.pos, kv => kv.val);

var maxX = input.Count();
var maxY = input.First().Count();

var shortDist = (int w) => { return 0; };
shortDist = (int w) =>
{
    var prioQueue = new PriorityQueue<(int x, int y), int>();
    prioQueue.Enqueue((0, 0), 0);

    var seen = new HashSet<(int x, int y)>();

    var current = (x: 0, y: 0);
    var risk = 0;
    while (prioQueue.TryDequeue(out current, out risk))
    {
        if (current.x == w * maxX - 1 && current.y == w * maxY - 1)
        {
            return risk;
        }

        var nbrCoords = new List<(int x, int y)> { (0, 1), (0, -1), (1, 0), (-1, 0) };
        foreach (var nbrCoord in nbrCoords)
        {
            var nbrPosition = (x: current.x + nbrCoord.x, y: current.y + nbrCoord.y);
            if (nbrPosition.x < 0 || nbrPosition.y < 0 || nbrPosition.x >= w * maxX || nbrPosition.y >= w * maxY)
            {
                continue;
            }

            // transpose cavern 
            var xOrig = 0;
            var x_ = Math.DivRem(nbrPosition.x, input.Count(), out xOrig);
            var yOrig = 0;
            var y_ = Math.DivRem(nbrPosition.y, input.First().Count(), out yOrig);
            var cavernRisk = ((cavern[(xOrig, yOrig)] + x_ + y_) - 1) % 9 + 1;

            if (!seen.Contains(nbrPosition))
            {
                seen.Add(nbrPosition);
                prioQueue.Enqueue(nbrPosition, risk + cavernRisk);
            }
        }
    }
    return 0;
};

var p1 = shortDist(1);
Console.WriteLine($"P1:{p1}");

var p2 = shortDist(5);
Console.WriteLine($"P2:{p2}");
