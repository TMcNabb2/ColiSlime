using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace EventChannels.Editor
{
	[CustomPropertyDrawer(typeof(EventSignal))]
	public class EventSignalDrawer : PropertyDrawer
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var nameField = new TextField() {
				name = "event-name",
				label = "Name",
				bindingPath = property.FindPropertyRelative(nameof(EventSignal.name)).propertyPath,
				style = {
					flexGrow = 1,
				},
			};
			var id = property.FindPropertyRelative(nameof(EventSignal.id));
			nameField.RegisterValueChangedCallback(evt => { id.intValue = evt.newValue.GetHashCode(); property.serializedObject.ApplyModifiedProperties(); });

			nameField.Bind(property.serializedObject);

			return nameField;
		}
	}
}
