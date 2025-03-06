using UnityEngine;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class SafeAreaBorder : MonoBehaviour
{
    [Tooltip("UI document that contains the UXML hierarchy")]
    [SerializeField] UIDocument m_Document;

    [Tooltip("Color for the border area. Use a transparent color to show the background.")]
    [SerializeField] Color m_BorderColor = Color.black;

    [Tooltip("Name of top-level element container. Leave empty to use rootVisualElement.")]
    [SerializeField] string m_Element;

    [Tooltip("Percentage multiplier for safe area distance")]
    [Range(0, 1f)]
    [SerializeField] float m_Multiplier = 1f;

    [Tooltip("Show debug messages in the console.")]
    [SerializeField] bool m_Debug;

    VisualElement m_Root;
    float m_LeftBorder, m_RightBorder, m_TopBorder, m_BottomBorder;

    public VisualElement RootElement => m_Root;
    public float LeftBorder => m_LeftBorder;
    public float RightBorder => m_RightBorder;
    public float TopBorder => m_TopBorder;
    public float BottomBorder => m_BottomBorder;
    public float Multiplier { get => m_Multiplier; set => m_Multiplier = value; }
    void Awake()
    {
        Initialize();
    }
    public void Initialize()
    {
        if (m_Document == null || m_Document.rootVisualElement == null)
        {
            Debug.LogError("UIDocument or rootVisualElement is null. Delay Start.");
            return;
        }
        if (string.IsNullOrEmpty(m_Element))
        {
            m_Root = m_Document.rootVisualElement;
        }
        else
        {
            m_Root = m_Document.rootVisualElement.Q<VisualElement>(m_Element);
        }
        if (m_Root == null)
        {
            if (m_Debug)
            {
                Debug.LogError("m_Root is null. Element not found or UIDocument is delayed.");
            }
            return;
        }
        m_Root.RegisterCallback<GeometryChangedEvent>(evt => OnGeometryChangedEvent());
        ApplySafeArea();
    }
    void OnGeometryChangedEvent()
    {
        ApplySafeArea();
    }
    void OnValidate()
    {
        // Call ApplySafeArea when m_Multiplier is changed
        ApplySafeArea();
    }
    void ApplySafeArea()
    {
        if (m_Root == null)
        {
            return;
        }
        Rect safeArea = Screen.safeArea;

        //Calculate Borders based on Safe Area Rect
        m_LeftBorder = safeArea.x;
        m_RightBorder = Screen.width - safeArea.xMax;
        m_TopBorder = Screen.height - safeArea.yMax;    
        m_BottomBorder = safeArea.y;

        // Borders for any orientation
        m_Root.style.borderTopWidth = m_TopBorder * m_Multiplier;
        m_Root.style.borderBottomWidth = m_BottomBorder * m_Multiplier;
        m_Root.style.borderLeftWidth = m_LeftBorder * m_Multiplier;
        m_Root.style.borderRightWidth = m_RightBorder * m_Multiplier;

        //apply border styles/colors
        m_Root.style.borderTopColor = m_BorderColor;
        m_Root.style.borderBottomColor = m_BorderColor;
        m_Root.style.borderLeftColor = m_BorderColor;
        m_Root.style.borderRightColor = m_BorderColor;

        if (m_Debug)
        {
            Debug.Log($"[SafeAreaBorder] Applied Safe Area | Screen Orientation: {Screen.orientation} | Left: {m_LeftBorder}, Right: {m_RightBorder}, Top: {m_TopBorder}, Bottom: {m_BottomBorder}");
        }    }
}