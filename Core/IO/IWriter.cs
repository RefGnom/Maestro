namespace Maestro.Core.IO;

public interface IWriter
{
    void WriteLine(string message, ConsoleColor color);
}