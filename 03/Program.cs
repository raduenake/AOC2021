// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;
using System.Diagnostics;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\r\n")
    .Select(l => l.Select(c => uint.Parse(c.ToString())).ToImmutableList())
    .ToImmutableList();
var sw = new Stopwatch();

//--- p1 ---
sw.Start();
Func<IEnumerable<IList<uint>>, IEnumerable<uint>, IList<(int zero, int one)>> countZeroesAndOnes = (list, bitsToConsider) =>
{
    return bitsToConsider.Select((val, idx) =>
        val switch
        {
            1 => list.Aggregate(
                (zero: 0, one: 0),
                (acc, line) =>
                    line[idx] switch
                    {
                        1 => (acc.zero, ++acc.one),
                        0 => (++acc.zero, acc.one),
                        _ => acc
                    }
                ),
            0 => (0, 0),
            _ => throw new Exception()
        }
    ).ToList();
};

var countZeroOnes = countZeroesAndOnes(input, Enumerable.Repeat<uint>(1, input.First().Count()));
var gammaBinary = countZeroOnes.Select(v => v.Item1 > v.Item2 ? 0 : 1);
var epsilonBinary = countZeroOnes.Select(v => v.Item1 > v.Item2 ? 1 : 0);

var gamma = Convert.ToInt32(string.Join("", gammaBinary), 2);
var epsilon = Convert.ToInt32(string.Join("", epsilonBinary), 2);
sw.Stop();

Console.WriteLine($"[{sw.Elapsed}] P1: {gamma * epsilon}");

//--- p2 ---
Func<IEnumerable<IList<uint>>, Func<uint, (int zero, int one), bool>, int> extractValue = (searchList, filter) =>
{
    var result = 0;
    var currentBit = 0;
    var bits = Enumerable.Repeat<uint>(0, input.First().Count()).ToList();
    
    while (true)
    {
        bits[currentBit] = 1;
        var currentBitCount = countZeroesAndOnes(searchList, bits);
        searchList = searchList.Where(line => filter(line[currentBit], currentBitCount[currentBit])).ToList();
        if (searchList.Count() == 1)
        {
            result = Convert.ToInt32(string.Join("", searchList.First()), 2);
            break;
        }
        else
        {
            bits[currentBit] = 0;
            currentBit++;
        }
    }
    return result;
};

sw.Reset();
sw.Start();
var o2Value = extractValue(input, (currentBitValue, currentBitCount) => currentBitValue == (currentBitCount.one >= currentBitCount.zero ? 1 : 0));
var co2Value = extractValue(input, (currentBitValue, currentBitCount) => currentBitValue == (currentBitCount.one < currentBitCount.zero ? 1 : 0));
sw.Stop();

Console.WriteLine($"[{sw.Elapsed}] P2: {o2Value * co2Value}");