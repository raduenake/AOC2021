using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd().Split("\r\n")
    .ToList();

var pairs = input.Select(line => new Pair(JsonConvert.DeserializeObject<JToken>(line))).ToList();

var addLeft = (Pair? left, Pair? right) => { return new Pair(); };
addLeft = (Pair? left, Pair? right) =>
{
    if (left == null)
    {
        throw new Exception();
    }

    if (right == null)
    {
        return left;
    }

    if (left.PairValue != null)
    {
        left.PairValue += right.PairValue;
        return left;
    }

    return new Pair(addLeft(left.Left, right), left.Right);
};

var addRight = (Pair? left, Pair? right) => { return new Pair(); };
addRight = (Pair? left, Pair? right) =>
{
    if (left == null) 
    {
        throw new Exception();
    }

    if (right == null)
    {
        return left;
    }

    if (left.PairValue != null)
    {
        left.PairValue += right.PairValue;
        return left;
    }
    return new Pair(left.Left, addRight(left.Right, right));
};

var explode = (Pair current, int depth) => (exp: false, left: new Pair(), result: new Pair(), right: new Pair());
explode = (Pair current, int depth) =>
{
    if (current.PairValue != null)
    {
        return (false, null, current, null);
    }
    else if (current.Left == null || current.Right == null) {
        throw new Exception();
    }

    if (depth == 0)
    {
        return (true, current.Left, new Pair(0), current.Right);
    }

    var explodeLeft = explode(current.Left, depth - 1);
    if (explodeLeft.exp == true)
    {
        return (true, explodeLeft.left, new Pair(explodeLeft.result, addLeft(current.Right, explodeLeft.right)), null);
    }

    var explodeRight = explode(current.Right, depth - 1);
    if (explodeRight.exp == true)
    {
        return (true, null, new Pair(addRight(explodeLeft.result, explodeRight.left), explodeRight.result), explodeRight.right);
    }

    return (exp: false, left: null, result: current, right: null);
};

var split = (Pair current) => { return (split: false, result: new Pair()); };
split = (Pair current) =>
{
    if (current.PairValue != null)
    {
        if (current.PairValue.Value >= 10)
            return (true, new Pair(new Pair(current.PairValue / 2), new Pair((int)Math.Ceiling((double)current.PairValue.Value / 2))));
        return (false, current);
    }
    else if (current.Left == null || current.Right == null) {
        throw new Exception();
    }

    var splitLeft = split(current.Left);
    if (splitLeft.split)
    {
        return (true, new Pair(splitLeft.result, current.Right));
    }

    var splitRight = split(current.Right);
    return (split: splitRight.split, result: new Pair(splitLeft.result, splitRight.result));
};

var add = (Pair left, Pair right) =>
{
    var current = new Pair((Pair)left.Clone(), (Pair)right.Clone());
    while (true)
    {
        var expl = explode(current, 4);
        current = expl.result;
        if (expl.exp)
        {
            continue;
        }
        var spl = split(current);
        current = spl.result;
        if (!spl.split)
        {
            break;
        }
    }
    return current;
};

var sumPairs = pairs.Skip(1).Aggregate(pairs.First(), (acc, p) => acc = add(acc, p));
Console.WriteLine($"P1: {sumPairs.Magnitude}");

var mag =
    from s in pairs
    from s1 in pairs
    where s != s1
    select add(s, s1).Magnitude;

Console.WriteLine($"P2: {mag.Max()}");
