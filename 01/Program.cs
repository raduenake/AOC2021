// See https://aka.ms/new-console-template for more information

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\r\n")
    .Select(l => int.Parse(l))
    .ToList();


int inc = 0;
int previousMeasurement = -1;
foreach (var measurement in input)
{
    if (previousMeasurement > 0 && previousMeasurement < measurement)
    {
        inc++;
    }
    previousMeasurement = measurement;
}

int inc2 = 0;
for (int i = 0; i < input.Count() - 3; i++)
{
    int slidingWindow = input[i] + input[i + 1] + input[i + 2];
    int slidingWindow1 = input[i + 1] + input[i + 2] + input[i + 3];
    if (slidingWindow < slidingWindow1)
    {
        inc2++;
    }
}

Console.WriteLine($"Inc: {inc}; Window Inc {inc2}");