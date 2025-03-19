using System;
using UnityEngine;
using UnityEngine.UIElements;
[RequireComponent(typeof(UIDocument))]
public class UIManager : MonoBehaviour
{

    UIDocument m_mainMenuDocument;

    //UIView m_currentView;
    //UIView m_previousView;

    ////list of all the Ui Views
    //ListView<UIView> m_allViews = new List<UIView>();

    //// Modular Screens
    //UIView m_homeView; // Main Screen
    //UIView m_charView; // Character Screen

    ////Overlay Screens
    //UIView m_settingsView; // Settings Screen

    ////Toolbars
    //UIView m_optionsBar;
    //UIView m_menuBar;

    // String Ids for the views
    const string k_homeView = "HomeScreen";
    const string k_charView = "CharacterScreen";
    const string k_settingsView = "SettingsScreen";
    const string k_optionsBar = "OptionsBar";
    const string k_menuBar = "MenuBar";
    private void OnEnable()
    {
        m_mainMenuDocument = GetComponent<UIDocument>();

        //SetUpViews();
        //SubscribeToEvents();

        //// start with the home screen
        //ShowModalView();
    }

    private void ShowModalView()
    {
        throw new NotImplementedException();
    }

    private void SubscribeToEvents()
    {
       // MainMenuUiEvents.Home
    }

    private void SetUpViews()
    {
        throw new NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
