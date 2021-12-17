// too lazy to pattern match and read the input ...
var targetAreaInput = (X: (xMin: 201, xMax: 230), Y: (yMin: -99, yMax: -65));

var simulation = new Simulation(targetAreaInput);
var hits = simulation.RunSim().ToList();

Console.WriteLine($"P1: {hits.OrderBy(result => result.yMaxHit).Last().yMaxHit}");
Console.WriteLine($"P2: {hits.Count()}");