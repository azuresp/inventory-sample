using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Valent.Inv.Domain
{
    public interface INotifyRepository
    {
        void Notify(string message);
    }
}
