using System;
using UnityEngine;

namespace EventChannels
{
	public delegate void SignalEvent(object sender, object data);

	[Serializable]
	public class EventSignal
	{
		public bool disabled;
		public int id;
		public string name;

		public event SignalEvent OnBroadcast;

		public EventSignal(string name = "EventSignal")
		{
			OnBroadcast = delegate { };
			this.name = name;
			this.id = name.GetHashCode();
		}
		
		public void Invoke(object sender, object data) => OnBroadcast?.Invoke(sender, data);

		public override bool Equals(object obj) => obj is EventSignal evt && evt.id == this.id;
		public override int GetHashCode() => id;

		public static bool operator ==(EventSignal left, EventSignal right) => left.Equals(right);
		public static bool operator !=(EventSignal left, EventSignal right) => !(left == right);
	}
}
