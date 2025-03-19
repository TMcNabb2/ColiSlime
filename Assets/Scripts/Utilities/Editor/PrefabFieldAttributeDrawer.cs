using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(PrefabAttribute))]
public class PrefabFieldAttributeDrawer : PropertyDrawer
{
	public override VisualElement CreatePropertyGUI(SerializedProperty property)
	{
		if (property.propertyType != SerializedPropertyType.ObjectReference)
			return new Label($"{nameof(PrefabAttribute)} must be used on a field of type GameObject.");

		var field = new ObjectField(property.displayName) {
			allowSceneObjects = false,
			objectType = typeof(GameObject),
			bindingPath = property.propertyPath,
		};

		field.AddToClassList(ObjectField.alignedFieldUssClassName);
		field.Bind(property.serializedObject);

		return field;
	}
}
