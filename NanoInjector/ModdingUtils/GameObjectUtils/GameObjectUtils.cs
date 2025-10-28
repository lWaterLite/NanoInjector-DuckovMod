using UnityEngine;
using Object = UnityEngine.Object;

namespace NanoInjector.ModdingUtils.GameObjectUtils
{
    public static class GameObjectUtils
    {
        public static T InstantiateNewGameObject<T>(GameObject go, string gameObjectName)
        {
            GameObject newGameObject = Object.Instantiate(go);
            newGameObject.name = gameObjectName;
            Object.DontDestroyOnLoad(newGameObject);
            return newGameObject.GetComponent<T>();
        }
    }
}