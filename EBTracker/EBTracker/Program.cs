using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK.Events;

namespace EBTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Initialize.Init;
        }
    }
}
