// See https://aka.ms/new-console-template for more information

var file = System.IO.File.OpenText("input.txt");
var input = file.ReadToEnd()
    .Split("\r\n")
    .ToList();

Console.WriteLine($"Hello, World!");