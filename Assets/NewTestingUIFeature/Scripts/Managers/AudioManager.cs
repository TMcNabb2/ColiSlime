using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // AudioMixerGroup names
    public static string MusicGroup = "Music";
    public static string SfxGroup = "SFX";

    // parameter suffix
    const string k_Parameter = "Volume";
    private static float s_LastSFXPlayTime = -1f;
    private static float sfxCooldown = 0.1f; // Global cooldown for playing sound effects

    [SerializeField] AudioMixer m_MainAudioMixer;

    // basic range of UI sound clips
    [Header("UI Sounds")]
    [Tooltip("General button click.")]
    [SerializeField] AudioClip m_DefaultButtonSound;
    [Tooltip("General button click.")]
    [SerializeField] AudioClip m_AltButtonSound;
    [Tooltip("General shop purchase clip.")]
    [SerializeField] AudioClip m_QuitSound;
    [Tooltip("General error sound.")]
    [SerializeField] AudioClip m_DefaultWarningSound;

    [Header("Game Sounds")]
    [Tooltip("Level up or level win sound.")]
    [SerializeField] AudioClip m_VictorySound;
    [Tooltip("Level defeat sound.")]
    [SerializeField] AudioClip m_DefeatSound;
    [Tooltip("Attack defeat sound.")]
    [SerializeField] AudioClip m_FireSound;
    [Tooltip("Attack defeat sound.")]
    [SerializeField] AudioClip m_IceSound;
    [Tooltip("Attack defeat sound.")]
    [SerializeField] AudioClip m_LightningSound;
    [Tooltip("Attack defeat sound.")]
    [SerializeField] AudioClip m_ExplosionSound;
    [Tooltip("Attack defeat sound.")]
    [SerializeField] AudioClip m_HealSound;
    [Tooltip("Attack defeat sound.")]
    [SerializeField] AudioClip m_PoisonSound;
    [Tooltip("Attack defeat sound.")]
    [SerializeField] AudioClip m_SlowSound;
    [Tooltip("Attack defeat sound.")]
    [SerializeField] AudioClip m_StunSound;
    [Tooltip("Attack defeat sound.")]
    [SerializeField] AudioClip m_SummonSound;
    [Tooltip("Attack defeat sound.")]
    [SerializeField] AudioClip m_TeleportSound;
    [Tooltip("Attack defeat sound.")]
    [SerializeField] AudioClip m_WindSound;
    [Tooltip("Attack defeat sound.")]
    [SerializeField] AudioClip m_WaterSound;
    [Tooltip("Attack defeat sound.")]
    [SerializeField] AudioClip m_EarthSound;
    [Tooltip("Attack defeat sound.")]
    [SerializeField] AudioClip m_DarkSound;

    void OnEnable()
    {
        SettingsEvents.SettingsUpdated += OnSettingsUpdated;
        GameplayEvents.SettingsUpdated += OnSettingsUpdated;
    }
    private void OnDisable()
    {
        SettingsEvents.SettingsUpdated -= OnSettingsUpdated;
        GameplayEvents.SettingsUpdated -= OnSettingsUpdated;
    }
    
    //Play one-shot audio

    public static void PlayOneSFX(AudioClip clip, Vector3 sfxPosition)
    {
        if(clip == null) return;
        if (Time.time - s_LastSFXPlayTime < sfxCooldown) return;
        s_LastSFXPlayTime = Time.time;

        GameObject sfx = new GameObject(clip.name);
        sfx.transform.position = sfxPosition;

        AudioSource source = sfx.AddComponent<AudioSource>();
        source.clip = clip;
        source.Play();

        source.outputAudioMixerGroup = GetAudioMixerGroup(SfxGroup);
        Destroy(sfx, clip.length);
    }

    public static AudioMixerGroup GetAudioMixerGroup(string groupName)
    {
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();

        if(audioManager == null) return null;
        if(audioManager.m_MainAudioMixer == null) return null;

        AudioMixerGroup[] groups = audioManager.m_MainAudioMixer.FindMatchingGroups(groupName);
        foreach (AudioMixerGroup matched in groups)
        {
            if (matched.ToString() == groupName)
                return matched;            
        }
        return null;
    }

    public static float GetDecibelValue(float linearValue)
    {
        float conversionFactor = 20f;
        float decibelValue = (linearValue !=0) ? conversionFactor * Mathf.Log10(linearValue) : -144f;
        return decibelValue;
    }
    public static float GetLinearValue(float decibelValue)
    {
        float conversionFactor = 20f;
        return Mathf.Pow(10f, decibelValue / conversionFactor);
    }

    public static void SetVolume(string groupName, float linearValue)
    {
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager == null) return;

        float decibelValue = GetDecibelValue(linearValue);

        if(audioManager.m_MainAudioMixer != null)
        {
            audioManager.m_MainAudioMixer.SetFloat(groupName, decibelValue);
        }
    }

    public static float GetVolume(string groupName)
    {
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager == null) return 0f;

        float decibelValue = 0f;

        if (audioManager.m_MainAudioMixer != null)
        {
            audioManager.m_MainAudioMixer.GetFloat(groupName, out decibelValue);
        }
        return GetLinearValue(decibelValue);
    }
    public static void PlayDefaultButtonSound()
    {
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager == null)
            return;

        PlayOneSFX(audioManager.m_DefaultButtonSound, Vector3.zero);
    }
    public static void PlayAltButtonSound()
    {
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager == null)
            return;

        PlayOneSFX(audioManager.m_AltButtonSound, Vector3.zero);
    }
    public static void PlayQuitSound()
    {
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager == null)
            return;
        PlayOneSFX(audioManager.m_QuitSound, Vector3.zero);
    }
    public static void PlayDefaultWarningSound()
    {
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager == null)
            return;
        PlayOneSFX(audioManager.m_DefaultWarningSound, Vector3.zero);
    }
    public static void PlayVictorySound()
    {
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager == null)
            return;
        PlayOneSFX(audioManager.m_VictorySound, Vector3.zero);
    }
    public static void PlayDefeatSound()
    {
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager == null)
            return;
        PlayOneSFX(audioManager.m_DefeatSound, Vector3.zero);
    }
    public static void PlayFireSound()
    {
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager == null)
            return;
        PlayOneSFX(audioManager.m_FireSound, Vector3.zero);
    }
    public static void PlayIceSound()
    {
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager == null)
            return;
        PlayOneSFX(audioManager.m_IceSound, Vector3.zero);
    }
    public static void PlayLightningSound()
    {
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager == null)
            return;
        PlayOneSFX(audioManager.m_LightningSound, Vector3.zero);
    }
    public static void PlayExplosionSound()
    {
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager == null)
            return;
        PlayOneSFX(audioManager.m_ExplosionSound, Vector3.zero);
    }
    public static void PlayHealSound()
    {
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager == null)
            return;
        PlayOneSFX(audioManager.m_HealSound, Vector3.zero);
    }
    public static void PlayPoisonSound()
    {
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager == null)
            return;
        PlayOneSFX(audioManager.m_PoisonSound, Vector3.zero);
    }
    public static void PlaySlowSound()
    {
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager == null)
            return;
        PlayOneSFX(audioManager.m_SlowSound, Vector3.zero);
    }
    public static void PlayStunSound()
    {
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager == null)
            return;
        PlayOneSFX(audioManager.m_StunSound, Vector3.zero);
    }
    public static void PlaySummonSound()
    {
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager == null)
            return;
        PlayOneSFX(audioManager.m_SummonSound, Vector3.zero);
    }
    public static void PlayTeleportSound()
    {
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager == null)
            return;
        PlayOneSFX(audioManager.m_TeleportSound, Vector3.zero);
    }
    public static void PlayWindSound()
    {
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager == null)
            return;
        PlayOneSFX(audioManager.m_WindSound, Vector3.zero);
    }
    public static void PlayWaterSound()
    {
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager == null)
            return;
        PlayOneSFX(audioManager.m_WaterSound, Vector3.zero);
    }
    public static void PlayEarthSound()
    {
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager == null)
            return;
        PlayOneSFX(audioManager.m_EarthSound, Vector3.zero);
    }
    public static void PlayDarkSound()
    {
        AudioManager audioManager = FindFirstObjectByType<AudioManager>();
        if (audioManager == null)
            return;
        PlayOneSFX(audioManager.m_DarkSound, Vector3.zero);
    }
    void OnSettingsUpdated(GameData gameData)
    {
        // use the gameData to set the music and sfx volume
        SetVolume(MusicGroup + k_Parameter, gameData.MusicVolume / 100f);
        SetVolume(SfxGroup + k_Parameter, gameData.SfxVolume / 100f);
    }
}
