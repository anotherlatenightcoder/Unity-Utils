using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace Route24.Editor
{
    [InitializeOnLoad]
    public static class HierarchyEnhancements
    {
        // Only use dark theme colors
        private static readonly Color k_defaultColor = new Color(0.2196f, 0.2196f, 0.2196f);
        private static readonly Color k_selectedColor = new Color(0.1725f, 0.3647f, 0.5294f);
        private static readonly Color k_selectedUnFocusedColor = new Color(0.3f, 0.3f, 0.3f);
        private static readonly Color k_hoveredColor = new Color(0.2706f, 0.2706f, 0.2706f);
        
        static HierarchyEnhancements()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;    
        }

        private static Color GetColor(bool isSelected, bool isHovered, bool isWindowFocused)
        {
            if (isSelected)
            {
                return isWindowFocused ? k_selectedColor : k_selectedUnFocusedColor;
            }

            return isHovered ? k_hoveredColor : k_defaultColor;
        }

        static void DrawActivationToggle(Rect selectionRect, GameObject gameObject)
        {
            Rect toggleRect = new Rect(selectionRect);
            toggleRect.x -= 27f;
            toggleRect.width = 13f;
            bool active = EditorGUI.Toggle(toggleRect, gameObject.activeSelf);
            if (active != gameObject.activeSelf)
            {
                Undo.RecordObject(gameObject, "Changing active state of game object");
                gameObject.SetActive(active);
                EditorSceneManager.MarkSceneDirty(gameObject.scene);
            }
        }

        private static void OnHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            
            if (obj == null)
                return;

            DrawActivationToggle(selectionRect, obj);
            
            if (PrefabUtility.IsAnyPrefabInstanceRoot(obj))
                return;
            
            Component[] components = obj.GetComponents<Component>();
            
            if (components == null || components.Length == 0)
                return;
            
            Component component = components.Length > 1 ? components[1] : components[0];

            Type type = component.GetType();

            GUIContent content = EditorGUIUtility.ObjectContent(component, type);
            content.text = null;
            content.tooltip = type.Name;

            if (content.image == null)
                return;
            
            bool isSelected = Selection.instanceIDs.Contains(instanceID);
            bool isHovering = selectionRect.Contains(Event.current.mousePosition);
            bool isWindowFocused = EditorWindow.focusedWindow == EditorWindow.mouseOverWindow;

            Color color = GetColor(isSelected, isHovering, isWindowFocused);
            Rect backgroundRect = selectionRect;
            backgroundRect.width = 18.5f;
            EditorGUI.DrawRect(backgroundRect, color);
            
            EditorGUI.LabelField(selectionRect, content);
        }
    }   
}
