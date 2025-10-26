using System;
using System.Collections.Generic;
using System.Linq;
using Duckov.Buffs;
using Duckov.ItemUsage;
using ItemPickHint.ModdingUtils;
using ItemPickHint.ModdingUtils.GameObjectUtils;
using ItemStatsSystem;
using UnityEngine;

namespace ItemPickHint
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
            OriginalID = 1011,
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
                StartRegistry();
            }
            catch (Exception e)
            {
                Debug.LogError($"LMC: {e}");
            }
            
        }

        private static void StartRegistry()
        {
            // Register nano injector
            Item? newItem = ItemUtils.RegisterNewItem(NanoInjector);
            if (newItem == null) return;

            Buff? newBuff = BuffUtils.CreateNewBuff(NewAddSpeed);
            if (newBuff == null) return;

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