using System.Numerics;

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd().Split("\r\n").Where(l => !string.IsNullOrEmpty(l));

var degToRad = (float deg) => (float)(Math.PI / 180.0) * deg;

var rotateAxis = (string axis, float[] degs) =>
{
    var result = Quaternion.Identity;
    for (int i = 0; i < axis.Length; i++)
    {
        result = Quaternion.Concatenate(result, axis[i] switch
        {
            'x' => Quaternion.CreateFromAxisAngle(Vector3.UnitX, degToRad(degs[i])),
            'y' => Quaternion.CreateFromAxisAngle(Vector3.UnitY, degToRad(degs[i])),
            'z' => Quaternion.CreateFromAxisAngle(Vector3.UnitZ, degToRad(degs[i])),
            _ => throw new Exception()
        });
    }
    return result;
};

var generateRotation = (List<Vector3> scannerHits) =>
{
    var rotations = new[] {
        rotateAxis("zx", new[] {0f,0f}),
        rotateAxis("zx", new[] {0f,90f}),
        rotateAxis("zx", new[] {0f,180f}),
        rotateAxis("zx", new[] {0f,270f}),
        rotateAxis("zx", new[] {90f,0f}),
        rotateAxis("zx", new[] {90f,90f}),
        rotateAxis("zx", new[] {90f,180f}),
        rotateAxis("zx", new[] {90f,270f}),
        rotateAxis("zx", new[] {180f,0f}),
        rotateAxis("zx", new[] {180f,90f}),
        rotateAxis("zx", new[] {180f,180f}),
        rotateAxis("zx", new[] {180f,270f}),
        rotateAxis("zx", new[] {270f,0f}),
        rotateAxis("zx", new[] {270f,90f}),
        rotateAxis("zx", new[] {270f,180f}),
        rotateAxis("zx", new[] {270f,270f}),
        rotateAxis("yx", new[] {90f,0f}),
        rotateAxis("yx", new[] {90f,90f}),
        rotateAxis("yx", new[] {90f,180f}),
        rotateAxis("yx", new[] {90f,270f}),
        rotateAxis("yx", new[] {270f,0f}),
        rotateAxis("yx", new[] {270f,90f}),
        rotateAxis("yx", new[] {270f,180f}),
        rotateAxis("yx", new[] {270f,270f})
    };
    return rotations.Select(
        rotation => scannerHits.Select(scannerHit =>
        {
            var tr = Vector3.Transform(scannerHit, (Quaternion)rotation);
            return new Vector3((int)Math.Round(tr.X), (int)Math.Round(tr.Y), (int)Math.Round(tr.Z));
        })
    );
};

var scanners = new List<List<Vector3>>();

var scanner = new List<Vector3>();
foreach (var l in input)
{
    if (l.StartsWith("--"))
    {
        if (scanner.Any())
            scanners.Add(scanner);
        scanner = new List<Vector3>();
        continue;
    }
    var split = l.Split(",").Select(v => int.Parse(v)).ToList();
    scanner.Add(new Vector3(split[0], split[1], split[2]));
}
scanners.Add(scanner);

var alignScanners = (List<Vector3> foundBeacons, List<Vector3> currentScannerBeacons) =>
{
    var scannerBeaconsTransformed = new List<Vector3>();
    var scannerPosRelativeTo0 = new Vector3();
    foreach (var currScanBeaconsRot in generateRotation(currentScannerBeacons))
    {
        foreach (var beacon in foundBeacons)
        {
            foreach (var rotBeacon in currScanBeaconsRot)
            {
                scannerPosRelativeTo0 = beacon - rotBeacon;
                scannerBeaconsTransformed = currScanBeaconsRot.Select(b => b + scannerPosRelativeTo0).ToList();
                var intersection = foundBeacons.Intersect(scannerBeaconsTransformed).ToList();
                if (intersection.Count >= 12)
                {
                    return (aligned: true, pos: scannerPosRelativeTo0, transfBeacons: scannerBeaconsTransformed);
                }
            }
        }
    }
    return (aligned: false, pos: new Vector3(0, 0, 0), transfBeacons: new List<Vector3>());
};

var foundBeacons = scanners.First().ToList();
var remainingScannersBeacons = new Queue<List<Vector3>>(scanners.Skip(1));
var scannersPosRelativeTo0 = new List<Vector3>() { Vector3.Zero };

while (remainingScannersBeacons.Any())
{
    var currScanBeacons = remainingScannersBeacons.Dequeue();

    var align = alignScanners(foundBeacons, currScanBeacons);
    if (align.aligned)
    {
        foundBeacons = foundBeacons.Union(align.transfBeacons).ToList();
        scannersPosRelativeTo0.Add(align.pos);
    }
    else
    {
        remainingScannersBeacons.Enqueue(currScanBeacons);
    }
}

Console.WriteLine($"P1: {foundBeacons.Count()}");

var scannersManhattanDistance = scannersPosRelativeTo0.SelectMany((s, i) =>
    scannersPosRelativeTo0.Skip(i+1).Select(s1 =>
    {
        var d = s - s1;
        return (int)Math.Abs(d.X) + (int)Math.Abs(d.Y) + (int)Math.Abs(d.Z);
    })
);

Console.WriteLine($"P2: {scannersManhattanDistance.Max()}");