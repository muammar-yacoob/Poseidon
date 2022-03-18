using System.Linq;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class ColoredHierarchy : MonoBehaviour
{
    private static Vector2 offset = new Vector2(20, 0);
    //private GUISkin skin;

    static ColoredHierarchy() => EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
    //public static void SetDebug() => EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;

    private void OnDestroy()
    {
        EditorApplication.hierarchyWindowItemOnGUI -= HandleHierarchyWindowItemOnGUI;
    }

    //private void OnEnable() => skin = Resources.Load<GUISkin>("guiStyles/Default");
    private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        Color fontColor = Color.blue;
        Color backgroundColor = Color.cyan;// new Color(0.5f,0.8f,0.8f);

        var obj = EditorUtility.InstanceIDToObject(instanceID);
        if (obj != null)
        {
            if (obj.name.Contains("- Pool"))
            {
                if (Selection.instanceIDs.Contains(instanceID))
                {
                    fontColor = Color.white;
                    backgroundColor = new Color(0.24f, 0.48f, 0.90f);
                }

                Rect offsetRect = new Rect(selectionRect.position + offset, selectionRect.size);
                EditorGUI.DrawRect(selectionRect, backgroundColor);
                //EditorGUI.DrawTextureAlpha(selectionRect, backgroundColor);
                EditorGUI.LabelField(offsetRect, obj.name, new GUIStyle()
                {
                    normal = new GUIStyleState() { textColor = fontColor },
                    fontStyle = FontStyle.Bold
                }
                );
            }
        }
    }
}