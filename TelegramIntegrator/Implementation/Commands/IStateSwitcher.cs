namespace Maestro.TelegramIntegrator.Implementation.Commands;

public interface IStateSwitcher
{
    void SetState<TState>() where TState : IState;
}