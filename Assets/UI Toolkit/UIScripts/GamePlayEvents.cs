using System;
using UnityEngine; 
using UnityEngine.UIElements;
public static class GamePlayEvents
{
    //Triggered after the battle is won
    public static Action WinScreenShown;
    //Triggered after the battle is lost
    public static Action LoseScreenShown;
    //Triggered when the game is paused
    public static Action GamePaused;
    //Triggered when the game is resumed
    public static Action GameResumed;
    //Triggered when the game is paused or resumed
    public static Action TogglePause;
    //Triggered when the game is quit
    public static Action GameQuit;
    //Triggered when the game is restarted
    public static Action GameRestart;

    public static Action AbilityOneUsed;
    public static Action AbilityTwoUsed;
    public static Action<float> XPUp;

    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
        WinScreenShown = delegate { };
        LoseScreenShown = delegate { };
        GamePaused = delegate { };
        GameResumed = delegate { };
        TogglePause = delegate { };
        GameQuit = delegate { };
        GameRestart = delegate { };
        AbilityOneUsed = delegate { };
        AbilityTwoUsed = delegate { };
        XPUp = delegate { };
    }
}
