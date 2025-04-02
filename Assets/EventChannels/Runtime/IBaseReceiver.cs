using System;

namespace EventChannels
{
	public delegate void Receiver(object sender);
	public delegate void Receiver<in T>(object sender, T data);

	public interface IBaseReceiver : IDisposable
	{
		int EventId { get; }
		bool Enabled { get; }
		void ReceiveEvent(object sender, object data)
		{
			if (Enabled)
				Receive(sender, data);
		}
		void Receive(object sender, object data);
	}

	public interface IReceiver : IBaseReceiver
	{
		void IBaseReceiver.Receive(object sender, object data) => Receive(sender);

		void Receive(object sender);
	}

	public interface IReceiver<in T> : IBaseReceiver
	{
		void IBaseReceiver.Receive(object sender, object data)
		{
			if (data is T d)
				Receive(sender, d);
		}

		void Receive(object sender, T data);
	}

	public interface IBroadcaster<in TData>
	{
		EventChannel Channel { get; }
		int EventId { get; }
		void Broadcast(object sender, TData data)
		{
			if (Channel != null)
				Channel.Broadcast(EventId, sender, data);
		}
	}

	public interface IBroadcaster : IBroadcaster<object>
	{
		virtual void Broadcast(object sender) => this.Broadcast(sender, null);
	}

	public static class BroadcasterExtensions
	{
		public static void Broadcast<T>(this IBroadcaster<T> broadcaster, object sender, T data)
		{
			broadcaster.Broadcast(sender, data);
		}

		public static void Broadcast(this IBroadcaster broadcaster, object sender)
		{
			broadcaster.Broadcast(sender);
		}
	}
}
