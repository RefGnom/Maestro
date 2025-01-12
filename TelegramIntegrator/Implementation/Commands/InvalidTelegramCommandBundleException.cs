namespace Maestro.TelegramIntegrator.Implementation.Commands;

public class InvalidTelegramCommandBundleException(string message) : Exception(message);