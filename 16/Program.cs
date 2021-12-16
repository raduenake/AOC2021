var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .SelectMany(c => Convert.ToString(Convert.ToInt64(c.ToString(), 16), 2).PadLeft(4, '0').Select(c1 => c1.ToString()))
    .ToList();

var readBits = (List<string> data, int bitCount, ref int idx) =>
{
    var s = string.Join("", data.Skip(idx).Take(bitCount));
    idx += bitCount;
    return s;
};

var readInt64 = (List<string> data, int bitCount, ref int idx) =>
{
    return Convert.ToInt64(readBits(data, bitCount, ref idx), 2);
};

var parse = (List<string> data) => { return (Pck: new Packet(0, 0, 0, new()), Off: 0); };
parse = (List<string> data) =>
{
    var index = 0;
    var children = new List<Packet>();

    var version = readInt64(data, 3, ref index);
    var typeId = readInt64(data, 3, ref index);
    if (typeId == 4)
    {
        var takeFiveBits = data.Skip(index).Chunk(5);
        var valueString = "";
        foreach (var fiveBits in takeFiveBits)
        {
            valueString += string.Join("", fiveBits.Skip(1));
            index += 5;
            if (fiveBits.First() == "0")
            {
                break;
            }
        }

        var valueInt64 = Convert.ToInt64(valueString, 2);
        var p = new Packet(version, typeId, valueInt64, new());
        return (p, index);
    }
    else
    {
        var lenTypeId = readInt64(data, 1, ref index);
        switch (lenTypeId)
        {
            case 0:
                var subLen = readInt64(data, 15, ref index);
                while (subLen > 0)
                {
                    var p_ = parse(data.Skip(index).Take((int)subLen).ToList());
                    children.Add(p_.Pck);
                    index += p_.Off;
                    subLen -= p_.Off;
                }
                break;
            case 1:
                var subCnt = readInt64(data, 11, ref index);
                foreach (var _ in Enumerable.Range(0, (int)subCnt))
                {
                    var p_ = parse(data.Skip(index).ToList());
                    children.Add(p_.Pck);
                    index += p_.Off;
                }
                break;
        }
    }
    var pp = new Packet(version, typeId, 0, children);
    return (pp, index);
};

var p = parse(input);

Console.WriteLine($"P1:{p.Pck.SumVer()}");
Console.WriteLine($"P2:{p.Pck.Eval()}");
