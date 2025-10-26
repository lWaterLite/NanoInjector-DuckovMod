using System.Collections.Generic;
using System.Linq;
using Duckov.Buffs;
using Duckov.Utilities;
using UnityEngine;

namespace ItemPickHint.ModdingUtils.GameObjectUtils
{
    public static class BuffUtils
    {
        public static Buff? FindBuff(int id)
        {
            object? obj = ReflectionUtils.GetPrivateField(GameplayDataSettings.Buffs, "allBuffs");
            if (obj == null) return null;
            List<Buff> buffs = (List<Buff>) obj;
            return buffs.FirstOrDefault(b => b.ID == id);
        }

        public static Buff? CreateNewBuff(BuffInfo buffInfo)
        {
            Buff? originalBuff = FindBuff(buffInfo.OriginalID);
            if (originalBuff == null)
            {
                Debug.LogError($"LMC: Cannot find buff by id {buffInfo.OriginalID}");
                return null;
            }
            Debug.Log($"LMC: Successfully find Buff id {buffInfo.OriginalID}");

            Buff newBuff =
                GameObjectUtils.InstantiateNewGameObject<Buff>(originalBuff.gameObject, buffInfo.GameObjectName);
            newBuff.ID = buffInfo.NewID;
            if (buffInfo.AdditionalInfo == null) return newBuff;
            foreach ((string key, object value) in buffInfo.AdditionalInfo)
            {
                newBuff.SetPrivateField(key, value);
            }

            return newBuff;
        }
    }

    public class BuffInfo
    {
        public int OriginalID;
        public int NewID;
        public string GameObjectName = "default";
        public Dictionary<string, object>? AdditionalInfo;
    }
}