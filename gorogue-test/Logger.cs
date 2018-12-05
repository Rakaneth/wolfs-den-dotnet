using System;
using System.IO;

public enum LogLevel
{
  ERROR,
  WARNING,
  INFO
}

namespace GoRogueTest
{
  public static class Logger
  {
    private const string LOGFILE = "log.html";

    static Logger()
    {
      using (var fs = new FileStream(LOGFILE, FileMode.Create, FileAccess.Write))
      {
        using (var sw = new StreamWriter(fs))
        {
          string style = @"
          <!DOCTYPE html>
          <style>
            ul:nth-child(even) {background:#CCC;}
            ul:nth-child(odd) {background:#FFF;}   
          </style>
          <ul>";
          sw.Write(style);
        }
      }
    }
    public static void Log(LogLevel level, string text)
    {
      using (var fs = new FileStream(LOGFILE, FileMode.Append, FileAccess.Write))
      {
        using (var sw = new StreamWriter(fs))
        {
          string start = $"{DateTime.Now.ToShortTimeString()}";
          string output = "";
          switch (level)
          {
            case LogLevel.ERROR: output = "<span style='color: #ff0000;'>[ERROR]"; break;
            case LogLevel.INFO: output = "<span style='color: #00ff00;'>[INFO]"; break;
            case LogLevel.WARNING: output = "<span style='color: #0000ff;'>[WARNING]"; break;
          }
          sw.WriteLine($"<li>{output} {start} {text}</span></li>");
        }  
      }
    }

    public static void CloseLog()
    {
      using (var fs = new FileStream(LOGFILE, FileMode.Append, FileAccess.Write))
      {
        using (var sw = new StreamWriter(fs))
        {
          sw.WriteLine("</ul>");
        }
      }
    }
  }
}