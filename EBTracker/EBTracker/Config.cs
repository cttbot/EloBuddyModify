using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberHidesStaticFromOuterClass

namespace EBTracker
{
    internal static class Config
    {
        private const string MenuName = "EloBuddy Tracker";

        private static readonly Menu Menu;

        static Config()
        {
            Menu = MainMenu.AddMenu(MenuName, MenuName.ToLower());
            Menu.AddGroupLabel("EloBuddy Tracker");
            Menu.AddLabel("Credit By: MarioGK", 50);
            Menu.AddLabel("Modify By: CTTBOT", 50);
            Types.Initialize();
        }

        public static void Initialize()
        {
        }

        public static class Types
        {
            private static readonly Menu RecallTrackerMenu, SpellTrackerMenu;

            static Types()
            {
                RecallTrackerMenu = Menu.AddSubMenu("::Recall Tracker::");
                RecallTracker.Initialize();

                SpellTrackerMenu = Menu.AddSubMenu("::Spell Tracker::");
                SpellTracker.Initialize();
            }

            public static void Initialize()
            {
            }

            public static class RecallTracker
            {
                private static readonly CheckBox _turnOff;
                private static readonly CheckBox _recallEnemies;
                private static readonly CheckBox _recallAllies;
                private static readonly CheckBox _chatWarning;
                private static readonly Slider _yPos;
                private static readonly Slider _xPos;

                public static bool TurnOff
                {
                    get { return _turnOff.CurrentValue; }
                }
                public static bool ChatWarning
                {
                    get { return _chatWarning.CurrentValue; }
                }

                public static bool RecallEnemies
                {
                    get { return _recallEnemies.CurrentValue; }
                }

                public static bool RecallAllies
                {
                    get { return _recallAllies.CurrentValue; }
                }

                public static int YPos
                {
                    get { return _yPos.CurrentValue; }
                }

                public static int XPos
                {
                    get { return _xPos.CurrentValue; }
                }

                static RecallTracker()
                {
                    RecallTrackerMenu.AddGroupLabel("Recall Tracker Settings");
                    _turnOff = RecallTrackerMenu.Add("turnoffrecalltracker", new CheckBox("Turn off recall tracker ?", false));
                    _chatWarning = RecallTrackerMenu.Add("chatWarning", new CheckBox("Chat Notification", false));
                    RecallTrackerMenu.AddSeparator();
                    _recallEnemies = RecallTrackerMenu.Add("recallEnemies", new CheckBox("Enemies recall ?"));
                    _recallAllies = RecallTrackerMenu.Add("recallAllies", new CheckBox("Allies recall?", false));
                    RecallTrackerMenu.AddGroupLabel("Recall Tracker Position");
                    _yPos = RecallTrackerMenu.Add("ypositionslider", new Slider("y Position?", 610, 0, 700));
                    _xPos = RecallTrackerMenu.Add("xpositionslider", new Slider("x Position?", 700, 0, 1600));
                    
                }

                public static void Initialize()
                {
                }
            }

            public static class SpellTracker
            {
                private static readonly CheckBox _turnOff;
                private static readonly CheckBox _drawEnemies;
                private static readonly CheckBox _drawAllies;

                public static bool TurnOff
                {
                    get { return _turnOff.CurrentValue; }
                }

                public static bool DrawEnemies
                {
                    get { return _drawEnemies.CurrentValue; }
                }

                public static bool DrawAllies
                {
                    get { return _drawAllies.CurrentValue; }
                }

                static SpellTracker()
                {
                    SpellTrackerMenu.AddGroupLabel("SpellTracker");
                    _turnOff = SpellTrackerMenu.Add("turnoffspelltracker", new CheckBox("Turn Off Spell Tracker ?" ,false));
                    SpellTrackerMenu.AddSeparator();
                    _drawEnemies = SpellTrackerMenu.Add("drawenemiesid", new CheckBox("Draw Enemies ?"));
                    _drawAllies = SpellTrackerMenu.Add("drawalliesid", new CheckBox("Draw Allies ?"));
                }

                public static void Initialize()
                {
                }
            }
        }
    }
}


