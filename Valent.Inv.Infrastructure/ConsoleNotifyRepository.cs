using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valent.Inv.Domain;

namespace Valent.Inv.Infrastructure
{
    public class ConsoleNotifyRepository : INotifyRepository
    {
        public void Notify(string message)
        {
            Console.WriteLine(message);
        }
    }
}
