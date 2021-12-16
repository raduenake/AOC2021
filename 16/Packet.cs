record Packet(Int64 Version, Int64 Typeid, Int64 Value, List<Packet> Children)
{
    public Int64 SumVer() => Version + Children.Sum(p => p.SumVer());

    public Int64 Eval() => Typeid switch
    {
        0 => Children.Sum(p => p.Eval()),
        1 => Children.Aggregate(1L, (acc, p) => acc * p.Eval()),
        2 => Children.Min(p => p.Eval()),
        3 => Children.Max(p => p.Eval()),
        4 => Value,
        5 => Children[0].Eval() > Children[1].Eval() ? 1 : 0,
        6 => Children[0].Eval() < Children[1].Eval() ? 1 : 0,
        7 => Children[0].Eval() == Children[1].Eval() ? 1 : 0,
        _ => throw new Exception()
    };
};