using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ItemStatsSystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NanoInjector.ModdingUtils.GameObjectUtils
{
    public static class ItemUtils
    {

        private static string ToSuffix(this EmbeddedSourceType embeddedSourceType)
        {
            return embeddedSourceType switch
            {
                EmbeddedSourceType.JPG => "jpg",
                EmbeddedSourceType.PNG => "png",
                EmbeddedSourceType.None => string.Empty,
                _ => throw new ArgumentOutOfRangeException(nameof(embeddedSourceType), embeddedSourceType, null)
            };
        }
        
        private static Sprite? LoadEmbeddedSprite(ItemInfo itemInfo)
        {
            string resourceName =
                $"NanoInjector.Assets.{itemInfo.DisplayNameKey}.{itemInfo.EmbeddedSourceType.ToSuffix()}";
            try
            {
                Stream? manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName);
                if (manifestResourceStream == null)
                {
                    Debug.LogError("LMC: Fail to find embedded resource: " + resourceName);
                    foreach (string manifestResourceName in Assembly.GetExecutingAssembly().GetManifestResourceNames())
                        Debug.Log(manifestResourceName);
                    return null;
                }
                byte[] numArray = new byte[manifestResourceStream.Length];
                int read = manifestResourceStream.Read(numArray, 0, numArray.Length);
                if (read != manifestResourceStream.Length)
                {
                    Debug.LogError($"LMC: Stream read fewer bytes than expected. The read is {read}, while expect {manifestResourceStream.Length}");
                    return null;
                }
                manifestResourceStream.Close();
                Texture2D texture2D = new(2, 2, TextureFormat.RGBA32, false);
                if (!texture2D.LoadImage(numArray))
                {
                    Debug.LogError("LMC: Fail to load image for texture.");
                    return null;
                }
                texture2D.filterMode = FilterMode.Bilinear;
                texture2D.Apply();
                Sprite sprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), 100f);
                GameObject target = new($"lmc:sprite_{itemInfo.DisplayNameKey}");
                Object.DontDestroyOnLoad(target);
                ResourceHolder resourceHolder = target.AddComponent<ResourceHolder>();
                resourceHolder.iconTexture = texture2D;
                resourceHolder.iconSprite = sprite;
                return sprite;
            }
            catch (Exception ex)
            {
                Debug.LogError($"LMC: Caught exception while loading icon - {ex}");
                return null;
            }
        }
        
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
            
            if (itemInfo.EmbeddedSourceType != EmbeddedSourceType.None)
            {
                Sprite? sprite = LoadEmbeddedSprite(itemInfo);
                if (sprite == null) return item;
                item.SetPrivateField("icon", sprite);
            }
            

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
        public EmbeddedSourceType EmbeddedSourceType = EmbeddedSourceType.None;
        public readonly Dictionary<string, object> AdditionalInfo = new();
    }
    
    public class ResourceHolder : MonoBehaviour
    {
        public Texture2D? iconTexture;
        public Sprite? iconSprite;
    }

    public enum EmbeddedSourceType
    {
        None,
        PNG,
        JPG
    }
}