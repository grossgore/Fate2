﻿#region Use
using System.Drawing;
using LeagueSharp.Common; 
#endregion

namespace TwistedFate
{
    internal static class Config
    {
        #region Properties

        internal static Menu QMenu { get; private set; }

        internal static Menu WMenu { get; private set; }

        internal static Menu ExtraMenu { get; private set; }

        internal static Menu DrawMenu { get; private set; }

        internal static Menu OrbwalkMenu { get; private set; }

        internal static Menu TargetSelectorMenu { get; private set; }

        internal static Menu TwistedFateMenu { get; private set; }

        #endregion

        #region Methods

        internal static void BuildConfig()
        {
            TwistedFateMenu = new Menu("Apdo Fate", "twistedfate", true).SetFontStyle(FontStyle.Bold, SharpDX.Color.BlueViolet);

            TargetSelectorMenu = new Menu("Target Selector", "targetselector");
            TargetSelector.AddToMenu(TargetSelectorMenu);
            TwistedFateMenu.AddSubMenu(TargetSelectorMenu);

            OrbwalkMenu = new Menu("Orbwalker", "orbwalker");
            Mainframe.Orbwalker = new Orbwalking.Orbwalker(OrbwalkMenu);
            TwistedFateMenu.AddSubMenu(OrbwalkMenu);

            QMenu = new Menu("Q Spell", "qSpellMenu");
            QMenu.AddItem(new MenuItem("qKeys", "Q - Key Bindings")).SetFontStyle(FontStyle.Bold, SharpDX.Color.BlueViolet);
            QMenu.AddItem(
                new MenuItem("qClear", "Q Wave Clear (hold)").SetValue(
                    new KeyBind("A".ToCharArray()[0], KeyBindType.Press)));
            QMenu.AddItem(new MenuItem("qClearCount", "X min Creeps to hit").SetValue(new Slider(3, 2, 5)));
            QMenu.AddItem(
                new MenuItem("qEnemy", "Q Enemy (hold)").SetValue(
                    new KeyBind("C".ToCharArray()[0], KeyBindType.Press)));
            QMenu.AddItem(new MenuItem("qAuto", "Q - Automated")).SetFontStyle(FontStyle.Bold, SharpDX.Color.BlueViolet);
            QMenu.AddItem(new MenuItem("qAMana", "X min Mana to use Auto Q").SetValue(new Slider(10, 0, 100)));
            QMenu.AddItem(new MenuItem("qDashing", "if target is dashing").SetValue(false));
            QMenu.AddItem(new MenuItem("qSlowed", "if target is slowed").SetValue(true));
            QMenu.AddItem(new MenuItem("qImmobile", "if target is immobile").SetValue(true));
            QMenu.AddItem(new MenuItem("qKS", "killsteal").SetValue(true));
            QMenu.AddItem(new MenuItem("qOptimize", "Optimizer")).SetFontStyle(FontStyle.Bold, SharpDX.Color.BlueViolet);
            QMenu.AddItem(new MenuItem("qAfterW", "Predict Gold/Red into Q").SetValue(true));
            TwistedFateMenu.AddSubMenu(QMenu);

            WMenu = new Menu("W Spell", "wSpellMenu");
            WMenu.AddItem(new MenuItem("wQuick", "Harass")).SetFontStyle(FontStyle.Bold, SharpDX.Color.BlueViolet);
            WMenu.AddItem(new MenuItem("wHarass", "Rotate cards").SetValue(true));
            WMenu.AddItem(new MenuItem("wHMana", "X min Mana to Rotate cards").SetValue(new Slider(20, 0, 100))); 
            WMenu.AddItem(new MenuItem("wHRange", "Rotate if target in AA range + X").SetValue(new Slider(250, 100, 250)));
            WMenu.AddItem(new MenuItem("wSelector", "Koratu's Cards Selector")).SetFontStyle(FontStyle.Bold, SharpDX.Color.BlueViolet);
            WMenu.AddItem(
                new MenuItem("csGold", "GOLD").SetValue(new KeyBind("O".ToCharArray()[0], KeyBindType.Press)));
            WMenu.AddItem(
                new MenuItem("csBlue", "BLUE").SetValue(
                    new KeyBind("U".ToCharArray()[0], KeyBindType.Press)));
            WMenu.AddItem(
                new MenuItem("csRed", "RED").SetValue(new KeyBind("I".ToCharArray()[0], KeyBindType.Press)));
            WMenu.AddItem(new MenuItem("wMiscs", "Miscs")).SetFontStyle(FontStyle.Bold, SharpDX.Color.BlueViolet);
            WMenu.AddItem(new MenuItem("wCGold", "Always pick GOLD in COMBO").SetValue(true));
            TwistedFateMenu.AddSubMenu(WMenu);

            ExtraMenu = new Menu("More+", "extraMenu");
            ExtraMenu.AddItem(new MenuItem("goldInter", "Interrupter").SetValue(true));
            ExtraMenu.AddItem(new MenuItem("goldGap", "Anti-GapCloser").SetValue(true));
            TwistedFateMenu.AddSubMenu(ExtraMenu);

            DrawMenu = new Menu("Drawings", "drawings");
            DrawMenu.AddItem(new MenuItem("drawQrange", "Q Range").SetValue(true));
            DrawMenu.AddItem(new MenuItem("drawRrange", "R Range").SetValue(true));
            DrawMenu.AddItem(new MenuItem("drawRmap", "R Range Minimap").SetValue(true));
            DrawMenu.AddItem(new MenuItem("drawComboDmg", "Combo Damage").SetValue(true));
            DrawMenu.AddItem(new MenuItem("fillDmg", "Damage Fill Color").SetValue(new Circle(true, Color.LightSteelBlue)));
            TwistedFateMenu.AddSubMenu(DrawMenu);

            #region DamageDrawings

            DrawingDamage.DamageToUnit = Computed.GetComboDamage;
            DrawingDamage.Enabled = TwistedFateMenu.Item("drawComboDmg").GetValue<bool>();
            DrawingDamage.Fill = TwistedFateMenu.Item("fillDmg").GetValue<Circle>().Active;
            DrawingDamage.FillColor = TwistedFateMenu.Item("fillDmg").GetValue<Circle>().Color;

            TwistedFateMenu.Item("drawComboDmg").ValueChanged +=
            delegate (object sender, OnValueChangeEventArgs eventArgs)
            {
                DrawingDamage.Enabled = eventArgs.GetNewValue<bool>();
            };

            TwistedFateMenu.Item("fillDmg").ValueChanged +=
            delegate (object sender, OnValueChangeEventArgs eventArgs)
            {
                DrawingDamage.Fill = eventArgs.GetNewValue<Circle>().Active;
                DrawingDamage.FillColor = eventArgs.GetNewValue<Circle>().Color;
            };

            #endregion

            TwistedFateMenu.AddToMainMenu();
        }

