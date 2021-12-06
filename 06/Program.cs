// See https://aka.ms/new-console-template for more information
var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split(',')
    .Select(v => int.Parse(v))
    .GroupBy(n => n)
    .ToDictionary(g => g.Key, g => (long)g.Count());

Func<Dictionary<int, long>, Dictionary<int, long>> DayCycle = (Dictionary<int, long> lanternFish) =>
    new()
    {
        [8] = lanternFish.GetValueOrDefault(0),
        [7] = lanternFish.GetValueOrDefault(8),
        [6] = lanternFish.GetValueOrDefault(0) + lanternFish.GetValueOrDefault(7),
        [5] = lanternFish.GetValueOrDefault(6),
        [4] = lanternFish.GetValueOrDefault(5),
        [3] = lanternFish.GetValueOrDefault(4),
        [2] = lanternFish.GetValueOrDefault(3),
        [1] = lanternFish.GetValueOrDefault(2),
        [0] = lanternFish.GetValueOrDefault(1),
    };

for (int i = 0; i < 80; i++)
    input = DayCycle(input);
Console.WriteLine($"P1: {input.Values.Sum()}");

for (int i = 80; i < 256; i++)
    input = DayCycle(input);
Console.WriteLine($"P2: {input.Values.Sum()}");