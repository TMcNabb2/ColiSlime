using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using System;

namespace EventChannels.Editor
{
	[CustomEditor(typeof(Channel), editorForChildClasses: true)]
	public class EventChannelEditor : UnityEditor.Editor
	{
		[SerializeField] private StyleSheet _style;

		public override VisualElement CreateInspectorGUI()
		{
			var events = serializedObject.FindProperty("_events");

			VisualElement root = new();
			root.styleSheets.Add(_style);
			root.AddToClassList("body");

			var label = new Label() {
				name = "header",
				text = "Events",
			};
			label.AddToClassList("event-header");
			root.Add(label);

			var container = new VisualElement() {
				name = "container",
			};
			container.AddToClassList("container");

			var eventList = new VisualElement() {
				name = "event-list",
			};
			eventList.AddToClassList("event-list");

			container.Add(eventList);

			var addButton = new Button(() => addItem()) {
				name = "add-button",
				text = "Add Event",
			};
			addButton.AddToClassList("add-button");
			container.Add(addButton);

			UpdateEventList(events, eventList);

			root.Add(container);

			root.Bind(serializedObject);
			return root;

			void addItem()
			{
				AddEventToList(events, eventList);
			}
		}

		private void AddEventToList(SerializedProperty events, VisualElement listContainer)
		{
			events.InsertArrayElementAtIndex(events.arraySize);
			events.serializedObject.ApplyModifiedProperties();
			serializedObject.Update();
			//listContainer.Add(CreateListItem(events, listContainer, events.arraySize - 1));
			UpdateEventList(events, listContainer);
		}

		private void UpdateEventList(SerializedProperty events, VisualElement listContainer)
		{
			serializedObject.Update();
			listContainer.Clear();

			if (events.arraySize > 0)
			{
				for (int i = 0; i < events.arraySize; i++)
				{
					int index = i;
					VisualElement element = this.CreateListItem(events, listContainer, index);
					listContainer.Add(element);
				}
			}
			else
			{
				var emptyLabel = new Label("List is Empty");
				emptyLabel.AddToClassList("event-list-item");
				emptyLabel.AddToClassList("empty-label");
				listContainer.Add(emptyLabel);
			}

			serializedObject.Update();
		}

		private VisualElement CreateListItem(SerializedProperty events, VisualElement listContainer, int index)
		{
			var property = events.GetArrayElementAtIndex(index);

			var element = new VisualElement() {
				name = "list-item-" + index,
			};
			element.AddToClassList("event-list-item");

			var removeButton = new Button(() => RemoveItem(events, property, listContainer)) {
				name = "remove-button",
				text = "–",
			};
			element.Add(removeButton);

			element.Add(CreateSignalField(property));

			element.Bind(serializedObject);
			return element;
		}

		private void RemoveItem(SerializedProperty events, SerializedProperty property, VisualElement listContainer)
		{
			property.DeleteCommand();
			serializedObject.ApplyModifiedProperties();
			UpdateEventList(events, listContainer);
		}

		private static VisualElement CreateSignalField(SerializedProperty property)
		{
			var nameField = new TextField() {
				name = "event-name",
				label = "Name",
				bindingPath = property.FindPropertyRelative(nameof(EventSignal.name)).propertyPath,
				style = {
					marginRight = 4,
				},
			};
			nameField.AddToClassList("flex-grow");
			var id = property.FindPropertyRelative(nameof(EventSignal.id));
			nameField.RegisterValueChangedCallback(evt => { id.intValue = evt.newValue.GetHashCode(); property.serializedObject.ApplyModifiedProperties(); });

			nameField.Bind(property.serializedObject);

			return nameField;
		}
	}
}
