using EventChannels;
using UnityEngine;

public class TestScript : MonoBehaviour
{
	public enum Behaviour
	{
		Sender, Receiver
	}
	public Behaviour behaviour;
	public EventReceiver<int> intReceiver;
	public int data;
	public EventReceiver voidReceiver;

	public void Awake()
	{
		if (behaviour is Behaviour.Receiver)
		{
			intReceiver.Receiver += this.IntReceiver_Receiver;
			voidReceiver.Receiver += this.VoidReceiver_Receiver;
		}
	}

	private void IntReceiver_Receiver(object sender, int data)
	{
		Debug.Log($"Received message from {sender}! Value is {data}");
	}

	private void VoidReceiver_Receiver(object sender)
	{
		Debug.Log($"Received message from {sender}!");
	}

	void Start()
	{
		if (behaviour is Behaviour.Sender)
		{
			intReceiver.Broadcast(this, data);
			voidReceiver.Broadcast(this);
		}
	}
}
