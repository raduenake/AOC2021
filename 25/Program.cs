using System.Collections.Immutable;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\r\n")
    .ToImmutableList();

var height = input.Count();
var length = input.First().Count();

var dirs = input.SelectMany((l, dwn) => l.Select((c, rgt) => (cucumber: c, pos: (dwn, rgt))));

var step = (HashSet<(int dwn,int rgt)> right, HashSet<(int dwn,int rgt)> down) => {
    var changes = 0;
    var newRight = new HashSet<(int, int)>();
    var newDown = new HashSet<(int, int)>();

    foreach (var rightPos in right) {
        var deltaRgt = rightPos.rgt == length - 1 ? 0 : rightPos.rgt+1;
        var newRightPos = (x: rightPos.dwn,y: deltaRgt);
        if (right.Contains(newRightPos) || down.Contains(newRightPos)) {
            newRight.Add(rightPos);
        }
        else {
            changes++;
            newRight.Add(newRightPos);
        }
    }

    foreach (var downPos in down) {
        var deltaDwn = downPos.dwn == height - 1 ? 0 : downPos.dwn+1;
        var newDownPos = (x: deltaDwn, downPos.rgt);
        if (down.Contains(newDownPos) || newRight.Contains(newDownPos)) {
            newDown.Add(downPos);
        }
        else {
            changes++;
            newDown.Add(newDownPos);
        }
    }
    return (changes, newRight, newDown);
};

var it = 0;
var _right = dirs.Where(x => x.cucumber == '>').Select(x => x.pos).ToHashSet();
var _down = dirs.Where(x => x.cucumber == 'v').Select(x => x.pos).ToHashSet();

while(true) {
    it++;
    var stepResult = step(_right, _down);
    
    if (stepResult.changes == 0) {
        break;
    }

    _right = stepResult.newRight;
    _down = stepResult.newDown;
}

Console.WriteLine($"P1:{it}");
