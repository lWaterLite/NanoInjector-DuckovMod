using System.Collections.Generic;
using System.Linq;
using Duckov.Buffs;
using Duckov.ItemUsage;
using Duckov.Utilities;
using ItemStatsSystem;
using UnityEngine;

namespace ItemPickHint
{
    public class ModBehaviour : Duckov.Modding.ModBehaviour
    {
        protected override void OnAfterSetup()
        {
            Debug.Log("VEI: Start!");
            TestDebugInfoLogger();
            StartRegistry();
        }

        private static Buff? FindBuff(int id)
        {
            List<Buff> buffs = (List<Buff>) ItemUtils.GetPrivateField(GameplayDataSettings.Buffs, "allBuffs");
            return buffs.FirstOrDefault(b => b.ID == id);
        }

        private static void TestDebugInfoLogger()
        {
            Item prefab = ItemAssetsCollection.GetPrefab(398);
            if (prefab == null) return;
            UsageUtilities usageUtilities = prefab.UsageUtilities;
            LogHelper(usageUtilities.behaviors.Count);
            foreach (UsageBehavior behavior in usageUtilities.behaviors.Where(behavior => behavior.GetType() == typeof(AddBuff)))
            {
                int id = ((AddBuff)behavior).buffPrefab.ID;
                LogHelper(id);
                LogHelper(GameplayDataSettings.Buffs.GetBuffDisplayName(id));
            }

            // Find AddSpeed buff
            Buff? buff = FindBuff(1011);
            if (buff == null) return;
            LogHelper(buff.DisplayName);
            
            
            return;

            void LogHelper(object info)
            {
                Debug.Log($"VEI: {info}");
            }
        }

        private static void StartRegistry()
        {
            Item prefab = ItemAssetsCollection.GetPrefab(398);
            if ( prefab == null)
            {
                Debug.LogError("VEI: Original injection item has been removed.");
                return;
            }

            GameObject gameObject = Instantiate(prefab.gameObject);
            gameObject.name = "vei:new_injection_test";
            DontDestroyOnLoad(gameObject);

            Item newItem = gameObject.GetComponent<Item>();
            ItemUtils.SetItemProperties(newItem, new ItemInfo()
            {
                DisplayName = "test item",
                NewTypeId = 10086,
                Quality = 5,
                Value = 2888,
                Weight = 0.2f
            });
            
            if (ItemAssetsCollection.AddDynamicEntry(newItem))
            {
                Debug.Log("VEI: Registration complete.");
            }
            else
            {
                Debug.LogError("VEI: registration failed!");
                Destroy(gameObject);
            }
        }
    }
}