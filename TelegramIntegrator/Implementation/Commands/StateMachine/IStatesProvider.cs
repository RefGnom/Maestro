namespace Maestro.TelegramIntegrator.Implementation.Commands.StateMachine;

public interface IStatesProvider
{
    IState GetState<TState>() where TState : IState;
}