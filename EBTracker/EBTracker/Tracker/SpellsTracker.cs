using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Rendering;
using EBTracker.Properties;
using SharpDX;
using Color = System.Drawing.Color;

using Settings = EBTracker.Config.Types.SpellTracker;

namespace EBTracker.Tracker
{
    internal class SpellsTracker
    {
        private const int OffsetHudX = -21;
        private const int OffsetHudY = 2;

        private const int OffsetSpellsX = OffsetHudX + 22;
        private const int OffsetSpellsY = OffsetHudY + 11;

        private const int OffsetSummonersX = OffsetHudX + 4;
        private const int OffsetSummonersY = OffsetHudY + 2;

        public static readonly TextureLoader TextureLoader = new TextureLoader();

        private static Sprite MainBar { get; set; }
        private static Text Text { get; set; }

        public static readonly SpellSlot[] SpellSlots = { SpellSlot.Q, SpellSlot.W, SpellSlot.E, SpellSlot.R };
        private static readonly SpellSlot[] Summoners = { SpellSlot.Summoner1, SpellSlot.Summoner2 };
        private static readonly Dictionary<string, Sprite> SummonerSpells = new Dictionary<string, Sprite>();

        public static Menu Menu { get; set; }

        public static void Initialize()
        {
            // Load main hud textures
            TextureLoader.Load("hud", Resources.hud2);

            // Initialize main drawings
            Text = new Text("", new Font(FontFamily.GenericSansSerif, 8, FontStyle.Bold)) { Color = Color.AntiqueWhite };
            MainBar = new Sprite(() => TextureLoader["hud"]);

            TextureLoader.Load("summonersmite", Resources.SummonerSmite1);
            TextureLoader.Load("s5_summonersmiteduel", Resources.SummonerRedSmite);
            TextureLoader.Load("s5_summonersmiteplayerganker", Resources.SummonerBlueSmite);
            SummonerSpells.Add("summonersmite", new Sprite(() => TextureLoader["summonersmite"]));
            SummonerSpells.Add("s5_summonersmiteduel", new Sprite(() => TextureLoader["s5_summonersmiteduel"]));
            SummonerSpells.Add("s5_summonersmiteplayerganker", new Sprite(() => TextureLoader["s5_summonersmiteplayerganker"]));

            // Load summoner spells dynamically
            foreach (var summoner in EntityManager.Heroes.AllHeroes.SelectMany(o => o.Spellbook.Spells.Where(s => Summoners.Contains(s.Slot))).Select(o => o.Name.ToLower()))
            {
                var summonerName = summoner;
                if (SummonerSpells.ContainsKey(summonerName))
                {
                    continue;
                }

                Bitmap bitmap = null;
                switch (summonerName)
                {
                    //Clarity
                    case "summonerclarity":
                        bitmap = Resources.SummonerClarity;
                        break;
                    //Garrison
                    case "summonergarrison":
                        bitmap = Resources.SummonerGarrison;
                        break;
                    //Ghost
                    case "summonerhaste":
                        bitmap = Resources.SummonerGhost;
                        break;
                    //Heal
                    case "summonerheal":
                        bitmap = Resources.SummonerHeal;
                        break;
                    //SnowBall
                    case "summonersnowball":
                        bitmap = Resources.SummonerSnowBall;
                        break;
                    //Barrier
                    case "summonerbarrier":
                        bitmap = Resources.SummonerBarrier;
                        break;
                    //Exhaust
                    case "summonerexhaust":
                        bitmap = Resources.SummonerExhaust;
                        break;
                    //Cleanse
                    case "summonerboost":
                        bitmap = Resources.SummonerCleanse;
                        break;
                    //TP
                    case "summonerteleport":
                        bitmap = Resources.SummonerTeleport;
                        break;
                    //ClairVoyance
                    case "summonerclairvoyance":
                        bitmap = Resources.SummonerClairvoyance;
                        break;
                    //Flash
                    case "summonerflash":
                        bitmap = Resources.SummonerFlash;
                        break;
                    //Ignite
                    case "summonerdot":
                        bitmap = Resources.SummonerIgnite;
                        break;
                    case "summonersmite":
                        bitmap = Resources.SummonerSmite1;
                        break;
                    //RedSmite
                    case "s5_summonersmiteduel":
                        bitmap = Resources.SummonerRedSmite;
                        break;
                    //BlueSmite
                    case "s5_summonersmiteplayerganker":
                        bitmap = Resources.SummonerBlueSmite;
                        break;
                }

                TextureLoader.Load(summonerName, bitmap);
                SummonerSpells.Add(summonerName, new Sprite(() => TextureLoader[summonerName]));
            }
        }

