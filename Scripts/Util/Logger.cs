using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class Logger
{
    public static int LogSize = 50;
    public static List<String> LogEntries = new List<String>();

    public static void Log(string message)
    {
        GD.Print(message);
        LogEntries.Add(message);

        if (LogEntries.Count > LogSize)
            LogEntries.Remove(LogEntries[0]);
    }
}

