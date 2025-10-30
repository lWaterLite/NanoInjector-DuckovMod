using System.Collections.Generic;
using ItemStatsSystem;
using UnityEngine;

namespace NanoInjector.ModdingUtils.GameObjectUtils
{
    public static class ItemUtils
    {
        private static void SetItemProperties(Item target, ItemInfo itemInfo)
        {
            target
                .SetPrivateField("typeID", itemInfo.NewTypeId)
                .SetPrivateField("value", itemInfo.Value)
                .SetPrivateField("quality", itemInfo.Quality)
                .SetPrivateField("weight", itemInfo.Weight)
                .SetPrivateField("displayName", itemInfo.DisplayNameKey);
            
            if (itemInfo.AdditionalInfo.Count == 0) return;
            foreach ((string? key, object? value) in itemInfo.AdditionalInfo)
            {
                target.SetPrivateField(key, value);
            }
        }

        public static Item? GetItemPrefab(int typeID)
        {
            Item prefab = ItemAssetsCollection.GetPrefab(typeID);
            if (prefab != null) return prefab;
            Debug.LogError($"LMC: Cannot find original item by id {typeID}");
            return null;
        }

        public static Item? RegisterNewItem(ItemInfo itemInfo)
        {
            Item? prefab = GetItemPrefab(itemInfo.OriginalTypeId);
            if (prefab == null) return null;

            GameObject itemGameObject = Object.Instantiate(prefab.gameObject);
            itemGameObject.name = $"lmc:item_{itemInfo.DisplayNameKey}";
            Object.DontDestroyOnLoad(itemGameObject);
            Item item = itemGameObject.GetComponent<Item>();

            SetItemProperties(item, itemInfo);

            if (ItemAssetsCollection.AddDynamicEntry(item))
            {
                Debug.Log($"LMC: Successfully register new item {itemInfo.NewTypeId}");
                return item;
            }

            Debug.LogWarning($"LMC: Fail to register new item {itemInfo.NewTypeId}");
            Object.Destroy(item);
            return null;
        }
    }

    public class ItemInfo
    {
        public string DisplayNameKey = "default";
        public int NewTypeId;
        public int OriginalTypeId;
        public int Quality;
        public int Value;
        public float Weight;
        public Dictionary<string, object> AdditionalInfo = new Dictionary<string, object>();
    }
}