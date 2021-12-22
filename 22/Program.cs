using System.Collections.Immutable;
using System.Text.RegularExpressions;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\r\n")
    .Select(l =>
    {
        var rx = new Regex(@"^(x=.*),(y=.*),(z=.*)$");
        var rx2 = new Regex(@"^.=(.*)\.\.(.*)$");

        var split = l.Split(" ");
        var instr = split[0];

        var match = rx.Match(split[1]);
        var xm = rx2.Match(match.Groups[1].Value);
        var x = (min: int.Parse(xm.Groups[1].Value), max: int.Parse(xm.Groups[2].Value));
        var ym = rx2.Match(match.Groups[2].Value);
        var y = (min: int.Parse(ym.Groups[1].Value), max: int.Parse(ym.Groups[2].Value));
        var zm = rx2.Match(match.Groups[3].Value);
        var z = (min: int.Parse(zm.Groups[1].Value), max: int.Parse(zm.Groups[2].Value));
        return (instr: instr, x: x, y: y, z: z);
    })
    .ToImmutableList();

var onCubes = new Dictionary<(int, int, int), bool>();
foreach (var instruction in input)
{
    foreach (var x in Enumerable.Range(-50, 100))
        foreach (var y in Enumerable.Range(-50, 100))
            foreach (var z in Enumerable.Range(-50, 100))
            {
                if (instruction.x.min <= x && x <= instruction.x.max &&
                    instruction.y.min <= y && y <= instruction.y.max &&
                    instruction.z.min <= z && z <= instruction.z.max)
                {
                    if (!onCubes.ContainsKey((x, y, z)))
                    {
                        onCubes.Add((x, y, z), instruction.instr == "on" ? true : false);
                    }
                    else
                    {
                        onCubes[(x, y, z)] = (instruction.instr == "on" ? true : false);
                    }
                }
            }
}
Console.WriteLine($"P1: {onCubes.Count(kv => kv.Value == true)}");

var intersect = ((int min, int max) A, (int min, int max) B) =>
{
    var min = Math.Max(A.min, B.min);
    var max = Math.Min(A.max, B.max);
    return (min, max);
};

var totalRanges = new Dictionary<((int min, int max) x, (int min, int max) y, (int min, int max) z), int>();
foreach (var instruction in input)
{
    var rangesUpdate = new Dictionary<((int min, int max) x, (int min, int max) y, (int min, int max) z), int>();
    foreach (var existingRange in totalRanges)
    {
        var intersectX = intersect(instruction.x, existingRange.Key.x);
        var intersectY = intersect(instruction.y, existingRange.Key.y);
        var intersectZ = intersect(instruction.z, existingRange.Key.z);
        if (intersectX.min <= intersectX.max && intersectY.min <= intersectY.max && intersectZ.min <= intersectZ.max)
        {
            var key = (intersectX, intersectY, intersectZ);
            if (!rangesUpdate.ContainsKey(key))
            {
                rangesUpdate.Add(key, 0);
            }
            rangesUpdate[key] -= existingRange.Value;
        }
    }
    if (instruction.instr == "on")
    {
        var key = (instruction.x, instruction.y, instruction.z);
        if (!rangesUpdate.ContainsKey(key))
        {
            rangesUpdate.Add(key, 0);
        }
        rangesUpdate[key] += 1;
    }
    foreach (var update in rangesUpdate)
    {
        if (!totalRanges.ContainsKey(update.Key))
        {
            totalRanges.Add(update.Key, update.Value);
        }
        else
        {
            totalRanges[update.Key] += update.Value;
        }
    }
}

var totalOn = 0UL;
totalOn = totalRanges.Aggregate(0UL, (acc, it) =>
    acc + (ulong)((ulong)it.Value *
        ((ulong)(it.Key.x.max - it.Key.x.min + 1)) *
        ((ulong)(it.Key.y.max - it.Key.y.min + 1)) *
        ((ulong)(it.Key.z.max - it.Key.z.min + 1)))
);
Console.WriteLine($"P2: {totalOn}");