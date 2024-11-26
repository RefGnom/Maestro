using System.Text.Json;
using Data.Models;

namespace Data;

// ReSharper disable once ClassNeverInstantiated.Global
public class DataContext : IAsyncDisposable
{
    private static DataContext? _instance;
    private static bool _isDisposed;

    private static readonly FileInfo FileInfo = new DirectoryInfo(Directory.GetCurrentDirectory())
        .Parent!.Parent!.Parent!.Parent!
        .GetFiles("data.json").Single();

    private readonly List<User> _users = null!;
    private readonly List<Event> _events = null!;

    public List<User> Users
    {
        get
        {
            ObjectDisposedException.ThrowIf(_isDisposed, _instance!);
            return _users;
        }
        init => _users = value;
    }
    
    public List<Event> Events
    {
        get
        {
            ObjectDisposedException.ThrowIf(_isDisposed, _instance!);
            return _events;
        }
        init => _events = value;
    }
    public static async Task<DataContext> GetAsync()
    {
        ObjectDisposedException.ThrowIf(_isDisposed, _instance!);

        if (_instance is null)
            await LoadAsync();

        return _instance!;
    }

    public async ValueTask DisposeAsync()
    {
        if (_isDisposed)
            throw new InvalidOperationException($"{nameof(DataContext)} is already disposed.");

        await using var fileStream = FileInfo.Open(FileMode.Create, FileAccess.Write, FileShare.None);
        await JsonSerializer.SerializeAsync(fileStream, _instance);
        _isDisposed = true;
        GC.SuppressFinalize(this);
    }

    private static async Task LoadAsync()
    {
        await using var fileStream = FileInfo.OpenRead();
        _instance = await JsonSerializer.DeserializeAsync<DataContext>(fileStream);
    }
}