using System;
using System.Reflection;
using ItemStatsSystem;
using UnityEngine;

namespace ItemPickHint
{
    public static class ItemUtils
    {
        public static Item SetPrivateField(this Item item, string fieldName, object value)
        {
            FieldInfo? fieldInfo = typeof(Item).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (fieldInfo == null)
            {
                Debug.LogWarning($"VEI: Cannot find field {fieldName}, field name could been changed.");
                return item;
            }
            fieldInfo.SetValue(item, value);
            return item;
        }

        public static object GetPrivateField(object target, string fieldName)
        {
            FieldInfo? info = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (info == null) throw new InvalidOperationException($"object {target} don't have field {fieldName}.");
            return info.GetValue(target);
        }
        
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