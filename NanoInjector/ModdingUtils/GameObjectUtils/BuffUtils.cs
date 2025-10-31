using System.Collections.Generic;
using System.Linq;
using Duckov.Buffs;
using Duckov.Utilities;
using UnityEngine;

namespace NanoInjector.ModdingUtils.GameObjectUtils
{
    public static class BuffUtils
    {
        public static Buff? FindBuffPrefab(int id)
        {
            object? obj = GameplayDataSettings.Buffs.GetPrivateField("allBuffs");
            if (obj == null) return null;
            var buffs = (List<Buff>)obj;
            Buff? buff = buffs.FirstOrDefault(b => b.ID == id);
            if (buff == null) Debug.LogWarning($"LMC: Cannot find buff by id {id}");
            return buff;
        }

        public static Buff? RegisterNewBuff(BuffInfo buffInfo)
        {
            Buff? originalBuff = FindBuffPrefab(buffInfo.OriginalID);
            if (originalBuff == null) return null;

            Buff newBuff = Object.Instantiate(originalBuff);
            Object.DontDestroyOnLoad(newBuff);
            newBuff.name = $"lmc:buff_{buffInfo.DisplayNameKey}";
            newBuff.ID = buffInfo.NewID;
            newBuff.SetPrivateField("displayName", buffInfo.DisplayNameKey);
            newBuff.SetPrivateField("description", buffInfo.DisplayNameKey + "_Desc");
            newBuff.SetPrivateField("hide", false);
            newBuff.SetPrivateField("totalLifeTime", buffInfo.TotalLifeTime);
            if (buffInfo.AdditionalInfo == null) return newBuff;
            foreach ((string key, object value) in buffInfo.AdditionalInfo) newBuff.SetPrivateField(key, value);
            Debug.Log($"LMC: Successfully register new buff {buffInfo.NewID}");

            return newBuff;
        }
    }

    public class BuffInfo()
    {
        public string DisplayNameKey = "default";
        public int NewID;
        public int OriginalID;
        public float TotalLifeTime;
        public Dictionary<string, object>? AdditionalInfo;
        
        public BuffInfo(BuffInfo buffInfo, float buffTime) : this()
        {
            DisplayNameKey = buffInfo.DisplayNameKey;
            NewID = buffInfo.NewID;
            OriginalID = buffInfo.OriginalID;
            TotalLifeTime = buffTime;
            AdditionalInfo = buffInfo.AdditionalInfo;
        }
    }
}