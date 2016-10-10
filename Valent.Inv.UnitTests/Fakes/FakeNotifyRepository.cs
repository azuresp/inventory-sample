using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valent.Inv.Domain;

namespace Valent.Inv.UnitTests.Fakes
{
    class FakeNotifyRepository : INotifyRepository
    {
        public List<string> Messages = new List<string>();

        public void Notify(string message)
        {
            Messages.Add(message);
        }
    }
}
