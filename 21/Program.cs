var p1Start = 7;
var p2Start = 10;

var die = 0;
var rollDie = () =>
{
    return ++die % 100;
};

var p1Score = 0L;
var p2Score = 0L;
var p1PosZero = p1Start - 1;
var p2PosZero = p2Start - 1;
while (true)
{
    var p1Move = rollDie() + rollDie() + rollDie();
    p1PosZero = (p1PosZero + p1Move) % 10;
    p1Score += p1PosZero + 1;
    if (p1Score >= 1000)
    {
        break;
    }
    var p2Move = rollDie() + rollDie() + rollDie();
    p2PosZero = (p2PosZero + p2Move) % 10;
    p2Score += p2PosZero + 1;
    if (p2Score >= 1000)
    {
        break;
    }
}

Console.WriteLine($"P1: {Math.Min(p1Score, p2Score) * die}");

var gameHistory = new Dictionary<(int, int, int, int), (ulong, ulong)>();
var splitUnivPlay = (int p1Pos, int scoreP1, int p2Pos, int scoreP2) => (p1wins: 0UL, p2wins: 0UL);
splitUnivPlay = (int p1Pos, int scoreP1, int p2Pos, int scoreP2) =>
{
    if (scoreP1 >= 21)
    {
        return (1UL, 0UL);
    }
    if (scoreP2 >= 21)
    {
        return (0UL, 1UL);
    }
    if (gameHistory.ContainsKey((p1Pos, scoreP1, p2Pos, scoreP2)))
    {
        return gameHistory[(p1Pos, scoreP1, p2Pos, scoreP2)];
    }
    var result = (p1wins: 0UL, p2wins: 0UL);
    foreach (var d1 in Enumerable.Range(1, 3))
        foreach (var d2 in Enumerable.Range(1, 3))
            foreach (var d3 in Enumerable.Range(1, 3))
            {
                var splitP1Pos = (p1Pos + d1 + d2 + d3) % 10;
                var splitScoreP1 = scoreP1 + splitP1Pos + 1;

                var res = splitUnivPlay(p2Pos, scoreP2, splitP1Pos, splitScoreP1);
                // players are swapped, this is "confusing", but needed for the logic
                result = (result.p1wins + res.p2wins, result.p2wins + res.p1wins);
            }
    gameHistory.Add((p1Pos, scoreP1, p2Pos, scoreP2), result);
    return result;
};

p1PosZero = p1Start - 1;
p2PosZero = p2Start - 1;
var splitPlayResult = splitUnivPlay(p1PosZero, 0, p2PosZero, 0);
Console.WriteLine($"P2: {Math.Max(splitPlayResult.p1wins, splitPlayResult.p2wins)}");