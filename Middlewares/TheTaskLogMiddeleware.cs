using System.Diagnostics;

namespace myTask.Middlewares
{
    public class TheTaskLogMiddeleware
    {
        private readonly object _lock = new object();
        private readonly RequestDelegate next;
        private readonly string logger;

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

            WriteLogToFile($"{DateTime.Now} {c.Request.Path}.{c.Request.Method} took {sw.ElapsedMilliseconds}ms."
                + $" User: {c.User?.FindFirst("Id")?.Value ?? "unknown"}");
        }

        private void WriteLogToFile(string logMessage)
        {
            lock(_lock)
            {
            using StreamWriter sw = File.AppendText(logger);
            sw.WriteLine(logMessage);
             }
        }
    }

    public static partial class MyMiddleExtensions
    {
        public static IApplicationBuilder UseMyMiddleExtensions(this IApplicationBuilder builder, string FilePath)
        {
            return builder.UseMiddleware<TheTaskLogMiddeleware>(FilePath);
        }
    }
}

