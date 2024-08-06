using System;
using System.Linq;
using UnityEngine;
using UnityEditor;

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
            EditorApplication.hierarchyWindowItemOnGUI += OnHierachyWindowItemOnGUI;    
        }

        private static Color GetColor(bool isSelected, bool isHovered, bool isWindowFocused)
        {
            if (isSelected)
            {
                return isWindowFocused ? k_selectedColor : k_selectedUnFocusedColor;
            }

            return isHovered ? k_hoveredColor : k_defaultColor;
        }

        private static void OnHierachyWindowItemOnGUI(int instanceid, Rect selectionrect)
        {
            GameObject obj = EditorUtility.InstanceIDToObject(instanceid) as GameObject;
            
            if (obj == null)
                return;
            
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
            
            bool isSelected = Selection.instanceIDs.Contains(instanceid);
            bool isHovering = selectionrect.Contains(Event.current.mousePosition);
            bool isWindowFocused = EditorWindow.focusedWindow == EditorWindow.mouseOverWindow;

            Color color = GetColor(isSelected, isHovering, isWindowFocused);
            Rect backgroundRect = selectionrect;
            backgroundRect.width = 18.5f;
            EditorGUI.DrawRect(backgroundRect, color);
            
            EditorGUI.LabelField(selectionrect, content);
        }
    }   
}
