// See https://aka.ms/new-console-template for more information
using System.Collections.Immutable;
using System.Diagnostics;

var file = System.IO.File.OpenText("input2.txt");
var input = file.ReadToEnd()
    .Split("\r\n")
    .ToImmutableList();

var sw = new Stopwatch();
sw.Start();

var draws = input.First()
    .Split(",")
    .Select(n => uint.Parse(n))
    .ToImmutableList();

var boards = new List<List<List<uint>>>();
var board = new List<List<uint>>();
foreach (var inputLine in input.Skip(2))
{
    if (string.IsNullOrWhiteSpace(inputLine))
    {
        boards.Add(board);
        board = new List<List<uint>>();
    }
    else
    {
        board.Add(
            inputLine
                .Split(" ")
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .Select(v => uint.Parse(v))
                .ToList()
            );
    }
}

Func<List<List<uint>>, List<uint>, bool> boardWins = (board, draws) =>
{
    var boardWins = false;
    for (int i = 0; i < board.First().Count(); i++)
    {
        boardWins = board.All(b => draws.Contains(b[i])) || board[i].All(b => draws.Contains(b));
        if (boardWins)
        {
            break;
        }
    }
    return boardWins;
};

Func<List<List<uint>>, List<uint>, uint> sumBoard = (board, roundDraws) =>
{
    return board.Aggregate((uint) 0, (acc, line) =>
    {
        return acc += (uint)line
            .Where(boardNumber => !roundDraws.Contains(boardNumber))
            .Sum(n => n);
    });
};

// P1
// start with at least 5 numbers
var round = 5;
var winners = new List<(int round, List<List<uint>> winnerBoard)>();
while (round < draws.Count() && boards.Any())
{
    var roundWinners = boards.Where(b => boardWins(b, draws.Take(round).ToList()));
    if (roundWinners != null)
    {
        winners.AddRange(roundWinners.Select(roundWinner => (round, roundWinner)));
        foreach (var roundWinner in roundWinners.ToList())
            boards.Remove(roundWinner);
    }
    round++;
}

// P1
Console.WriteLine($"[{sw.Elapsed}] P1: {sumBoard(winners.First().winnerBoard, draws.Take(winners.First().round).ToList()) * draws[winners.First().round - 1]}");

// P2
Console.WriteLine($"[{sw.Elapsed}] P2: {sumBoard(winners.Last().winnerBoard, draws.Take(winners.Last().round).ToList()) * draws[winners.Last().round - 1]}");

foreach (var winner in winners) {
    Console.WriteLine($"Round: [{winner.round}]; Result: [{sumBoard(winner.winnerBoard, draws.Take(winner.round).ToList()) * draws[winner.round - 1]}]");
}


sw.Stop();