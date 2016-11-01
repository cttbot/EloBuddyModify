using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;

namespace Orbextra
{
    class Program
    {
        private static Orbwalking.Orbwalker Orbwalker { get; set; }

        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += OnGameLoad;
        }
        public static void OnGameLoad(EventArgs args)
        {
            var orbwalkMenu = MainMenu.AddMenu("Orbwalker", "Orbwalker");
            {
                Orbwalker = new Orbwalking.Orbwalker(orbwalkMenu);
            }
        }
    }
}
