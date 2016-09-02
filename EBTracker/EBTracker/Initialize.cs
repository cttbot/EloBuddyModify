using System;
using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.SDK.Menu;

namespace EBTracker
{
    internal class Initialize
    {
        public static void Init(EventArgs args)
        {
            Config.Initialize();

            Tracker.RecallTracker.Initialize();

            Tracker.SpellsTracker.Initialize();

            #region EventsIntializers
            Drawing.OnEndScene += Drawing_OnEndScene;
            Drawing.OnDraw += Drawing_OnDraw;
            Teleport.OnTeleport += Teleport_OnTeleport;
            GameObject.OnCreate += GameObject_OnCreate;
            GameObject.OnDelete += GameObject_OnDelete;
            //For tracker only
            AppDomain.CurrentDomain.DomainUnload += OnUnload;
            AppDomain.CurrentDomain.ProcessExit += OnUnload;
            #endregion EventsIntializers
        }

        private static void Drawing_OnEndScene(EventArgs args)
        {
            #region RecallTracker
            Tracker.RecallTracker.DrawOnEnd(args);
            #endregion RecallTracker

            #region SpellsTracker
            Tracker.SpellsTracker.OnDrawEnd(args);
            #endregion SpellsTracker
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            #region WardTracker
            Tracker.WardTracker.OnDraw(args);
            #endregion WardTracker
        }

        private static void Teleport_OnTeleport(Obj_AI_Base sender, Teleport.TeleportEventArgs args)
        {
            #region RecallTracker
            var hero = sender as AIHeroClient;

            Tracker.RecallTracker.OnTeleport(hero, args);
            #endregion RecallTracker
        }

        private static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            #region WardTracker
            Tracker.WardTracker.OnCreate(sender, args);
            #endregion WardTracker
        }

        private static void GameObject_OnDelete(GameObject sender, EventArgs args)
        {
            #region WardTracker
            Tracker.WardTracker.OnDelete(sender, args);
            #endregion WardTracker
        }

        private static void OnUnload(object sender, EventArgs e)
        {
            #region RecallTracker
            Tracker.SpellsTracker.OnUnload(sender, e);
            #endregion RecallTracker
        }
    }
}
