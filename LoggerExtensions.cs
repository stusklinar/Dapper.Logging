
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace stuartalexanderltd.dapper
{
    internal static class ProfiledDbExtensions
    {
        public static void Action(ILogger logger, Action func, string message)
        {
            var sw = new Stopwatch();
            func();
            sw.Stop();
            logger.LogInformation($"{message} - {sw.ElapsedMilliseconds}ms");
        }
        public static T Action<T>(ILogger logger, Func<T> func, string message)
        {
            var sw = new Stopwatch();
            var result = func();
            sw.Stop();
            logger.LogInformation($"{message} - {sw.ElapsedMilliseconds}ms");
            return result;
        }
    }
}