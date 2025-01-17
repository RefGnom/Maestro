using Microsoft.Extensions.Caching.Memory;

namespace Maestro.TelegramIntegrator.Implementation.Commands.StateMachine;

public class StateSwitcher(Lazy<IStatesProvider> statesProvider) : IStateSwitcher
{
    private static readonly TimeSpan CacheExpirationTimeout = TimeSpan.FromMinutes(15);

    private readonly MemoryCache _memoryCache = new(new MemoryCacheOptions());
    private readonly Lazy<IStatesProvider> _statesProvider = statesProvider;

    public async Task SetStateAsync<TState>(long userId) where TState : IState
    {
        var state = _statesProvider.Value.GetState<TState>();
        _memoryCache.Set(userId, state, CacheExpirationTimeout);
        await state.Initialize(userId);
    }

    public IState GetState(long userId)
    {
        if (_memoryCache.TryGetValue<IState>(userId, out var state))
        {
            return state!;
        }

        var mainState = _statesProvider.Value.GetState<MainState>();
        _memoryCache.Set(userId, mainState, CacheExpirationTimeout);
        return mainState;

    }
}