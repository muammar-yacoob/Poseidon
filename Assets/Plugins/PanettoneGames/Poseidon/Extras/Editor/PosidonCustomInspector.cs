using PanettoneGames.Poseidon.Utilities;
using UnityEditor;
using UnityEngine;

namespace PanettoneGames.Poseidon.Menu
{
    [CustomEditor(typeof(PooledShootingBehaviour),true), CanEditMultipleObjects]
    public class PosidonCustomInspector : Editor
    {
        private GUISkin skin;
        private Texture2D iconHeader;
        private PooledShootingBehaviour pooledBehaviour;
        //private static string myPubID = "46749";
        private GameObject activeObject;
        private string headerText;
        private Rect headerRect;
        private Texture2D headerTexture;
        private float headerTexScale = 0.20f;

        void OnEnable()
        {
            skin = Resources.Load<GUISkin>("guiStyles/Default");
            headerTexture = AssetDatabase.LoadAssetAtPath("Assets/Plugins/PanettoneGames/Poseidon/Extras/Editor/Resources/icons/fork_header.png", typeof(Texture2D)) as Texture2D;

            pooledBehaviour = target as PooledShootingBehaviour;
        }

        /// <summary>
        /// Draws UI. Called every time the mouse hovers over the editor
        /// </summary>
        public override void OnInspectorGUI()
        {
            activeObject = Selection.activeGameObject;

            //Header
            GUILayout.Label($"Poseidon", skin.GetStyle("PanHeaderDefault"));
            headerRect = new Rect(Screen.width - headerTexture.width * headerTexScale, 0, headerTexture.width * headerTexScale, headerTexture.height * headerTexScale);
            GUI.DrawTexture(headerRect, headerTexture);

            //headerText = activeObject == null ? string.Empty : activeObject.name;
            //if (!string.IsNullOrEmpty(headerText)) GUILayout.Label($"Inspecting {activeObject.name}");

            base.DrawDefaultInspector();
            DrawGUI();
        }

        private void DrawGUI()
        {
            EditorGUILayout.Space(20);
            if (EditorApplication.isPlaying && pooledBehaviour is PlayerShooting)
            {
                if (GUILayout.Button("Test Fire")) pooledBehaviour.TestFire();
            }

            //showInfo = EditorGUILayout.BeginFoldoutHeaderGroup(showInfo, "Settings");
            //if (showInfo)
            //{
            //  if (activeObject != null) GUILayout.Label($"{activeObject.name} Ver: {Application.version}");
            //}
            //EditorGUILayout.EndFoldoutHeaderGroup();

            //if (GUILayout.Button("More cool tools...", skin.GetStyle("PanStoreLink")))
            //{
            //    Application.OpenURL($"https://assetstore.unity.com/publishers/" + myPubID);
            //    Application.OpenURL($"https://panettonegames.com/");
            //}
            //GUILayout.Space(5);
        }
    }
}