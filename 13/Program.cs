using System.Collections.Immutable;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\r\n")
    .ToImmutableList();

var inputPoints = new List<(int x, int y)>();
var inputFolds = new List<(string dir, int amount)>();

var readFolds = false;
foreach (var line in input)
{
    if (string.IsNullOrEmpty(line))
    {
        readFolds = true;
        continue;
    }
    if (readFolds)
    {
        var foldDir = line[line.IndexOf('=') - 1].ToString();
        var foldAmount = int.Parse(line.Substring(line.IndexOf('=') + 1));
        inputFolds.Add((foldDir, foldAmount));
    }
    else
    {
        var coordSplit = line.Split(",");
        inputPoints.Add((int.Parse(coordSplit[0]), int.Parse(coordSplit[1])));
    }
}

var foldTransform = (List<(int x, int y)> dots, (string dir, int amount) fold) =>
{
    return fold.dir switch
    {
        "x" => dots.Select(p => (Math.Min(p.x, 2 * fold.amount - p.x), p.y)).Distinct().ToList(),
        "y" => dots.Select(p => (p.x, Math.Min(p.y, 2 * fold.amount - p.y))).Distinct().ToList(),
        _ => throw new Exception()
    };
};

var p1 = inputPoints.ToList();
p1 = foldTransform(p1, inputFolds.First());
Console.WriteLine($"{p1.Count()}");

var p2 = inputPoints.ToList();
foreach (var f in inputFolds)
{
    p2 = foldTransform(p2, f);
}

var maxX = p2.Max(p => p.x);
var maxY = p2.Max(p => p.y);
for (int y = 0; y <= maxY; y++)
{
    var outLine = string.Empty;
    for (int x = 0; x <= maxX; x++)
    {
        outLine += p2.Contains((x, y)) switch
        {
            true => "#",
            false => " "
        };
    }
    Console.WriteLine(outLine);
}
