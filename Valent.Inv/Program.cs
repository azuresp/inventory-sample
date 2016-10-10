using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;

namespace Valent.Inv
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9000/";

            // Start OWIN host 
            using (WebApp.Start<Startup>(baseAddress))
            {
                Console.WriteLine($"Hosting at {baseAddress}");
                Console.WriteLine($"Documentation at {baseAddress}swagger");

                Console.WriteLine("Any key to exit.");
                Console.ReadKey();
            }
        }
    }
}
