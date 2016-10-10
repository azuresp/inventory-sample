using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace Valent.Inv
{
    class ConsoleExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            Console.WriteLine($"An exception has occurred: {context.Exception}");
        }
    }
}
