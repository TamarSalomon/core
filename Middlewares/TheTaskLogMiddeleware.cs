using System.Diagnostics;

namespace myTask.Middlewares;

public class TheTaskLogMiddeleware
{
    private RequestDelegate next;
    private string logger;

    public TheTaskLogMiddeleware(RequestDelegate next, string logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task Invoke(HttpContext c)
    {
        var sw = new Stopwatch();
        sw.Start();
        await next(c);
        WriteLogToFile($"{c.Request.Path}.{c.Request.Method} took {sw.ElapsedMilliseconds}ms."
            + $" User: {c.User?.FindFirst("userId")?.Value ?? "unknown"}");     
    }    

   private void WriteLogToFile(string logMessage)
    {
        using (StreamWriter sw = File.AppendText(logger))
        {
            sw.WriteLine(logMessage);
        }
    }
}

public static partial class LogerMiddleExtensions
{
    public static IApplicationBuilder UseLogMiddleware(this IApplicationBuilder builder,string FilePath)
    {
        return builder.UseMiddleware<TheTaskLogMiddeleware>(FilePath);
    }
}