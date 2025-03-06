using UnityEngine;
using System;
using UnityEngine.UIElements;

public class GameplayEvents
{
    // Triggered after the battle is won
    public static Action WinScreenShown;

    // Triggered after the battle is lost
    public static Action LoseScreenShown;

    public static Action<GameData> SettingsUpdated;
    //load the music and sfx volume
    public static Action<GameData> SettingsLoaded;
    // notify lister to pause the game
    public static Action<float> GamePaused;
    //resume the game from pause
    public static Action GameResumed;
    // quit the game from pause
    public static Action GameQuit;
    // restart from pause
    public static Action GameRestarted;
    // Adjust the music volume during gameplay
    public static Action<float> MusicVolumeChanged;
    // Adjust the SFx volume during gameplay
    public static Action<float> SfxVolumeChanged;

}
