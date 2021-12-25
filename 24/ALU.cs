public class ALU
{
    public Dictionary<string, int> register = new()
    {
        ["w"] = 0,
        ["x"] = 0,
        ["y"] = 0,
        ["z"] = 0
    };

    public ALU()
    {
    }

    public ALU(Dictionary<string, int> _register)
    {
        foreach (var k in _register.Keys)
        {
            register[k] = _register[k];
        }
    }

    public int Z => register["z"];

    public void runInst(string instr, string input)
    {
        var split = instr.Split(" ");

        var op = split[0];
        var first = split[1];

        var second = string.Empty;
        int secondVal = 0;
        var secondIsReg = false;
        if (op != "inp")
        {
            second = split[2];
            secondVal = 0;
            secondIsReg = !int.TryParse(second, out secondVal);
        }

        switch (op)
        {
            case "inp":
                register[first] = int.Parse(input.ToString());
                break;
            case "add":
                register[first] += secondIsReg ? register[second] : secondVal;
                break;
            case "mul":
                register[first] *= secondIsReg ? register[second] : secondVal;
                break;
            case "div":
                register[first] /= secondIsReg ? register[second] : secondVal;
                break;
            case "mod":
                register[first] %= secondIsReg ? register[second] : secondVal;
                break;
            case "eql":
                register[first] = register[first] == (secondIsReg ? register[second] : secondVal) ? 1 : 0;
                break;
        }
    }
}