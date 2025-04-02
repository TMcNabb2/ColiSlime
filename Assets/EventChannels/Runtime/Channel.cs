using System.Collections.Generic;
using UnityEngine;

namespace EventChannels
{
	[CreateAssetMenu(fileName = "NewEventChannel", menuName = "Event Channels/Event Channel")]
	public sealed class Channel : EventChannel
	{
		[SerializeField] private List<EventSignal> _events = new();
		internal override List<EventSignal> EventList { get => _events; set => _events = value; }
	}
}
