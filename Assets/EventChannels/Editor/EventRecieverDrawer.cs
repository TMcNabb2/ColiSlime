using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace EventChannels.Editor
{
	[CustomPropertyDrawer(typeof(EventReceiver))]
	[CustomPropertyDrawer(typeof(EventReceiver<>))]
	public class EventReceiverDrawer : PropertyDrawer
	{
		public override VisualElement CreatePropertyGUI(SerializedProperty property)
		{
			var foldout = new Foldout() {
				text = property.displayName,
				value = property.isExpanded,
			};
			foldout.RegisterValueChangedCallback(evt => property.isExpanded = evt.newValue);


			var channelField = new ObjectField() {
				label = "Channel",
				bindingPath = property.FindPropertyRelative("_channel").propertyPath,
				allowSceneObjects = false,
				objectType = typeof(EventChannel),
			};
			channelField.AddToClassList(ObjectField.alignedFieldUssClassName);
			foldout.Add(channelField);
			var id = property.FindPropertyRelative("_eventId");

			var selectorButton = new Button() {
				style = {
					flexGrow = 1,
				},
			};
			selectorButton.clicked += () => ShowSelectorDropdown(id, selectorButton, channelField.value as EventChannel);
			foldout.Add(selectorButton);

			var enabledToggle = new Toggle() {
				label = "Enabled",
				bindingPath = property.FindPropertyRelative("_enabled").propertyPath,
			};
			enabledToggle.AddToClassList(ObjectField.alignedFieldUssClassName);
			foldout.Add(enabledToggle);

			channelField.RegisterValueChangedCallback(evt => UpdateSelectorButton(evt.newValue as EventChannel, id, selectorButton));

			foldout.Bind(property.serializedObject);

			return foldout;
		}

		private void UpdateSelectorButton(EventChannel channel, SerializedProperty property, Button selectorButton)
		{
			const string invalidMessage = "Event channel or ID is invalid.";
			void disable()
			{
				ClearEvent(property, selectorButton);
				selectorButton.enabledSelf = false;
			}

			if (channel != null)
			{
				var evts = channel.GetEvents();
				var signal = evts.FirstOrDefault(e => e.id == property.intValue);

				if (signal is not null)
				{
					SetEvent(property, selectorButton, signal);
					selectorButton.enabledSelf = true;
				}
				else if (property.intValue != 0)
				{
					Debug.LogWarning(invalidMessage);
					EditorApplication.Beep();
					ClearEvent(property, selectorButton);
				}
				else
				{
					ClearEvent(property, selectorButton);
				}
			}
			else
			{
				disable();
			}
		}

		private void ClearEvent(SerializedProperty property, Button selectorButton)
		{
			property.intValue = 0;
			selectorButton.text = "Event: None";
			property.serializedObject.ApplyModifiedProperties();
		}

		private void SetEvent(SerializedProperty property, Button selectorButton, EventSignal signal)
		{
			property.intValue = signal.id;
			selectorButton.text = "Event: " + signal.name;
			property.serializedObject.ApplyModifiedProperties();
		}

		private void ShowSelectorDropdown(SerializedProperty property, Button selectorButton, EventChannel channel)
		{
			var evts = channel.GetEvents();

			GenericMenu menu = new();

			menu.AddItem(
				content: new GUIContent("None"),
				on: property.intValue == 0,
				func: () => ClearEvent(property, selectorButton));

			for (int i = 0; i < evts.Count; i++)
			{
				int index = i;
				menu.AddItem(
					content: new GUIContent(evts[i].name),
					on: property.intValue == evts[i].id,
					func: () => SetEvent(property, selectorButton, evts[index]));

			}

			menu.ShowAsContext();
		}
	}
}
