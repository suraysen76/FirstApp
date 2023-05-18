using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HangfireTest1.Services
{
    public class JobsHelper
    {

        public void Print(string message)
        {
            // Wait for 15 seconds
            Thread.Sleep(15000);
            Console.WriteLine("The message is " + message);
        }
    }
}
