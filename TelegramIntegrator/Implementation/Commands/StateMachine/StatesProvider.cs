namespace Maestro.TelegramIntegrator.Implementation.Commands.StateMachine;

public class StatesProvider(IEnumerable<IState> states) : IStatesProvider
{
    private readonly IState[] _states = states.ToArray();

    public IState GetState<TState>() where TState : IState
    {
        return _states.First(x => x is TState);
    }
}