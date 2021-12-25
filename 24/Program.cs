using System.Collections.Immutable;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\r\n")
    .ToImmutableList();

var addX = new List<int>();
var divZ = new List<int>();
var addY = new List<int>();
foreach (var instr in input.Select((l, ix) => (pos: ix, data: l)))
{
    if (instr.data.Contains("add x ") && !instr.data.Contains("add x z"))
    {
        addX.Add(int.Parse(instr.data.Split(" ")[2]));
    }
    if (instr.data.Contains("div z"))
    {
        divZ.Add(int.Parse(instr.data.Split(" ")[2]));
    }
    if (instr.data.Contains("add y") && instr.pos % 18 == 15)
    {
        addY.Add(int.Parse(instr.data.Split(" ")[2]));
    }
}

var zMax = divZ.Select((_, idx) =>
    {
        var pow = divZ
            .Select((val, powIdx) => (val, powIdx))
            .Aggregate(0, (acc, x) => acc += (x.powIdx >= idx && x.val == 26) ? 1 : 0);
        return Math.Pow(26, pow);
    }).ToList();

// ALU reduced ...
var alu2 = (int digit, int zReg, int wReg) =>
{
    var x = addX[digit] + (zReg % 26);
    zReg = zReg / divZ[digit];
    if (x != wReg)
    {
        zReg *= 26;
        zReg += wReg + addY[digit];
    }
    return zReg;
};

var alg = (int digit, int z) =>
{
    return Enumerable.Empty<string>();
};
alg = (int digit, int zRegister) =>
{
    var result = new List<string>();
    if (digit == 14)
    {
        if (zRegister == 0)
        {
            result.Add(string.Empty);
        }
        return result;
    }

    if (zRegister > zMax[digit])
    {
        return result;
    }

    var digitOptions = Enumerable.Range(1, 9);
    var futureX = addX[digit] + zRegister % 26;
    if (Enumerable.Range(1, 9).Contains(futureX))
    {
        digitOptions = new[] { futureX };
    }

    foreach (var digitOption in digitOptions)
    {
        // too slow ...
        // var instrStart = digit * 18;
        // var currentDigit = digit;
        // var newAlu = new ALU(alu.register);
        // for (var instrIdx = instrStart; instrIdx < instrStart + 18; instrIdx++)
        // {
        //     newAlu.runInst(input[instrIdx], $"{digitOption}");
        // }

        var zNext = alu2(digit, zRegister, digitOption);
        var algSols = alg(digit + 1, zNext);
        result.AddRange(algSols.Select(s => $"{digitOption}{s}"));
    }
    return result;
};

var sols = alg(0, 0);
sols = sols.OrderBy(s => s).ToList();

Console.WriteLine($"min: {sols.Min()}; max: {sols.Max()}");