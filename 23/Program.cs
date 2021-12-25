Dictionary<char, int> goalPositions = new()
{
    ['A'] = 2,
    ['B'] = 4,
    ['C'] = 6,
    ['D'] = 8
};

Dictionary<char, int> moveCost = new()
{
    ['A'] = 1,
    ['B'] = 10,
    ['C'] = 100,
    ['D'] = 1000
};

var isReachable = (List<string> board, int start, int dest) =>
{
    var min = Math.Min(start, dest);
    var max = Math.Max(start, dest);
    for (int i = min; i < max + 1; i++)
    {
        if (i == start || goalPositions.ContainsValue(i))
        {
            continue;
        }
        if (board[i] != ".")
        {
            return false;
        }
    }
    return true;
};

var roomContainsOnlyGoal = (List<string> board, char piece, int dest) =>
{
    return board[dest].All(c => c == '.' || c == piece);
};

var popFromRoom = (string room) =>
{
    var piece = '\0';
    var newRoom = string.Empty;
    var dist = 0;
    foreach (var c in room)
    {
        if (c == '.')
        {
            newRoom += c;
            dist++;
        }
        else if (piece == '\0')
        {
            piece = c;
            dist++;
            newRoom += '.';
        }
        else
        {
            newRoom += c;
        }
    }
    return (piece: piece, distance: room.Length == 1 ? 0 : dist, room: newRoom);
};

var possiblePositionDestinations = (List<string> board, int position) =>
{
    var roomPop = popFromRoom(board[position]);
    var piece = roomPop.piece;
    if (!goalPositions.ContainsValue(position))
    {
        if (isReachable(board, position, goalPositions[piece]) &&
            roomContainsOnlyGoal(board, piece, goalPositions[piece]))
        {
            return new[] { goalPositions[piece] };
        }
        return Enumerable.Empty<int>();
    }

    if (position == goalPositions[piece] &&
        roomContainsOnlyGoal(board, piece, position))
    {
        return Enumerable.Empty<int>();
    }

    var possibleDestinations = new List<int>();
    foreach (var destination in Enumerable.Range(0, board.Count()))
    {
        if (destination == position ||
            (goalPositions.ContainsValue(destination) && goalPositions[piece] != destination) ||
            (goalPositions[piece] == destination && !roomContainsOnlyGoal(board, piece, destination)))
        {
            continue;
        }
        if (isReachable(board, position, destination))
        {
            possibleDestinations.Add(destination);
        }
    }
    return possibleDestinations;
};

var addToRoom = (char piece, string room) =>
{
    var roomCharList = room.Select(c => c).ToList();
    var distance = room.Count(c => c == '.');
    roomCharList[distance - 1] = piece;
    return (room: string.Join("", roomCharList), distance: room.Length == 1 ? 0 : distance);
};

var move = (List<string> board, int position, int dest) =>
{
    var newBoard = new List<string>(board);
    var distance = 0;

    var boardPosUpdate = popFromRoom(board[position]);
    newBoard[position] = boardPosUpdate.room;

    distance += boardPosUpdate.distance;
    distance += Math.Abs(position - dest);

    var boardDestUpdate = addToRoom(boardPosUpdate.piece, board[dest]);
    newBoard[dest] = boardDestUpdate.room;
    distance += boardDestUpdate.distance;

    return (newBoard, distance * moveCost[boardPosUpdate.piece]);
};

var solve = (List<string> board) =>
{
    var states = new Dictionary<string, int>();
    states.Add(string.Join("", board), 0);

    var stack = new Stack<List<string>>(new[] { board });
    while (stack.Any())
    {
        var currentBoard = stack.Pop();
        for (int pos = 0; pos < currentBoard.Count(); pos++)
        {
            if (popFromRoom(currentBoard[pos]).piece == '\0')
            {
                continue;
            }
            var possibleDestinations = possiblePositionDestinations(currentBoard, pos);
            foreach (var dest in possibleDestinations)
            {
                var moveResult = move(currentBoard, pos, dest);
                var newCost = states[string.Join("", currentBoard)] + moveResult.Item2;
                var cost = states.ContainsKey(string.Join("", moveResult.newBoard)) ? states[string.Join("", moveResult.newBoard)] : int.MaxValue;
                if (newCost < cost)
                {
                    if (!states.ContainsKey(string.Join("", moveResult.newBoard)))
                    {
                        states.Add(string.Join("", moveResult.newBoard), int.MaxValue);
                    }
                    states[string.Join("", moveResult.newBoard)] = newCost;
                    stack.Push(moveResult.newBoard);
                }
            }
        }
    }
    return states;
};

var part1Board = new[] { ".", ".", "DC", ".", "AA", ".", "CB", ".", "DB", ".", "." };
var part1ResultSet = solve(part1Board.ToList());

var result = part1ResultSet[string.Join("", new[] { ".", ".", "AA", ".", "BB", ".", "CC", ".", "DD", ".", "." })];
Console.WriteLine($"{result}");

var part2Board = new[] { ".", ".", "DDDC", ".", "ACBA", ".", "CBAB", ".", "DACB", ".", "." };
var part2ResultSet = solve(part2Board.ToList());

result = part2ResultSet[string.Join("", new[] { ".", ".", "AAAA", ".", "BBBB", ".", "CCCC", ".", "DDDD", ".", "." })];
Console.WriteLine($"{result}");