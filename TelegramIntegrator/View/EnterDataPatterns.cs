namespace Maestro.TelegramIntegrator.View;

public static class EnterDataPatterns
{
    public const string TimeSpanPattern = """
                                          7 -> 7 минут
                                          2:7 -> 2 часа, 7 минут
                                          1:2:7 -> 1 день, 2 часа, 7 минут
                                          """;
}