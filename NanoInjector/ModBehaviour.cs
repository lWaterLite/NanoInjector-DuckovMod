using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Buffs;
using Duckov.ItemUsage;
using ItemStatsSystem;
using NanoInjector.ModdingUtils;
using NanoInjector.ModdingUtils.GameObjectUtils;
using UnityEngine;

namespace NanoInjector
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        #region RegisterTable

        private static readonly ItemInfo NanoInjector = new ItemInfo
        {
            DisplayName = "纳米激素",
            NewTypeId = 13600,
            Quality = 5,
            Value = 2888,
            Weight = 0.2f,
            OriginalTypeId = 398, // MaxWeight Injector
            GameObjectName = "nano_injector",
        };

        private static readonly BuffInfo NewAddSpeed = new BuffInfo
        {
            OriginalID = 1011, // AddSpeed Buff
            NewID = 13600,
            GameObjectName = "new_add_speed",
            AdditionalInfo = new Dictionary<string, object>
            {
                {"totalLifeTime", 90f}
            }
        };

        #endregion

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
            // Buff? addSpeed = BuffUtils.FindBuff(1011);
            // if (addSpeed == null) return;
            // object? obj = ReflectionUtils.GetPrivateField(addSpeed, "effects");
            // if (obj == null) return;
            // List<Effect> effects = (List<Effect>)obj;
            // Effect addSpeedEffect = effects.First();
            // ModifierAction[] actions = addSpeedEffect.GetComponents<ModifierAction>();
            // foreach (ModifierAction action in actions)
            // {
            //     action.modifierValue = 1.0f;
            // }
            
            Debug.Log("Test Complete.");
        }

        private static void StartRegistry()
        {
            // Register nano injector
            Item? newItem = ItemUtils.RegisterNewItem(NanoInjector);
            if (newItem == null) return;

            // Create new AddSpeed Buff
            Buff? newBuff = BuffUtils.CreateNewBuff(NewAddSpeed);
            if (newBuff == null) return;
            
            // Modify AddSpeed Effect
            object? obj = ReflectionUtils.GetPrivateField(newBuff, "effects");
            if (obj == null) return;
            List<Effect> effects = (List<Effect>)obj;
            Effect addSpeedEffect = effects.First();
            ModifierAction[] actions = addSpeedEffect.GetComponents<ModifierAction>();
            foreach (ModifierAction action in actions)
            {
                action.modifierValue = 0.5f;
            }

            UsageUtilities usageUtilities = newItem.UsageUtilities;
            UsageBehavior? addBuffPrefab =
                usageUtilities.behaviors.FirstOrDefault(behavior => behavior.GetType() == typeof(AddBuff));
            if (addBuffPrefab == null)
            {
                Debug.LogError("LMC: Can't find original AddBuff instance. Fail to Register.");
                return;
            }

            AddBuff newAddBuff = GameObjectUtils.InstantiateNewGameObject<AddBuff>(addBuffPrefab.gameObject, "lmc:add_buff_new_add_speed");
            newAddBuff.buffPrefab = newBuff;
            usageUtilities.behaviors.Clear();
            usageUtilities.behaviors.Add(newAddBuff);

            Debug.Log("LMC: StartUp Complete!");
        }
    }
}