using Unity.Properties;
using UnityEngine;

public class GameData : MonoBehaviour
{
    [SerializeField] uint m_Gold = 500;

    [SerializeField] bool m_IsToggled;

    [SerializeField] float m_MusicVolume;
    [SerializeField] float m_SfxVolume;

    [CreateProperty]
    public uint Gold
    {
        get => m_Gold;
        set => m_Gold = value;
    }

    [CreateProperty]
    public bool IsToggled
    {
        get => m_IsToggled;
        set => m_IsToggled = value;
    }
    [CreateProperty]
    public float MusicVolume
    {
        get => m_MusicVolume;
        set => m_MusicVolume = value;
    }
    [CreateProperty]
    public float SfxVolume
    {
        get => m_SfxVolume;
        set => m_SfxVolume = value;
    }

    public GameData()
    {
        m_Gold = 500;
        m_IsToggled = false;
        m_MusicVolume = 80f;
        m_SfxVolume = 80f;
    }
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public void LoadJson(string jsonFilepath)
    {
        JsonUtility.FromJsonOverwrite(jsonFilepath, this);
    }
}
