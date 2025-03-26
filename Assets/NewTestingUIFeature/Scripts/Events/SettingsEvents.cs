using UnityEngine;
using System;

public class SettingsEvents
{
    public static Action SettingsShown;

    // Sync saved data from controller to Ui
    public static Action<GameData> GameDataLoaded;
    // pass copy of data to controller
    public static Action<GameData> UIGameDataUpdated;
    // Send data to listeners (gamedatamanager, audiomanager etc)
    public static Action<GameData> SettingsUpdated;
}
