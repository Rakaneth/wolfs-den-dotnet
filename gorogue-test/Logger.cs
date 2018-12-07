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
<html>
  <head>
    <style>
      body {background: black;}
      li:nth-child(even) {background:#999;}
      li:nth-child(odd) {background:#DDD;}   
    </style>
  </head>
  <body>
    <ul>
";
          sw.Write(style);
        }
      }
    }
    public static void Log(LogLevel level, string text)
    {
      if (World.Instance.Configs.Logging)
      {
        using (var fs = new FileStream(LOGFILE, FileMode.Append, FileAccess.Write))
        {
          using (var sw = new StreamWriter(fs))
          {
            string start = $"{DateTime.Now.ToShortTimeString()}";
            string output = "";
            switch (level)
            {
              case LogLevel.ERROR: output = "<span style='color: #990000;'>[ERROR]"; break;
              case LogLevel.INFO: output = "<span style='color: #009900;'>[INFO]"; break;
              case LogLevel.WARNING: output = "<span style='color: #000099;'>[WARNING]"; break;
            }
            sw.WriteLine($"     <li>{output} {start} {text}</span></li>");
          }  
        }
      }
    }

    public static void CloseLog()
    {
      using (var fs = new FileStream(LOGFILE, FileMode.Append, FileAccess.Write))
      {
        using (var sw = new StreamWriter(fs))
        {
          sw.Write(@"   </ul>
  </body>
</html>");
        }
      }
    }
  }
}