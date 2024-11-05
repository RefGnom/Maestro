using System.Text.Json;
using Core.Models;

namespace Core.Repositories;

// ReSharper disable once ClassNeverInstantiated.Global
public class DataContext : IAsyncDisposable
{
    private static DataContext? _instance;

    private static readonly FileInfo FileInfo = new DirectoryInfo(Directory.GetCurrentDirectory())
        .Parent!.Parent!.Parent!.Parent!
        .GetFiles("data.json").Single();

    public List<User> Users { get; init; } = null!;

    public async ValueTask DisposeAsync()
    {
        await using var fileStream = FileInfo.Open(FileMode.Create, FileAccess.Write, FileShare.None);
        await JsonSerializer.SerializeAsync(fileStream, _instance);
        GC.SuppressFinalize(this);
    }

    public static async Task<DataContext> GetAsync()
    {
        if (_instance is null)
            await LoadAsync();

        return _instance!;
    }

    private static async Task LoadAsync()
    {
        await using var fileStream = FileInfo.OpenRead();
        _instance = await JsonSerializer.DeserializeAsync<DataContext>(fileStream);
    }
}