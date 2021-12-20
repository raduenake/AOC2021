using Newtonsoft.Json.Linq;

class Pair : ICloneable
{
    public Pair? Left;
    public Pair? Right;
    public int? PairValue;
    public int Magnitude
    {
        get
        {
            if (PairValue != null)
            {
                return PairValue.Value;
            }
            else if (Left != null && Right != null)
            {
                return 3 * Left.Magnitude + 2 * Right.Magnitude;
            }
            return 0;
        }
    }

    public Pair()
    {
    }

    public Pair(int _val)
    {
        PairValue = _val;
    }

    public Pair(Pair? _left, Pair? _right)
    {
        Left = _left;
        Right = _right;
    }

    public Pair(JToken? input)
    {
        if (input == null)
        {
            throw new Exception();
        }

        if (input.Count() == 2 && input.First != null && input.Last != null)
        {
            Left = new Pair(input.First);
            Right = new Pair(input.Last);
        }
        else
        {
            PairValue = input.Value<int>();
        }
    }

    public object Clone()
    {
        var p = new Pair(Left != null ? (Pair)Left.Clone() : null, Right != null ? (Pair)Right.Clone() : null);
        p.PairValue = PairValue;
        return p;
    }
}
