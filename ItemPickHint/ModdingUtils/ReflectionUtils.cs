using System;
using System.Reflection;
using UnityEngine;

namespace ItemPickHint.ModdingUtils
{
    public static class ReflectionUtils
    {
        public static object SetPrivateField(this object target, string fieldName, object? value)
        {
            FieldInfo? fieldInfo = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (fieldInfo == null)
            {
                Debug.LogWarning($"LMC: Cannot find field {fieldName} in object {target}, fail to set value.");
                return target;
            }
            fieldInfo.SetValue(target, value);
            return target;
        }

        public static object? GetPrivateField(object target, string fieldName)
        {
            FieldInfo? info = target.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (info != null) return info.GetValue(target);
            Debug.LogWarning($"LMC: Cannot find field {fieldName} in object {target}, fail to get value.");
            return null;
        }
    }
}