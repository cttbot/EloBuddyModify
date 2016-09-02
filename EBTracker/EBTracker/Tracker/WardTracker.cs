using EloBuddy;
using EloBuddy.SDK.Rendering;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using Color = System.Drawing.Color;

namespace EBTracker.Tracker
{
    internal class WardTracker
    {
        public class Wards
        {
            public Wards(string name, bool isPink, bool available, float deleteTimer, Vector3 position, Color color)
            {
                Name = name;
                IsPink = isPink;
                DeleteTimer = deleteTimer;
                Position = position;
                Color = color;
                Available = available;
            }

            public string Name { get; set; }
            public bool IsPink { get; set; }
            public Color Color { get; set; }
            public float DeleteTimer { get; set; }
            public Vector3 Position { get; set; }
            public bool Available { get; set; }
        }

        private static List<Wards> _Wards = new List<Wards>();

        public static void OnCreate(GameObject sender, EventArgs args)
        {
            if (!(sender is Obj_AI_Minion) || !sender.Name.Contains("Ward")) return;

            var ward = (Obj_AI_Minion)sender;
            if (ward.IsAlly) return;

            switch (ward.BaseSkinName)
            {
                case "YellowTrinket":
                    _Wards.Add(new Wards(ward.Name, false, true, Game.Time + 60, ward.Position, Color.Green));
                    break;
                case "YellowTrinketUpgrade":
                    _Wards.Add(new Wards(ward.Name, false, true, Game.Time + 120, ward.Position, Color.Green));
                    break;
                case "VisionWard":
                    _Wards.Add(new Wards(ward.Name, true, true, 0, ward.Position, Color.DeepPink));
                    break;
                case "SightWard":
                    _Wards.Add(new Wards(ward.Name, false, true, Game.Time + 180, ward.Position, Color.Green));
                    break;
            }
        }

        public static void OnDelete(GameObject sender, EventArgs args)
        {
            if (!(sender is Obj_AI_Minion) || !sender.Name.Contains("Ward")) return;

            var ward = (Obj_AI_Minion)sender;

            if (ward.IsAlly) return;

            var _wards = _Wards.Where(w => w.Available).FirstOrDefault(w => w.Position == ward.Position);
            if (_wards != null)
                _wards.Available = false;
        }

        public static void OnDraw(EventArgs args)
        {
            var auxList = new List<Wards>();
            foreach (var wardInfo in _Wards)
            {
                if (!wardInfo.Available)
                {
                    auxList.Add(wardInfo);
                    continue;
                }

                var diffTime = wardInfo.DeleteTimer - Game.Time;

                if (diffTime > 0 || wardInfo.IsPink)
                {
                    var fancytimer = string.Format("{0:0}", diffTime);
                    new Circle { Color = wardInfo.Color, Radius = 20, BorderWidth = 1f }.Draw(wardInfo.Position);

                    Drawing.DrawText(Drawing.WorldToScreen(wardInfo.Position), Color.White, fancytimer,
                        50);
                }
                else
                {
                    auxList.Add(wardInfo);
                }
            }

            if (auxList.Count > 0)
            {
                _Wards = _Wards.Except(auxList).ToList();
            }
        }
    }
}
