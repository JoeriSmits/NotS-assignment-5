using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoadBalancer
{
    internal static class Algoritme
    {
        public static object algoritme;
        public static int roundRobinPos = 0;
        public static List<string[]> requestPerServer = new List<string[]>(); 

        public static string Get()
        {
            return (string) algoritme;
        }
    }
}
