using System;
using System.Reflection;
using ItemStatsSystem;
using UnityEngine;

namespace ItemPickHint.ModdingUtils
{
    public static class ItemUtils
    {
        public static void SetItemProperties(Item target, ItemInfo itemInfo)
        {
            target
                .SetPrivateField("typeID", itemInfo.NewTypeId)
                .SetPrivateField("value", itemInfo.Value)
                .SetPrivateField("quality", itemInfo.Quality)
                .SetPrivateField("weight", itemInfo.Weight)
                .SetPrivateField("displayName", itemInfo.DisplayName);
        }
    }

    public class ItemInfo
    {
        public int NewTypeId;
        public int Value;
        public int Quality;
        public float Weight;
        public string DisplayName = "test";
        
    }
}