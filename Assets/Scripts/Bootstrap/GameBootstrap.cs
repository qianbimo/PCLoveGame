using PCLoveGame.Core;
using UnityEngine;

namespace PCLoveGame.Bootstrap
{
    public static class GameBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            if (Object.FindObjectOfType<GameAppController>() != null)
            {
                return;
            }

            GameObject rootObject = new GameObject("GameAppController");
            rootObject.AddComponent<GameAppController>();
        }
    }
}
