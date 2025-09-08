using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }

    [SerializeField] private AudioSource bgmSource;
    [Range(0f, 1f)][SerializeField] private float defaultBgmVolume = 0.5f;

    private bool isMuted = false;

    private const string VolumeKey = "BGM_VOLUME";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.playOnAwake = false;
        }

        float savedVolume = PlayerPrefs.GetFloat(VolumeKey, defaultBgmVolume);
        SetBgmVolume(savedVolume);
    }

    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;
        if (bgmSource.clip == clip && bgmSource.isPlaying) return;

        bgmSource.clip = clip;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        bgmSource.mute = isMuted;
    }

    public void SetBgmVolume(float volume)
    {
        bgmSource.volume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(VolumeKey, bgmSource.volume);
        PlayerPrefs.Save();
    }

    public void BindBgmSlider(Slider slider)
    {
        if (slider == null) return;

        slider.value = bgmSource.volume;
        slider.onValueChanged.AddListener(SetBgmVolume);
    }

    public void UnbindBgmSlider(Slider slider)
    {
        if (slider == null) return;
        slider.onValueChanged.RemoveListener(SetBgmVolume);
    }
}
