using UnityEngine;
using UnityEngine.UIElements;

public class UIElementsXP : MonoBehaviour
{
    [SerializeField, Min(0)] private float _xp;
    [SerializeField, Min(0)] private float _maxXP;
    [SerializeField] private string _xpBarName;
    private ProgressBar _xpBar;

    private void Awake()
    {
        var uiDocument = FindFirstObjectByType<UIDocument>();
        var root = uiDocument.rootVisualElement;
        _xpBar = root.Q<ProgressBar>(_xpBarName);
        if (_xpBar == null)
        {
            Debug.LogError($"Could not find a <color=#00FF00>ProgressBar</color> with name '{_xpBarName}'.");
            return;
        }
        XPManager.Instance.XPValueChanged.AddListener(OnValueChange);
        XPManager.Instance.LevelValueChanged.AddListener(OnMaxChanged);
        _xpBar.lowValue = 0;
    }
    private void OnDisable()
    {
        if (XPManager.HasInstance)
        {
            XPManager.Instance.XPValueChanged.RemoveListener(OnValueChange);
            XPManager.Instance.LevelValueChanged.RemoveListener(OnMaxChanged);
        }
    }
    void OnValueChange(float value)
    {
        _xpBar.value = value;
    }
    void OnMaxChanged(float value)
    {
        _xpBar.highValue = value;
    }
}
