using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using PanettoneGames.Poseidon.Utilities;
using AudioManager = PanettoneGames.Poseidon.Utilities.AudioManager;
using System.Linq;
using PanettoneGames.Poseidon.Core;

namespace PanettoneGames.Poseidon.Menu
{
    public class PoolMenu : ScriptableObject
    {
        private static string myPubID = "46749";
        private static GameObjectPool _player2DPool;
        private static GameObjectPool _enemy2DPool;
        private static GameObjectPool _player3DPool;
        private static GameObjectPool _enemy3DPool;

        private static InputActionReference _inputActionReference;
        private const string resourcesPath = @"Assets/Plugins/PanettoneGames/Poseidon/Extras/Assets/Resources/";

        private const string player2DPoolAssetPath = resourcesPath + "PlayerPool_2D.asset";
        private const string enemy2DAIPoolAssetPath = resourcesPath + "EnemyAIPool_2D.asset";
        private const string player3DPoolAssetPath = resourcesPath + "PlayerPool_3D.asset";
        private const string enemy3DAIPoolAssetPath = resourcesPath + "EnemyAIPool_3D.asset";

        private const string inputActionAssetPath = resourcesPath + "PoolGameControls.inputactions";

        private const string DEBUG_MENU_NAME = "Tools/Poseidon/Debug Colors";
        private const string DEBUG_COLORS = "DebugColors";
        private static bool isDebugColors
        {
            get { return EditorPrefs.GetBool(DEBUG_COLORS, true); }
            set { EditorPrefs.SetBool(DEBUG_COLORS, value); }
        }
        private static void InitializeResources()
        {
            _player2DPool = (GameObjectPool)AssetDatabase.LoadAssetAtPath(player2DPoolAssetPath, typeof(GameObjectPool));
            _enemy2DPool = (GameObjectPool)AssetDatabase.LoadAssetAtPath(enemy2DAIPoolAssetPath, typeof(GameObjectPool));

            _player3DPool = (GameObjectPool)AssetDatabase.LoadAssetAtPath(player3DPoolAssetPath, typeof(GameObjectPool));
            _enemy3DPool = (GameObjectPool)AssetDatabase.LoadAssetAtPath(enemy3DAIPoolAssetPath, typeof(GameObjectPool));

            InputActionAsset _inputAsset = AssetDatabase.LoadAssetAtPath<InputActionAsset>(inputActionAssetPath);
            var _inputAction = _inputAsset.FindAction("Fire");
            _inputActionReference = InputActionReference.Create(_inputAction);
        }

        [MenuItem("Tools/Poseidon/Standard Player Setup", false, 11)]
        public static void PlayerPoolSetup()
        {
            var selectedObjects = Selection.gameObjects;
            if (selectedObjects.Length < 1) return;
            InitializeResources();

            foreach (var obj in selectedObjects)
            {
                if (HasBehaviour(obj)) continue;
                obj.AddComponent<PlayerShooting>();
                var comp = obj.GetComponent<PlayerShooting>();
                comp.SetFirePoint();

                //Assigning Pool
                if (_player2DPool == null || _player3DPool == null)
                {
                    Debug.LogError("pool asset is missing");
                }
                else
                {
                    var mesh = obj.GetComponentsInChildren<MeshRenderer>().ToList().FirstOrDefault();
                    var p = (mesh == null)? _player2DPool: _player3DPool;
                    comp.SetPool(p);
                }

                //Assigning Fire button
                if (_inputActionReference == null)
                {
                    Debug.LogError("Fire action is missing");
                }
                else
                {
                    comp.SetFireButton(_inputActionReference);
                }
            }
        }
        [MenuItem("Tools/Poseidon/Standard Player Setup", true, 11)]
        static bool Validate_PlayerPoolSetup() => Selection.gameObjects.Length > 0 && !EditorApplication.isPlaying;

        [MenuItem("Tools/Poseidon/Enemy AI Pool Setup", false, 12)]
        public static void EnemyAIPoolSetup()
        {
            var selectedObjects = Selection.gameObjects;
            if (selectedObjects.Length < 1) return;
            InitializeResources();

            foreach (var obj in selectedObjects)
            {
                if (HasBehaviour(obj)) continue;
                obj.AddComponent<EnemyAIShooting>();
                var comp = obj.GetComponent<EnemyAIShooting>();
                comp.SetFirePoint();

                if (_enemy2DPool == null || _enemy3DPool == null)
                {
                    Debug.LogError("pool asset is missing");
                }
                else
                {
                    var mesh = obj.GetComponentsInChildren<MeshRenderer>().ToList().FirstOrDefault();
                    var p = (mesh == null) ? _enemy2DPool : _enemy3DPool;
                    comp.SetPool(p);
                }
            }
        }

        [MenuItem("Tools/Poseidon/Enemy AI Pool Setup", true, 12)]
        static bool Validate_EnemyAIPoolSetup() => Selection.gameObjects.Length > 0 && !EditorApplication.isPlaying;


        private static bool HasBehaviour(GameObject activeObject)
        {
            var found = activeObject.GetComponent<PooledShootingBehaviour>() != null;
            if (found)
            {
                Debug.LogWarning($"{activeObject.name} already contains a shooting behaviour");
            }
            return found;
        }

        #region Audio Manager
        [MenuItem("Tools/Poseidon/Audio Manager Setup", false, 13)]
        public static void AudioManagerSetup()
        {
            var audioManager = FindObjectOfType<AudioManager>();
            if (audioManager != null)
            {
                Debug.LogWarning($"Scene already contains {audioManager.name}");
                return;
            }

            var mt = new GameObject();
            mt.transform.position = Vector3.zero;
            mt.AddComponent<AudioManager>();
            mt.name = "Audio Manager";
        }
        [MenuItem("Tools/Poseidon/Audio Manager Setup", true, 13)]
        static bool Validate_AudioManagerSetup() =>  !EditorApplication.isPlaying;
        #endregion


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

        #region DebugColors
        [MenuItem(DEBUG_MENU_NAME)]
        private static void PerformAction()
        {
            isDebugColors = !isDebugColors;
            var onOff = isDebugColors ? "On" : "Off";
            UnityEngine.Debug.Log($"Debug Colors is turned {onOff}");
            ColoredHierarchy.SetDebug(isDebugColors);
        }

        [MenuItem(DEBUG_MENU_NAME, true)]
        private static bool DebugMenuColors()
        {
            UnityEditor.Menu.SetChecked(DEBUG_MENU_NAME, isDebugColors);
            return true;
        }


        #endregion
    }
}