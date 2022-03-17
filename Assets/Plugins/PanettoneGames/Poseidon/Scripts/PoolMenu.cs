using PanettoneGames.Poseidon.Utilities;
using System;
using UnityEditor;
using UnityEngine;

namespace PanettoneGames.Poseidon.Menu
{
    public class PoolMenu : ScriptableObject
    {
        private static string myPubID = "46749";

        private const string PlayerPoolAssetPath = @"Assets/Plugins/PanettoneGames/Poseidon/Assets/Resources/PlayerPool.asset";
        private const string EnemyAIPoolAssetPath = @"Assets/Plugins/PanettoneGames/Poseidon/Assets/Resources/EnemyAIPool.asset";

        [MenuItem("Tools/Poseidon/Standard Player Setup", false, 11)]
        public static void PlayerPoolSetup()
        {
            if (!HasBehaviour())
            {
                Selection.activeGameObject.AddComponent<PlayerShooting>();
            }
            var comp = Selection.activeGameObject.GetComponent<PlayerShooting>();
            GameObjectPool _pool = (GameObjectPool)AssetDatabase.LoadAssetAtPath(PlayerPoolAssetPath, typeof(GameObjectPool));
            comp.SetPool(_pool);
        }

        [MenuItem("Tools/Poseidon/Enemy AI Pool Setup", false, 11)]
        public static void EnemyAIPoolSetup()
        {
            if (!HasBehaviour())
            {
                Selection.activeGameObject.AddComponent<EnemyAIShooting>();
            }
            var comp = Selection.activeGameObject.GetComponent<EnemyAIShooting>();
            GameObjectPool _pool = (GameObjectPool)AssetDatabase.LoadAssetAtPath(EnemyAIPoolAssetPath, typeof(GameObjectPool));
            comp.SetPool(_pool);
        }

        private static bool HasBehaviour()
        {
            var activeObject = Selection.activeGameObject;
            var found = activeObject.GetComponent<PooledShootingBehaviour>() != null;
            if (found)
            {
                Debug.LogWarning($"{activeObject.name} already contains a shooting behaviour");
            }
            return found;
        }

        [MenuItem("Tools/Poseidon/Rate Please :)", false, 30)]
        public static void MenuRate() => Application.OpenURL($"https://assetstore.unity.com/packages/tools/utilities/poseidon-simple-pooling-system-201537?aid=1011lds77&utm_source=aff#reviews");

        [MenuItem("Tools/Poseidon/Help", false, 31)]
        public static void MenuHelp()
        {
            Application.OpenURL(@"https://panettonegames.com/");

            string helpFilePath = Application.dataPath + @"/Plugins/PanettoneGames/Poseidon/Poseidon Read Me.pdf";
            Debug.Log($"Help file is in: {helpFilePath}");
            Application.OpenURL(helpFilePath);
            Application.OpenURL($"https://assetstore.unity.com/publishers/" + myPubID + "?aid=1011lds77");
        }


    }
}
