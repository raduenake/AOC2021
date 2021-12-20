var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\r\n")
    .ToList();

var inputEnhance = input.First()
    .Select((char c, int i) => (idx: i, ch: c.ToString()))
    .ToDictionary(x => x.idx, x => x.ch);

var inputImg = input.Skip(2)
    .SelectMany(
        (line, i) => line.Select((char c, int j) => (pos: (row: i, col: j), ch: c.ToString()))
    )
    .Where(posCh => posCh.ch == "#")
    .Select(posCh => posCh.pos)
    .ToHashSet();

var step = (HashSet<(int row, int col)> image, bool on) =>
{
    var res = new HashSet<(int, int)>();
    var rowLow = image.Min(k => k.row);
    var rowHigh = image.Max(k => k.row);
    var columnLow = image.Min(k => k.col);
    var columnHi = image.Max(k => k.col);

    for (int row = rowLow - 5; row < rowHigh + 10; row++)
    {
        for (int col = columnLow - 5; col < columnHi + 10; col++)
        {
            var enhIdx = 0;
            var bitPos = 8;
            foreach (var nbrRow in Enumerable.Range(-1, 3))
                foreach (var nbrCol in Enumerable.Range(-1, 3))
                {
                    var nbrPos = (row + nbrRow, col + nbrCol);
                    if (image.Contains(nbrPos) == on)
                    {
                        enhIdx += (int)Math.Pow(2, bitPos);
                    }
                    bitPos--;
                }

            if (inputEnhance.ContainsKey(enhIdx) && (inputEnhance[enhIdx] == "#") != on)
            {
                res.Add((row, col));
            }
        }
    }
    return res;
};

var enhancedImg = inputImg;
foreach (var x in Enumerable.Range(0, 50))
{
    if (x == 2)
    {
        Console.WriteLine($"P1: {enhancedImg.Count()}");
    }
    enhancedImg = step(enhancedImg, x % 2 == 0);
}
Console.WriteLine($"P2: {enhancedImg.Count()}");