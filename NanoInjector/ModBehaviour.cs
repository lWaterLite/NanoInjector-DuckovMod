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

            UsageUtilities usageUtilities = newItem.gameObject.GetComponent<UsageUtilities>();
            // Register new NanoBoost Buff
            Buff? newBuff = BuffUtils.RegisterNewBuff(NanoBoost);
            if (newBuff == null) return;

            // Modify NanoBoost Effect
            GameObject effectNanoBoostGameObject = newBuff.gameObject.transform.GetChild(0).gameObject;
            effectNanoBoostGameObject.name = $"effect_{NanoBoost.DisplayNameKey}";

            // Create New Modifier
            foreach (ModifierAction action in effectNanoBoostGameObject.GetComponents<ModifierAction>())
                Destroy(action);
            effectNanoBoostGameObject.SetActive(false);
            ModifierAction nanoPhysicResist = effectNanoBoostGameObject.AddComponent<ModifierAction>();
            nanoPhysicResist.SetConfig(NanoPhysicResist);
            ModifierAction nanoWalkSpeed = effectNanoBoostGameObject.AddComponent<ModifierAction>();
            nanoWalkSpeed.SetConfig(NanoWalkSpeed);
            ModifierAction nanoRunSpeed = effectNanoBoostGameObject.AddComponent<ModifierAction>();
            nanoRunSpeed.SetConfig(NanoRunSpeed);
            effectNanoBoostGameObject.SetActive(true);

            Effect effectNanoBoost = effectNanoBoostGameObject.GetComponent<Effect>();
            object? obj = effectNanoBoost.GetPrivateField("actions");
            if (obj == null) return;
            var actions = (List<EffectAction>)obj;
            actions.Clear();
            actions.Add(nanoPhysicResist);
            actions.Add(nanoWalkSpeed);
            actions.Add(nanoRunSpeed);

            // Add UsageBehaviour
            AddBuff newAddBuff = newItem.AddComponent<AddBuff>();
            DontDestroyOnLoad(newAddBuff);
            newAddBuff.buffPrefab = newBuff;
            usageUtilities.behaviors.Clear();
            usageUtilities.behaviors.Add(newAddBuff);

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

        private static readonly ModifierInfo NanoPhysicResist = new ModifierInfo
        {
            StatKey = "ElementFactor_Physics",
            ModifierType = ModifierType.PercentageAdd,
            Value = -0.50f
        };

        private static readonly ModifierInfo NanoWalkSpeed = new ModifierInfo
        {
            StatKey = "WalkSpeed",
            ModifierType = ModifierType.PercentageAdd,
            Value = 0.3f
        };

        private static readonly ModifierInfo NanoRunSpeed = new ModifierInfo
        {
            StatKey = "RunSpeed",
            ModifierType = ModifierType.PercentageAdd,
            Value = 0.3f
        };

        #endregion
    }
}