using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Timers
{
	/// <summary>
	/// A timer that counts up until stopped.
	/// </summary>
	public class StopwatchTimer : ITimer
	{
		private CancellationTokenSource _tokenSource;

		public event ITimer.TimerEvent OnStart;
		public event ITimer.TimerEvent OnStop;

		public float TimeElapsed { get; private set; }
		public bool IsRunning { get; private set; }

		public void Dispose()
		{
			Stop(false);
		}

		public async void Start(bool sendCallback = true)
		{
			_tokenSource = new CancellationTokenSource();
			TimeElapsed = 0f;

			try
			{
				if (sendCallback)
					OnStart?.Invoke(this);

				await Run(_tokenSource.Token);

				Stop();
			}
			catch (OperationCanceledException)
			{
				IsRunning = false;
			}
		}

		private async Task Run(CancellationToken cancellationToken)
		{
			float startTime = Time.time;
			while (true)
			{
				TimeElapsed = Time.time - startTime;
				cancellationToken.ThrowIfCancellationRequested();
				await Awaitable.EndOfFrameAsync(cancellationToken);
			}
		}

		public void Stop(bool sendCallback = true)
		{
			_tokenSource?.Cancel();
			_tokenSource?.Dispose();
			_tokenSource = null;

			if (sendCallback && IsRunning)
			{
				IsRunning = false;
				OnStop?.Invoke(this);
			}
			else
			{
				IsRunning = false;
			}
		}

		void ITimer.Restart(bool sendCallback)
		{
			Stop(sendCallback);
			Start(sendCallback);
		}
	}
}