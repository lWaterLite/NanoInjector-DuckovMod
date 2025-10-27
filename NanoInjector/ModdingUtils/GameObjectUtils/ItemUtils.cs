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
                .SetPrivateField("displayName", itemInfo.DisplayName);
        }

        public static Item? RegisterNewItem(ItemInfo itemInfo)
        {
            Item prefab = ItemAssetsCollection.GetPrefab(itemInfo.OriginalTypeId);
            if (prefab == null)
            {
                Debug.LogError($"LMC: Cannot find original item by id {itemInfo.OriginalTypeId}, exit item registration.");
                return null;
            }

            Item item = GameObjectUtils.InstantiateNewGameObject<Item>(prefab.gameObject, itemInfo.GameObjectName);
            SetItemProperties(item, itemInfo);

            if (ItemAssetsCollection.AddDynamicEntry(item))
            {
                Debug.Log($"LMC: Successfully register new item id {itemInfo.NewTypeId}");
                return item;
            }
            Debug.LogError($"LMC: Fail to register new item id {itemInfo.NewTypeId}");
            Object.Destroy(item);
            return null;
        }
    }

    public class ItemInfo
    {
        public int OriginalTypeId;
        public int NewTypeId;
        public int Value;
        public int Quality;
        public float Weight;
        public string DisplayName = "default";
        public string GameObjectName = "default";
        public string LocalizationKey => GameObjectName;
        public string LocalizationDesc => GameObjectName + "_Desc";

    }
}