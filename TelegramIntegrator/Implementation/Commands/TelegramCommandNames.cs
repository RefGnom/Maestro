namespace Maestro.TelegramIntegrator.Implementation.Commands;

public static class TelegramCommandNames
{
    public const string CreateReminderTelegramCommand = "/reminder";
    public const string CreateScheduleTelegramCommand = "/schedule";

    public static string[] GetCommandNames()
    {
        var type = typeof(TelegramCommandNames);
        var fieldInfos = type.GetFields();
        return fieldInfos.Select(x => x.GetValue(null)!.ToString()).ToArray()!;
    }
}