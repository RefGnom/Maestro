namespace Maestro.Core.IO;

public class Writer : IWriter
{
    private static readonly object Lock = new();

    public void WriteLine(string message, ConsoleColor color = ConsoleColor.Gray)
    {
        lock (Lock)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
        }
    }
}