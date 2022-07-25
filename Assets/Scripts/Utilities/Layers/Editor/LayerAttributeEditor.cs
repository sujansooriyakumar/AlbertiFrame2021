using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(LayerAttribute))]
[CanEditMultipleObjects]
class LayerAttributeEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.BeginChangeCheck(); //This way change will only take effect if this code block changes
        EditorGUI.showMixedValue = property.hasMultipleDifferentValues;
        if (property.hasMultipleDifferentValues)
        {

        }
        int temp = EditorGUI.LayerField(position, label, property.intValue);
        if (EditorGUI.EndChangeCheck())
        {
            property.intValue = temp;
        }
        EditorGUI.EndProperty();
    }
}