        #region Getters

        internal static int GetSliderValue(string itemName)
        {
            return TwistedFateMenu.Item(itemName).GetValue<Slider>().Value;
        }

        internal static int GetStringListValue(string itemName)
        {
            return TwistedFateMenu.Item(itemName).GetValue<StringList>().SelectedIndex;
        }

        internal static bool IsChecked(string itemName)
        {
            return TwistedFateMenu.Item(itemName).GetValue<bool>();
        }

        internal static bool IsKeyPressed(string itemName)
        {
            return TwistedFateMenu.Item(itemName).GetValue<KeyBind>().Active;
        }

        internal static bool DrawQ { get { return Config.IsChecked("drawQrange"); } }

        internal static bool DrawR { get { return IsChecked("drawRrange"); } }

        internal static bool DrawRm { get { return IsChecked("drawRmap"); } }

        internal static bool UseInterrupter { get { return IsChecked("goldInter"); } }

        internal static bool UseAntiGapCloser { get { return IsChecked("goldGap"); } }

        internal static bool GoldKey { get { return TwistedFateMenu.Item("csGold").GetValue<KeyBind>().Active; } }

        internal static bool BlueKey { get { return TwistedFateMenu.Item("csBlue").GetValue<KeyBind>().Active; } }

        internal static bool RedKey { get { return TwistedFateMenu.Item("csRed").GetValue<KeyBind>().Active; } }

        internal static bool UseClearQ { get { return TwistedFateMenu.Item("qClear").GetValue<KeyBind>().Active; } }

        internal static int ClearQCount { get { return TwistedFateMenu.Item("qClearCount").GetValue<Slider>().Value; } }

        internal static bool UseQEnemy { get { return TwistedFateMenu.Item("qEnemy").GetValue<KeyBind>().Active; } }

        internal static bool Rotate { get { return IsChecked("wHarass"); } }

        internal static int RotateRange { get { return TwistedFateMenu.Item("wHRange").GetValue<Slider>().Value; } }

        internal static int RotateMana { get { return TwistedFateMenu.Item("wHMana").GetValue<Slider>().Value; } }

        internal static bool UseGoldCombo { get { return IsChecked("wCGold"); } }

        internal static int AutoqMana { get { return TwistedFateMenu.Item("qAMana").GetValue<Slider>().Value; } }

        internal static bool IsImmobile { get { return IsChecked("qImmobile"); } }

        internal static bool IsDashing { get { return IsChecked("qDashing"); } }

        internal static bool IsSlowed { get { return IsChecked("qSlowed"); } }

        internal static bool CanqKS { get { return IsChecked("qKS"); } }

        internal static bool PredictQ { get { return IsChecked("qAfterW"); } }

        #endregion

        #endregion
    }
}