﻿namespace Maestro.Client.Models;

public record Message(string Command, DateTime ReminderTime, string Description);