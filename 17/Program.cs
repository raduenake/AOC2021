// too lazy to pattern match and read the input ...
var target = (x: (xmin: 201, xmax: 230), y: (ymin: -99, ymax: -65));

var sim = new Simulation(target);
var hits = sim.RunSim().ToList();

Console.WriteLine($"P1: {hits.OrderBy(r => r.y_max).Last().y_max}");
Console.WriteLine($"P2: {hits.Count()}");