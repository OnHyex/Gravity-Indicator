using System;
using CodeStage.AntiCheat.ObscuredTypes;
using PulsarModLoader;
using PulsarModLoader.Keybinds;
using PulsarModLoader.MPModChecks;

namespace GravityIndicator
{
    public class Mod : PulsarMod, IKeybind
    {
        public override string Version => "1.1.0";
        public override string Author => "OnHyex";
        public override string ShortDescription => "Adds gravity indicator to pilot ui";
        public override string Name => "Gravity Indicator";
        public override string HarmonyIdentifier()
        {
            return $"{Author}.{Name}";
        }
        public override int MPRequirements => (int)MPRequirement.None;
        public void RegisterBinds(KeybindManager manager)
        {
            manager.NewBind("Toggle Gravity Indicator", "gravIndicator", "Basics", "`");
        }
    }
}
