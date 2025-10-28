using ItemStatsSystem.Stats;

namespace NanoInjector.ModdingUtils.GameObjectUtils
{
    public static class ModifierUtils
    {
        public static void SetConfig(this ModifierAction action, ModifierInfo info)
        {
            action.enabled = true;
            action.targetStatKey = info.StatKey;
            action.ModifierType = info.ModifierType;
            action.modifierValue = info.Value;
            // action.SetPrivateField("modifier",
            //     new Modifier(action.ModifierType, action.modifierValue, action.overrideOrder, action.overrideOrderValue,
            //         action.Master));
            // action.SetPrivateField("targetStatHash", action.targetStatKey.GetHashCode());
        }
    }

    public class ModifierInfo
    {
        public ModifierType ModifierType;
        public string StatKey = "default";
        public float Value;
    }
}