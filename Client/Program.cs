using Core.IO;
using Data;
using Data.Models;

namespace Client;

internal class Program
{
    private static async Task Main(string[] args)
    {
        await using var dataContext = await DataContext.GetAsync();
        new Writer().WriteLine(dataContext.Users.Count.ToString());
        dataContext.Users.Add(new User { Id = dataContext.Users.Last().Id + 1 });
        new Writer().WriteLine(dataContext.Users.Count.ToString());
        await using var dataContext2 = await DataContext.GetAsync();
    }
}