        public static void OnDrawEnd(EventArgs args)
        {
            if (Settings.TurnOff) return;

            foreach (var unit in EntityManager.Heroes.AllHeroes.Where(o => o.IsHPBarRendered && !o.IsMe).Where(o => o.IsAlly ? Settings.DrawAllies : Settings.DrawEnemies))
            {
                var hpBarPos = new Vector2(unit.HPBarPosition.X, unit.HPBarPosition.Y + -1);
                // Summoner spells
                foreach (var summonerSpell in Summoners)
                {
                    var spell = unit.Spellbook.GetSpell(summonerSpell);
                    var cooldown = spell.CooldownExpires - Game.Time;
                    var spellPos = GetSummonerOffset(unit, spell.Slot);
                    if (SummonerSpells.ContainsKey(spell.Name.ToLower()))
                    {
                        var sprite = SummonerSpells[spell.Name.ToLower()];
                        sprite.Color = cooldown < 0 ? Color.White : Color.FromArgb(255, Color.DimGray);
                        sprite.Draw(new Vector2(spellPos.X + 1, spellPos.Y + 3));
                    }

                    if (!(cooldown > 0)) continue;

                    Text.TextValue = Math.Floor(cooldown).ToString(CultureInfo.InvariantCulture);
                    Text.Position = new Vector2((int)spellPos.X - 30 + Text.TextValue.Length, (int)spellPos.Y + 4);
                    Text.Draw();
                }

                // Spells
                foreach (var slot in SpellSlots)
                {
                    var spell = unit.Spellbook.GetSpell(slot);
                    var cooldown = spell.CooldownExpires - Game.Time;
                    var percent = (cooldown > 0 && Math.Abs(spell.Cooldown) > float.Epsilon) ? 1f - (cooldown / spell.Cooldown) : 1f;
                    var spellPos = GetSpellOffset(unit, slot);

                    Drawing.DrawLine(new Vector2(spellPos.X, spellPos.Y + 18), new Vector2(spellPos.X + (int)(percent * 22), spellPos.Y + 18), 4, spell.IsLearned ? GetDrawColor(percent) : Color.SlateGray);

                    if (!spell.IsLearned || !(cooldown > 0)) continue;

                    Text.TextValue = Math.Floor(cooldown).ToString(CultureInfo.InvariantCulture);
                    Text.Position = new Vector2((int)spellPos.X + 10 - Text.TextValue.Length * 2, (int)spellPos.Y + 28);
                    Text.Draw();
                }

                // Draw the main hud
                MainBar.Draw(new Vector2(hpBarPos.X + OffsetHudX, hpBarPos.Y + OffsetHudY - 2));
            }
        }

        private static Vector2 GetSpellOffset(Obj_AI_Base hero, SpellSlot slot)
        {
            var normalPos = new Vector2(hero.HPBarPosition.X + OffsetSpellsX, hero.HPBarPosition.Y + OffsetSpellsY + (-7));
            switch (slot)
            {
                case SpellSlot.W:
                    return new Vector2(normalPos.X + 27, normalPos.Y);
                case SpellSlot.E:
                    return new Vector2(normalPos.X + 2 * 27, normalPos.Y);
                case SpellSlot.R:
                    return new Vector2(normalPos.X + 3 * 27, normalPos.Y);
            }
            return normalPos;
        }

        private static Vector2 GetSummonerOffset(Obj_AI_Base hero, SpellSlot slot)
        {
            var normalPos = new Vector2(hero.HPBarPosition.X + OffsetSummonersX, hero.HPBarPosition.Y + OffsetSummonersY + (-7));
            return slot == SpellSlot.Summoner2 ? new Vector2(normalPos.X, normalPos.Y + 17) : normalPos;
        }

        public static Color GetDrawColor(float percent)
        {
            if (percent < 0.3)
            {
                return Color.OrangeRed;
            }
            if (percent < 0.6)
            {
                return Color.Orange;
            }
            return percent < 1 ? Color.Green : Color.Lime;
        }

        public static void OnUnload(object sender, EventArgs e)
        {
            TextureLoader.Dispose();

            if (Text == null) return;
            Text.Dispose();
            Text = null;
        }
    }
}
