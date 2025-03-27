using System;
using UnityEngine;

namespace EventChannels
{
	[Serializable]
	public class EventReceiver : IReceiver, IBroadcaster, IDisposable
	{
		[SerializeField] private EventChannel _channel;
		[SerializeField] private bool _enabled = true;
		[SerializeField] private int _eventId;

		private bool _disposedValue;

		public EventChannel Channel => _channel;
		public int EventId => _eventId;
		public bool Enabled { get => _enabled; set => _enabled = value; }

		public bool SignalEnabled {
			get {
				if (_disposedValue) return false;
				if (Channel == null) return false;
				return Channel.EventEnabled(EventId);
			}
			set {
				if (_disposedValue) return;
				if (Channel == null) return;

				if (value)
				{
					Channel.EnableEvent(EventId);
				}
				else
				{
					Channel.DisableEvent(EventId);
				}
			}
		}

		private event Receiver _receiver;
		public event Receiver Receiver {
			add {
				if (_disposedValue) return;
				if (Channel == null) return;
				_receiver += value;

				if (!Channel.HasListener(this))
					Channel.AddListener(this);
			}
			remove {
				if (_disposedValue) return;
				if (Channel == null) return;
				_receiver -= value;
			}
		}

		public void Receive(object sender)
		{
			if (_disposedValue) return;
			if (Enabled)
				_receiver?.Invoke(sender);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing && Channel)
				{
					Channel.RemoveListener(this);
				}
				_receiver = null;

				_disposedValue = true;
			}
		}

		~EventReceiver()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: false);
		}

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}

	[Serializable]
	public class EventReceiver<T> : IReceiver<T>, IBroadcaster<T>, IDisposable
	{
		[SerializeField] private EventChannel _channel;
		[SerializeField] private bool _enabled = true;
		[SerializeField] private int _eventId;

		private bool _disposedValue;

		public EventChannel Channel => _channel;
		public int EventId => _eventId;
		public bool Enabled { get => _enabled; set => _enabled = value; }

		public bool SignalEnabled {
			get {
				if (_disposedValue) return false;
				if (Channel == null) return false;
				return Channel.EventEnabled(EventId);
			}
			set {
				if (_disposedValue) return;
				if (Channel == null) return;

				if (value)
				{
					Channel.EnableEvent(EventId);
				}
				else
				{
					Channel.DisableEvent(EventId);
				}
			}
		}

		private event Receiver<T> _receiver;
		public event Receiver<T> Receiver {
			add {
				if (_disposedValue) return;
				if (Channel == null) return;
				_receiver += value;

				if (!Channel.HasListener(this))
					Channel.AddListener(this);
			}
			remove {
				if (_disposedValue) return;
				if (Channel == null) return;
				_receiver -= value;
			}
		}

		public void Receive(object sender, T data)
		{
			if (_disposedValue) return;
			if (Enabled)
				_receiver?.Invoke(sender, data);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				if (disposing && Channel && Channel.HasListener(this))
				{
					Channel.RemoveListener(this);
				}
				_receiver = null;

				_disposedValue = true;
			}
		}

		~EventReceiver()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: false);
		}

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
