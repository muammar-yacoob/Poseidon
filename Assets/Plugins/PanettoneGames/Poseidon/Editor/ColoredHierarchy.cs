using System.Linq;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class ColoredHierarchy : MonoBehaviour
{
    private static Vector2 offset = new Vector2(20, 0);
    private static Texture2D iconActive;
    private static Texture2D iconInactive;
    private static Color fontColor;
    private static Color backgroundColor;
    private static Texture2D currentIcon;
    private const string DEBUG_COLORS = "DebugColors";
    //private GUISkin skin;

    static ColoredHierarchy()
    {
        iconActive = AssetDatabase.LoadAssetAtPath("Assets/Plugins/PanettoneGames/Poseidon/Editor/Resources/icons/fork_black.png", typeof(Texture2D)) as Texture2D;
        iconInactive = AssetDatabase.LoadAssetAtPath("Assets/Plugins/PanettoneGames/Poseidon/Editor/Resources/icons/fork_white.png", typeof(Texture2D)) as Texture2D;

        fontColor = Color.blue;
        backgroundColor = Color.cyan;// new Color(0.5f,0.8f,0.8f);
        currentIcon = iconActive;

        var isDebugColors = EditorPrefs.GetBool(DEBUG_COLORS, true);
        if (isDebugColors)
        {
            EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
        }
    }

    public static void SetDebug(bool status)
    {
        EditorApplication.hierarchyWindowItemOnGUI -= HandleHierarchyWindowItemOnGUI;
        if (status)
        {
            EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
        }
        EditorApplication.RepaintHierarchyWindow();
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

        var obj = EditorUtility.InstanceIDToObject(instanceID);
        if (obj != null)
        {
            //var go = obj as GameObject;
            //if (go.TryGetComponent(out BoxCollider box))
            if (obj.name.Contains("- Pool"))
            {


                if (Selection.instanceIDs.Contains(instanceID))
                {
                    fontColor = Color.white;
                    backgroundColor = new Color(0.24f, 0.48f, 0.90f);
                    currentIcon = iconInactive;
                }
                else
                {
                    fontColor = Color.blue;
                    backgroundColor = Color.cyan;
                    currentIcon = iconActive;
                }

                Rect offsetRect = new Rect(selectionRect.position + offset, selectionRect.size);
                EditorGUI.DrawRect(selectionRect, backgroundColor);
                EditorGUI.LabelField(offsetRect, obj.name, new GUIStyle()
                {
                    normal = new GUIStyleState() { textColor = fontColor },
                    fontStyle = FontStyle.Bold
                }
                );
                SetIcon(instanceID, selectionRect, currentIcon);
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