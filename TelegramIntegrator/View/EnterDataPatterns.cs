namespace Maestro.TelegramIntegrator.View;

public static class EnterDataPatterns
{
    public const string TimeSpanPattern = """
                                          7 -> 7 минут
                                          2:7 -> 2 часа, 7 минут
                                          1:2:7 -> 1 день, 2 часа, 7 минут
                                          """;

    public const string DateTimePattern = """
                                          20 -> 20:00 текущего дня (или следующего)
                                          20:15 -> 20:15 текущего дня (или следующего)
                                          15.09 23 -> 15 сентября 23:00
                                          15.09.2100 13:31 -> 15 сентября 2100 года 13:31
                                          """;
}