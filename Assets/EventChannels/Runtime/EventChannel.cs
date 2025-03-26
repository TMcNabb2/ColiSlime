using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EventChannels
{
	public abstract class EventChannel : ScriptableObject
	{
		public Dictionary<int, EventSignal> EventsDict { get; } = new();

		internal abstract List<EventSignal> EventList { get; set; }

		protected readonly List<IBaseReceiver> _listeners = new();

		protected virtual void OnEnable()
		{
			EventList = EventList.Distinct().ToList();
			EventsDict.Clear();
			foreach (var evt in EventList)
			{
				if (!EventsDict.TryAdd(evt.id, evt))
				{
					Debug.LogWarning(Messages.DuplicateEvent(this, "add event", evt.id));
				}
			}
		}

		protected virtual void OnDisable()
		{
			EventsDict.Clear();

			for (int i = _listeners.Count - 1; i >= 0; i--)
			{
				IBaseReceiver listener = _listeners[i];
				if (HasListener(listener))
					RemoveListener(listener);
				listener.Dispose();
			}
		}

		public IReadOnlyList<EventSignal> GetEvents() => EventList.AsReadOnly();

		public bool HasListener(IBaseReceiver receiver) => _listeners.Contains(receiver);

		public void AddListener(IBaseReceiver receiver)
		{
			if (EventsDict.ContainsKey(receiver.EventId))
			{
				EventsDict[receiver.EventId].OnBroadcast += receiver.Receive;

				if (!HasListener(receiver))
					_listeners.Add(receiver);
			}
			else
			{
				Debug.LogWarning(Messages.MissingEvent(this, "add listener", receiver.EventId));
			}
		}

		public void RemoveListener(IBaseReceiver receiver)
		{
			if (EventsDict.ContainsKey(receiver.EventId))
			{
				EventsDict[receiver.EventId].OnBroadcast -= receiver.Receive;

				if (HasListener(receiver))
					_listeners.Remove(receiver);
			}
			else
			{
				Debug.LogWarning(Messages.MissingEvent(this, "remove listener", receiver.EventId));
			}
		}

		public void EnableEvent(int eventId)
		{
			if (EventsDict.TryGetValue(eventId, out var signal))
			{
				signal.disabled = false;
			}
			else
			{
				Debug.LogWarning(Messages.MissingEvent(this, "enable event", eventId));
			}
		}
		public void DisableEvent(int eventId)
		{
			if (EventsDict.TryGetValue(eventId, out var signal))
			{
				signal.disabled = true;
			}
			else
			{
				Debug.LogWarning(Messages.MissingEvent(this, "disable event", eventId));
			}
		}
		public bool EventEnabled(int eventId) => EventsDict.ContainsKey(eventId) && !this.EventsDict[eventId].disabled;

		public void Broadcast(int eventId, object sender, object data)
		{
			if (EventsDict.TryGetValue(eventId, out var signal))
			{
				if (!signal.disabled)
					signal.Invoke(sender, data);
			}
			else
			{
				Debug.LogWarning(Messages.MissingEvent(this, "invoke event", eventId));
			}
		}

		internal static class Messages
		{
			internal static string Prefix(EventChannel channel, string action)
				=> $"Failed to {action} on channel {channel.name}: ";
			internal static string MissingEvent(EventChannel channel, string action, int id)
				=> Prefix(channel, action) + $"No event with eventId '{id}'.";
			internal static string DuplicateEvent(EventChannel channel, string action, int id)
				=> Prefix(channel, action) + $"An event with eventId '{id}' already exists.";
		}
	}
}
