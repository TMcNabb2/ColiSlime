using System;
using UnityEngine;
using UnityEngine.UIElements;
public abstract class MenuScreen : MonoBehaviour
{
    [Tooltip("String Id from the UXML for the menu panel or screen.")]
    [SerializeField] protected string m_screenName;

    [Header("UI Management")]
    [Tooltip("Set the Ui Document here explicitly (or get from current object)")]
    [SerializeField] protected UIDocument m_document;

    //visual elements
    protected VisualElement m_root;
    protected VisualElement m_screen;

    //UXML element Name(with default to class name)
    protected virtual void OnValidate()
    {
        if (string.IsNullOrEmpty(m_screenName))
        {
            m_screenName = GetType().Name;
        }
    }
    protected virtual void Awake()
    {
        if (m_screen == null)
        {
            m_document = GetComponent<UIDocument>();
        }

        if(m_document == null)
        {
            Debug.LogError("MenuScreen " + m_screenName + ": missing UIDocument.");
            return;
        }
        else
        {
            SetVisualElements();
            RegisterButtonCallbacks();
        }
    }

    protected virtual void RegisterButtonCallbacks()
    {
        throw new NotImplementedException();
    }

    //Usues strings IDs to query the root and find matching visual elements in the UXML
    protected virtual void SetVisualElements()
    {
        if(m_document != null)
        {
            m_root = m_document.rootVisualElement;

        }
        m_screen = GetVisualElement(m_screenName);
    }
    public bool ISVisible()
    {
        if (m_screen == null)
        {
            return false;
        }
        return (m_screen.style.display == DisplayStyle.Flex);
    }
    //Toggle a Ui on and off by using display style
    public static void ShowVisualElement(VisualElement visualElement, bool state)
    {
        if (visualElement == null)
        {
            return;
        }
        visualElement.style.display = state ? DisplayStyle.Flex : DisplayStyle.None;
    }
    //returns element by name from query
    private VisualElement GetVisualElement(string elementName)
    {
        if(string.IsNullOrEmpty(elementName) || m_root == null)
        {
            return null;
        }
        return m_root.Q<VisualElement>(elementName);
    }

    //Show the screen
    public virtual void ShowScreen()
    {
        ShowVisualElement(m_screen, true);
    }
    //hide the screen
    public virtual void HideScreen()
    {
        if (ISVisible())
        {
            ShowVisualElement(m_screen, false);
        }
    }    
}
