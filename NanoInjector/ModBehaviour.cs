using System;
using System.Collections.Generic;
using Duckov.Buffs;
using Duckov.ItemUsage;
using ItemStatsSystem;
using ItemStatsSystem.Stats;
using NanoInjector.ModdingUtils;
using NanoInjector.ModdingUtils.GameObjectUtils;
using Unity.VisualScripting;
using UnityEngine;

namespace NanoInjector
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        protected override void OnAfterSetup()
        {
            Debug.Log("LMC: Start registry!");
            try
            {
                TestModding();
                StartRegistry();
            }
            catch (Exception e)
            {
                Debug.LogError($"LMC: {e}");
            }
        }

        private static void TestModding()
        {
        }

        private static void StartRegistry()
        {
            // Register nano injector
            Item? newItem = ItemUtils.RegisterNewItem(NanoInjector);
            if (newItem == null) return;
            foreach (UsageBehavior behavior in newItem.gameObject.GetComponents<UsageBehavior>()) Destroy(behavior);


            // Register new NanoBoost Buff
            Buff? nanoBoostBuff = BuffUtils.RegisterNewBuff(NanoBoost);
            if (nanoBoostBuff == null) return;
            
            // Register new NanoBleedResist Buff
            Buff? nanoBleedResistBuff = BuffUtils.RegisterNewBuff(NanoBleedResist);
            if (nanoBleedResistBuff == null) return;

            // Modify NanoBoost Effect
            GameObject effectNanoBoostGameObject = nanoBoostBuff.gameObject.transform.GetChild(0).gameObject;
            effectNanoBoostGameObject.name = $"lmc:effect_{NanoBoost.DisplayNameKey}";

            // Create New Modifier
            foreach (ModifierAction action in effectNanoBoostGameObject.GetComponents<ModifierAction>())
                Destroy(action);
            effectNanoBoostGameObject.SetActive(false);
            var effectActions = new List<EffectAction>();
            foreach (ModifierInfo modifierInfo in ModifierInfos)
            {
                ModifierAction nanoModifierAction = effectNanoBoostGameObject.AddComponent<ModifierAction>();
                nanoModifierAction.SetConfig(modifierInfo);
                effectActions.Add(nanoModifierAction);
            }

            effectNanoBoostGameObject.SetActive(true);

            Effect effectNanoBoost = effectNanoBoostGameObject.GetComponent<Effect>();
            effectNanoBoost.SetPrivateField("actions", effectActions);

            // Add UsageBehaviour
            UsageUtilities usageUtilities = newItem.gameObject.GetComponent<UsageUtilities>();
            AddBuff nanoBoostAddBuff = newItem.AddComponent<AddBuff>();
            nanoBoostAddBuff.buffPrefab = nanoBoostBuff;
            AddBuff nanoBleedResistAddBuff = newItem.AddComponent<AddBuff>();
            nanoBleedResistAddBuff.buffPrefab = nanoBleedResistBuff;
            usageUtilities.behaviors.Clear();
            usageUtilities.behaviors.Add(nanoBoostAddBuff);
            usageUtilities.behaviors.Add(nanoBleedResistAddBuff);
            
            LocalizationUtils.SetLocalization();

            Debug.Log("LMC: StartUp Complete!");
        }

        #region RegisterTable

        // GameObject Name Rule: lmc:{class}_{display_name_key}

        private static readonly ItemInfo NanoInjector = new ItemInfo
        {
            DisplayNameKey = "nano_injector",
            NewTypeId = 13600,
            Quality = 5,
            Value = 2888,
            Weight = 0.2f,
            OriginalTypeId = 398 // MaxWeight Injector
        };

        private static readonly BuffInfo NanoBoost = new BuffInfo
        {
            OriginalID = 1011, // AddSpeed Buff
            NewID = 13600,
            DisplayNameKey = "nano_boost",
            AdditionalInfo = new Dictionary<string, object>
            {
                { "totalLifeTime", 90f }
            }
        };

        private static readonly BuffInfo NanoBleedResist = new BuffInfo()
        {
            OriginalID = 1491, // BleedResist Buff
            NewID = 13601,
            DisplayNameKey = "nano_bleed_resist",
            AdditionalInfo = new Dictionary<string, object>
            {
                { "totalLifeTime", 90f }
            }
        };

        private static readonly List<ModifierInfo> ModifierInfos = new List<ModifierInfo>
        {
            new ModifierInfo
            {
                StatKey = "ElementFactor_Physics",
                ModifierType = ModifierType.PercentageAdd,
                Value = -0.50f
            },
            new ModifierInfo
            {
                StatKey = "WalkSpeed",
                ModifierType = ModifierType.PercentageAdd,
                Value = 0.3f
            },
            new ModifierInfo
            {
                StatKey = "RunSpeed",
                ModifierType = ModifierType.PercentageAdd,
                Value = 0.3f
            },
            new ModifierInfo
            {
                StatKey = "Stamina",
                ModifierType = ModifierType.Add,
                Value = 40f
            },
            new ModifierInfo
            {
                StatKey = "StaminaRecoverRate",
                ModifierType = ModifierType.PercentageAdd,
                Value = 0.2f
            },
            new ModifierInfo
            {
                StatKey = "StaminaDrainRate",
                ModifierType = ModifierType.PercentageAdd,
                Value = -0.4f
            },
            new ModifierInfo
            {
                StatKey = "MeleeDamageMultiplier",
                ModifierType = ModifierType.PercentageAdd,
                Value = 0.5f
            },
            new ModifierInfo()
            {
                StatKey = "RecoilControl",
                ModifierType = ModifierType.Add,
                Value = 0.5f
            },
            new ModifierInfo()
            {
                StatKey = "GunDamageMultiplier",
                ModifierType = ModifierType.Add,
                Value = 0.5f
            }
            
        };

        #endregion
    }
}