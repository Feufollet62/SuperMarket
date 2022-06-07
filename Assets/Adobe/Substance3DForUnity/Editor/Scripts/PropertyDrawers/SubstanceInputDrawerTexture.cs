using UnityEditor;
using UnityEngine;

namespace Adobe.Substance.Editor
{
    internal static class SubstanceInputDrawerTexture
    {
        public static bool DrawInput(SerializedProperty valueProperty, SubstanceInputGUIContent content, SubstanceNativeHandler handler, int graphID, int inputID)
        {
            Texture2D newValue;
            bool changed;

            switch (content.Description.WidgetType)
            {
                default:
                    changed = DrawDefault(valueProperty, content, out newValue);
                    break;
            }

            if (changed)
            {
                var pixels = newValue.GetPixels32();
                handler.SetInputTexture2D(pixels, newValue.width, newValue.height, inputID, graphID);
            }

            return changed;
        }

        private static bool DrawDefault(SerializedProperty valueProperty, SubstanceInputGUIContent content, out Texture2D newValue)
        {
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.ObjectField(valueProperty, content);
            var changed = EditorGUI.EndChangeCheck();
            newValue = null;

            if (changed)
            {
                newValue = valueProperty.objectReferenceValue as Texture2D;

                if (newValue != null)
                {
                    if (!newValue.isReadable)
                        TextureUtils.SetReadableFlag(newValue, true);
                }
            }

            return changed;
        }
    }
}