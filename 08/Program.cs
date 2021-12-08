// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\r\n")
    .ToImmutableList();

Dictionary<int, List<string>> p1 = new()
{
    // one
    [2] = new List<string>(),
    //seven
    [3] = new List<string>(),
    //four
    [4] = new List<string>(),
    // two, three, five 
    [5] = new List<string>(),
    // zero, six, nine
    [6] = new List<string>(),
    //eight
    [7] = new List<string>()
};

foreach (var digit in input.SelectMany(inputLine => inputLine.Split("|")[1].Trim().Split(" ")))
{
    p1[digit.Length].Add(digit);
}
Console.WriteLine($"P1: {p1[2].Count() + p1[3].Count() + p1[4].Count() + p1[7].Count()}");

/*
 aaaa
b    c
b    c
 dddd
e    f
e    f
 gggg
*/
int sum = 0;
foreach (var line in input)
{
    var lineSplit = line.Split("|");
    var segments = lineSplit[0].Trim().Split(" ");
    // one, four, seven, eight are 'inferred' directly
    Dictionary<int, string> digits = new()
    {
        [0] = string.Empty,
        // one 
        [1] = segments.Single(seg => seg.Length == 2),
        [2] = string.Empty,
        [3] = string.Empty,
        // four
        [4] = segments.Single(seg => seg.Length == 4),
        [5] = string.Empty,
        [6] = string.Empty,
        // seven
        [7] = segments.Single(seg => seg.Length == 3),
        // eight
        [8] = segments.Single(seg => seg.Length == 7),
        [9] = string.Empty
    };

    // nine has one more segment besides fours and seven
    digits[9] = segments.Single(seg => seg.Length == 6 && seg.Except(digits[7]).Except(digits[4]).Count() == 1);
    // six is not nine, and when compared to one, the latter has one more segment
    digits[6] = segments.Single(seg => seg.Length == 6 && seg != digits[9] && digits[1].Except(seg).Count() == 1);
    // zero is the remaining six segment that is not six and nine
    digits[0] = segments.Single(seg => seg.Length == 6 && seg != digits[9] && seg != digits[6]);

    // ee bar is the "difference" between eight and nine
    var eeBar = digits[8].Except(digits[9]).Single();
    // cc bar is the "difference" between eight and six
    var ccBar = digits[8].Except(digits[6]).Single();
    // ff bar is the "difference" between one bar and the ccBar
    var ffBar = digits[1].Except(new[] { ccBar }).Single();

    // we can determine five with ccBar and eeBar
    digits[5] = segments.Single(seg => seg.Length == 5 && !seg.Contains(ccBar) && !seg.Contains(eeBar));
    // two can be determined now
    digits[2] = segments.Single(seg => seg.Length == 5 && seg != digits[5] && seg.Contains(ccBar) && !seg.Contains(ffBar));
    // the remaining one is three
    digits[3] = segments.Single(seg => seg.Length == 5 && seg != digits[5] && seg != digits[2]);

    sum += lineSplit[1].Trim().Split(" ")
        .Select(s => 
            digits.Where(kv => kv.Value.Length == s.Length && !kv.Value.Except(s).Any())
            .Single()
            .Key
        ).Aggregate(0, (acc, digit) => acc * 10 + digit);
}

Console.WriteLine($"P2: {sum}");