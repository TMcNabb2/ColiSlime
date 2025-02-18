using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public interface ITimer : IDisposable
{
    /// <summary>
    /// Is <see langword="true"/> while the timer is running.
    /// </summary>
    bool IsRunning { get; }

    /// <summary>
    /// The amount of time passed since the timer started.
    /// </summary>
    float TimeElapsed { get; }

    /// <summary>
    /// Invoked when the timer starts.
    /// </summary>
    event TimerEvent OnStart;
    /// <summary>
    /// Invoked when the timer ends.
    /// </summary>
    event TimerEvent OnStop;

    public delegate void TimerEvent(ITimer timer);

    /// <summary>
    /// Starts the timer.
    /// </summary>
    /// <param name="sendCallback">If <see langword="true"/>, will invoke the <see cref="OnStart"/> event.</param>
    void Start(bool sendCallback = true);
    /// <summary>
    /// Stops the timer.
    /// </summary>
    /// <param name="sendCallback">If <see langword="true"/>, will invoke the <see cref="OnStop"/> event.</param>
    void Stop(bool sendCallback = true);
    /// <summary>
    /// Stops the timer before starting it again.
    /// </summary>
    /// <param name="sendCallback">If <see langword="true"/>, will invoke the <see cref="OnStop"/> and <see cref="OnStart"/> events.</param>
    public void Restart(bool sendCallback = true)
    {
        Stop(sendCallback);
        Start(sendCallback);
    }
}
