using System.Collections.Immutable;
using System.Collections.Concurrent;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\r\n")
    .ToImmutableList();

var polimer = input.First();
var rules = input.Skip(2).Select(l =>
    {
        var s = l.Split(" -> ");
        return (Key: s[0], Value: s[1]);
    })
    .ToDictionary(r => "" + r.Key, r => r.Value);

var runPolymer = (int steps) =>
{
    var counts = new ConcurrentDictionary<string, ulong>();
    foreach (var polPair in polimer.Select(c => c).Zip(polimer.Select(c => c).Skip(1)))
    {
        counts.AddOrUpdate("" + polPair.First + polPair.Second, 1, (k, v) => v += 1);
    }
    counts.AddOrUpdate("" + polimer.Last(), 1, (k, v) => v += 1);

    foreach (var _ in Enumerable.Range(0, steps))
    {
        var countsStep = new ConcurrentDictionary<string, ulong>();
        Parallel.ForEach(counts.Keys, pair =>
        {
            var ppp = rules.ContainsKey(pair) switch
            {
                true => new List<string> { pair[0] + rules[pair], rules[pair] + pair[1] },
                false => new List<string> { pair }
            };
            Parallel.ForEach(ppp, p => countsStep.AddOrUpdate(p, counts[pair], (k, v) => v += counts[pair]));
        });
        counts = countsStep;
    }

    var frequency = counts
        // we only care, when counting frequency in the first char
        .Select(kv => (Key: kv.Key[0], Cnt: kv.Value))
        // we group
        .GroupBy(kv => kv.Key)
        // and aggregate values to get frequency
        .Select(gr => (Key: gr.Key, Freq: gr.Aggregate((ulong)0, (acc, g) => acc += g.Cnt)))
        // and keep the sorted set
        .OrderBy(gr => gr.Freq);

    return (LC: frequency.First(), MC: frequency.Last());
};

var p1 = runPolymer(10);
Console.WriteLine($"P1: {p1.MC.Freq - p1.LC.Freq}");

var p2 = runPolymer(40);
Console.WriteLine($"P2: {p2.MC.Freq - p2.LC.Freq}");
