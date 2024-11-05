using Core.IO;
using Core.Models;
using Core.Repositories;

namespace Client;

internal class Program
{
    private static async Task Main(string[] args)
    {
        await using var dataContext = await DataContext.GetAsync();
        new Writer().WriteLine(dataContext.Users.Count.ToString());
        dataContext.Users.Add(new User { Id = dataContext.Users.Last().Id + 1 });
        new Writer().WriteLine(dataContext.Users.Count.ToString());
    }
}