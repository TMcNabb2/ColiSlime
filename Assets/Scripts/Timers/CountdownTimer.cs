using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// A timer that counts down to zero over a given duration.
/// </summary>
public class CountdownTimer : ITimer
{
    private float _timeElapsed;

    private CancellationTokenSource _tokenSource;

    public event ITimer.TimerEvent OnStart;
    public event ITimer.TimerEvent OnStop;

    public bool IsRunning { get; private set; }

    /// <summary>
    /// The amount of time in seconds which the timer will run for.
    /// </summary>
    public float Duration { get; private set; }

    /// <summary>
    /// The amount of time elapsed since the timer started.
    /// </summary>
    public float TimeElapsed { get => _timeElapsed; set => _timeElapsed = Mathf.Clamp(value, 0f, Duration); }

    /// <summary>
    /// Constructs an <see cref="CountdownTimer"/> with the given <paramref name="duration"/>.
    /// </summary>
    /// <param name="duration"></param>
    public CountdownTimer(float duration, bool repeat = false)
    {
        Duration = duration;

        if (repeat)
            OnStop += _ => Start();
    }

    public void Dispose() => Stop(sendCallback: false);

    public async void Start(bool sendCallback = true)
    {
        _tokenSource = new CancellationTokenSource();

        try
        {
            IsRunning = true;

            if (sendCallback)
                OnStart?.Invoke(this);

            await Run(_tokenSource.Token);

            Stop();
        }
        catch (OperationCanceledException)
        {
            IsRunning = false;
            Debug.Log($"{Duration} second timer cancelled.");
        }
    }

    private async Task Run(CancellationToken cancellationToken)
    {
        float startTime = Time.time;
        Debug.Assert(IsRunning);

        while (Time.time < startTime + Duration)
        {
            TimeElapsed = Time.time - startTime;
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

    /// <summary>
    /// Resets <see cref="TimeElapsed"/> to zero.
    /// </summary>
    /// <remarks>
    /// See also: <seealso cref="Restart(bool)"/>
    /// </remarks>
    public void Reset()
    {
        TimeElapsed = 0.0f;
    }

    void ITimer.Restart(bool sendCallback)
    {
        Stop(sendCallback);
        Start(sendCallback);
    }
}
