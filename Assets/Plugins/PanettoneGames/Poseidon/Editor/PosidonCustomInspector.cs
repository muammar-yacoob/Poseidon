using PanettoneGames.Poseidon.Utilities;
using UnityEditor;
using UnityEngine;

namespace PanettoneGames.Poseidon.Menu
{
    [CustomEditor(typeof(PooledShootingBehaviour), true)]
    public class PosidonCustomInspector : Editor
    {
        private bool showInfo;
        private GUISkin skin;
        private PooledShootingBehaviour pooledBehaviour;
        private static string myPubID = "46749";
        private GameObject activeObject;
        private string headerText;

        

        void OnEnable()
        {
            showInfo = true;
            skin = Resources.Load<GUISkin>("guiStyles/Default");
            pooledBehaviour = target as PooledShootingBehaviour;
        }

        /// <summary>
        /// Draws UI. Called every time the mouse hovers over the editor
        /// </summary>
        public override void OnInspectorGUI()
        {
            activeObject = Selection.activeGameObject;
            GUILayout.Label($"Poseidon", skin.GetStyle("PanHeaderDefault"));
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