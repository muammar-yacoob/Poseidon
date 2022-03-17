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

        private static string myPubID = "46749";
        private GameObject activeObject;
        private string headerText;

        void OnEnable()
        {
            showInfo = true;
            skin = Resources.Load<GUISkin>("guiStyles/Default");
        }

        /// <summary>
        /// Draws UI. Called every time the mouse hovers over the editor
        /// </summary>
        public override void OnInspectorGUI()
        {
            activeObject = Selection.activeGameObject;
            headerText = activeObject == null ? string.Empty : activeObject.name;
            GUILayout.Label($"Posidon 1.3.0", skin.GetStyle("PanHeaderDefault"));
            if (!string.IsNullOrEmpty(headerText))
            {
                GUILayout.Label($"Inspecting {activeObject.name}");
            }

            base.DrawDefaultInspector();
            DrawGUI();
        }

        private void DrawGUI()
        {
            EditorGUILayout.Space(20);
            //showInfo = EditorGUILayout.BeginFoldoutHeaderGroup(showInfo, "Poseidon", skin.GetStyle("PanHeaderDefault"));
            showInfo = EditorGUILayout.BeginFoldoutHeaderGroup(showInfo, "Settings");
            if (showInfo)
            {
                EditorGUILayout.Space(5);
                //GUILayout.Label($"Colliders");

                if (activeObject != null)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label($"{activeObject.name} Verts: {5}");
                    GUILayout.EndHorizontal();
                    EditorGUILayout.Space(5);
                }
                if (!EditorApplication.isPlaying)
                {
                    if (GUILayout.Button($"Fire"))
                    {

                    }
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            GUILayout.Space(15);

            if (GUILayout.Button("More cool tools...", skin.GetStyle("PanStoreLink")))
            {
                Application.OpenURL($"https://assetstore.unity.com/publishers/" + myPubID);
                Application.OpenURL($"https://panettonegames.com/");
            }
            GUILayout.Space(5);
        }
    }
}