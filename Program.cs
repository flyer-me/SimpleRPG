namespace dotnetHello;
using System;
class Program
{
    unsafe static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        string line;
        while ((line = Console.ReadLine()) != null) { // 注意 while 处理多个 case
            string[] tokens = line.Split(" ");
            Console.WriteLine(tokens.Last().Length);
        }
    }
}
