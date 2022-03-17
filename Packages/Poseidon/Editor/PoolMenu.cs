using PanettoneGames.Poseidon.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PanettoneGames.Poseidon.Menu
{
    public class PoolMenu : ScriptableObject
    {
        private static string myPubID = "46749";
        private const string resourcesPath = @"Packages/com.panettonegames.com.panettonegames.posidon/Package Resources/Assets/Resources/";
        private const string playerPoolAssetPath = resourcesPath + "PlayerPool.asset";
        private const string enemyAIPoolAssetPath = resourcesPath + "EnemyAIPool.asset";
        private const string inputActionAssetPath = resourcesPath + "PoolGameControls.inputactions";

        [MenuItem("Tools/Poseidon/Standard Player Setup", false, 11)]
        public static void PlayerPoolSetup()
        {
            //////////////disable if no object is selected
            var activeObject = Selection.activeGameObject;
            if (HasBehaviour(activeObject)) return;
            Selection.activeGameObject.AddComponent<PlayerShooting>();
            var comp = activeObject.GetComponent<PlayerShooting>();
            GameObjectPool _pool = (GameObjectPool)AssetDatabase.LoadAssetAtPath(playerPoolAssetPath, typeof(GameObjectPool));
            comp.SetPool(_pool);
            comp.SetFirePoint();

            //Assigning Fire Button
            InputActionAsset _inputAsset = AssetDatabase.LoadAssetAtPath<InputActionAsset>(inputActionAssetPath);
            var _inputAction = _inputAsset.FindAction("Fire");
            var _inputActionReference = InputActionReference.Create(_inputAction);
            comp.SetFireButton(_inputActionReference);
        }
        [MenuItem("Tools/Poseidon/Standard Player Setup", true, 11)]
        static bool Validate_PlayerPoolSetup() => Selection.activeGameObject != null;

        [MenuItem("Tools/Poseidon/Enemy AI Pool Setup", false, 11)]
        public static void EnemyAIPoolSetup()
        {
            var activeObject = Selection.activeGameObject;
            if (HasBehaviour(activeObject)) return;
            activeObject.AddComponent<EnemyAIShooting>();
            var comp = Selection.activeGameObject.GetComponent<EnemyAIShooting>();
            GameObjectPool _pool = (GameObjectPool)AssetDatabase.LoadAssetAtPath(enemyAIPoolAssetPath, typeof(GameObjectPool));
            comp.SetPool(_pool);
            comp.SetFirePoint();
        }

        [MenuItem("Tools/Poseidon/Enemy AI Pool Setup", true, 11)]
        static bool Validate_EnemyAIPoolSetup() => Selection.activeGameObject != null;
        private static bool HasBehaviour(GameObject activeObject)
        {
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
