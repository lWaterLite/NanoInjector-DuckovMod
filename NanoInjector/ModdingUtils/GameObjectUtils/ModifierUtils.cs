using ItemStatsSystem.Stats;

namespace NanoInjector.ModdingUtils.GameObjectUtils
{
    public static class ModifierUtils
    {
        
    }

    public class ConfigModifierAction : ModifierAction
    {
        private bool _isConfig;
        
        protected override void Awake()
        {
            if (!_isConfig) return;
            base.Awake();
        }

        public void SetConfig(ModifierInfo info)
        {
            _isConfig = true;
            enabled = true;
            targetStatKey = info.StatKey;
            ModifierType = info.ModifierType;
            modifierValue = info.Value;
            
            base.Awake();
        }
    }

    public class ModifierInfo
    {
        public ModifierType ModifierType;
        public string StatKey = "default";
        public float Value;
    }
}