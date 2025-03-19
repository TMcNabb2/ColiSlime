using System;

/// <summary>
/// Static class that contains all the events for the Main Menu UI
/// Events are called from the UI elements in the Main Menu, not the strict c# sense
/// </summary>
public static class MainMenuUIEvents
{
    //show the homescreen
    public static Action HomeScreenShown;
    //show the character screen to select character
    public static Action CharacterScreenShown;
    //show the settings screen overlay
    public static Action SettingsScreenShown;
    //Hide the Settings screen overlay
    public static Action SettingsMenuHidden;

    //Show the screen for gameplay
    public static Action GameScreenShown;

    //trigger when showing a new menuscren
    public static Action<MenuScreen> CurrentScreenChanged;

    public static Action<string> CurrentViewChanged;
    
}