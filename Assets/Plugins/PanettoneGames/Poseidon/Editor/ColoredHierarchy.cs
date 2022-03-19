using PanettoneGames.Poseidon.Utilities;
using PanettoneGames.Poseidon.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[InitializeOnLoad]
public class ColoredHierarchy : MonoBehaviour
{
    
    private static Vector2 offset = new Vector2(20, 0);
    private static GUISkin skin;
    private static GUIStyle poolActive;
    private static GUIStyle poolInactive;
    private static Texture2D iconActive;
    private static Texture2D iconInactive;
    private static Color fontColor;
    private static Color backgroundColor;
    private static GUIStyle sk;
    private static Texture2D currentIcon;
    private static DebugColors debugColors;
    private static Color bgColor;
    private const string DEBUG_COLORS = "DebugColors";
    //private GUISkin skin;

    private const string resourcesPath = @"Assets/Plugins/PanettoneGames/Poseidon/Assets/Resources/";

    static ColoredHierarchy()
    {
        InitialiseVisuals();

        var isDebugColors = EditorPrefs.GetBool(DEBUG_COLORS, true);
        if (isDebugColors)
        {
            EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
        }
    }

    private static void InitialiseVisuals()
    {
        skin = Resources.Load<GUISkin>("guiStyles/Default");
        poolActive = skin.GetStyle("GreenText");
        poolInactive = skin.GetStyle("RedText");

        iconActive = AssetDatabase.LoadAssetAtPath(resourcesPath + "fork_black.png", typeof(Texture2D)) as Texture2D;
        iconInactive = AssetDatabase.LoadAssetAtPath(resourcesPath + "fork_white.png", typeof(Texture2D)) as Texture2D;
        currentIcon = iconActive;

        debugColors = AssetDatabase.LoadAssetAtPath(resourcesPath + "DebugColors", typeof(DebugColors)) as DebugColors;
    }

    public static void SetDebug(bool status)
    {
        EditorApplication.hierarchyWindowItemOnGUI -= HandleHierarchyWindowItemOnGUI;
        if (status)
        {
            EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
        }
    }

    private void OnDestroy()
    {
        EditorApplication.hierarchyWindowItemOnGUI -= HandleHierarchyWindowItemOnGUI;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawIcon(transform.position, iconActive.name);
    }
    //private void OnEnable() => skin = Resources.Load<GUISkin>("guiStyles/Default");
    private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        if (!EditorApplication.isPlaying) return;
        var status = EditorPrefs.GetBool(DEBUG_COLORS, true);
        if (!status) return;

        Rect offsetRect = new Rect(selectionRect.position + offset, selectionRect.size);
        var obj = EditorUtility.InstanceIDToObject(instanceID);
        if (obj != null )
        {
            var go = obj as GameObject;
            
            if (obj.name.Contains("- Pool"))
            {
                //Selected
                if (Selection.instanceIDs.Contains(instanceID))
                {
                    sk = poolActive;
                    currentIcon = iconInactive;
                    bgColor = sk.onActive.textColor;
                }
                //not selected
                else
                {
                    sk = poolInactive;
                    currentIcon = iconInactive;
                    bgColor = sk.onNormal.textColor;
                }

                EditorGUI.DrawRect(selectionRect, bgColor);
                EditorGUI.LabelField(offsetRect, obj.name, sk);
                SetIcon(instanceID, selectionRect, currentIcon);
            }
            if (go.TryGetComponent(out PlayerShooting ps))
            {
                //Selected
                if (Selection.instanceIDs.Contains(instanceID))
                {
                    sk = poolActive;
                    bgColor = sk.active.textColor;
                }
                //not selected
                else
                {
                    sk = poolInactive;
                    bgColor = sk.normal.textColor;
                }

                EditorGUI.DrawRect(selectionRect, bgColor);
                EditorGUI.LabelField(offsetRect, obj.name, sk);
            }

            if (go.TryGetComponent(out EnemyAIShooting ai))
            {
                //Selected
                if (Selection.instanceIDs.Contains(instanceID))
                {
                    sk = poolActive;
                    bgColor = sk.active.textColor;
                }
                //not selected
                else
                {
                    sk = poolInactive;
                    bgColor = sk.normal.textColor;
                }

                EditorGUI.DrawRect(selectionRect, bgColor);
                EditorGUI.LabelField(offsetRect, obj.name, sk);
            }
        }
    }

    private static void SetIcon(int instanceID, Rect selectionRect, Texture2D icon)
    {
        Rect offsetRect = new Rect(selectionRect.position, selectionRect.size);
        Rect r = new Rect(selectionRect);
        r.x = r.width - 20;
        r.width = 20;
        r.position = selectionRect.position + Vector2.right;

        GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        GUI.Label(r, icon);
    }
}