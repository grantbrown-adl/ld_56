using UnityEngine;

public enum VolumeOptions { MasterVolume, UIVolume }

public class SoundManager : MonoBehaviour {
    [SerializeField] private AudioSource _masterAudioSource;
    [SerializeField] private AudioSource _uiAudioSource;
    [SerializeField] private float _currentMasterVolume;
    [SerializeField] private float _currentUIVolume;
    [SerializeField] private AudioClip _buttonHoverClip;
    [SerializeField] private AudioClip _buttonSelectClip;
    [SerializeField] private AudioClip _mainMenuMusicClip;
    [SerializeField] private AudioClip _mainGameMusicClip;

    private static SoundManager _instance;

    public int GetEnumLength<T>() {
        return System.Enum.GetValues(typeof(T)).Length;
    }

    public static SoundManager Instance { get => _instance; private set => _instance = value; }
    public AudioSource AudioSource { get => _masterAudioSource; set => _masterAudioSource = value; }
    public float CurrentMasterVolume { get => _currentMasterVolume; set => _currentMasterVolume = value; }
    public float CurrentUIVolume { get => _currentUIVolume; set => _currentUIVolume = value; }
    public AudioClip MainGameMusicClip { get => _mainGameMusicClip; set => _mainGameMusicClip = value; }

    private void Awake() {
        if (_instance != null && _instance != this) Destroy(gameObject);
        else {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start() {
        LoadVolume();
        SetAudioClip(_mainMenuMusicClip);
        _masterAudioSource.Play();
    }

    public void SetAudioClip(AudioClip audioClip) {
        _masterAudioSource.clip = audioClip;
    }

    public int GetVolume(VolumeOptions name) {
        int value = PlayerPrefs.GetInt(name.ToString());
        return value;
    }

    public void PlayAudio() {
        _masterAudioSource?.Play();
    }

    public void SetVolume(VolumeOptions name, int value) {
        float floatValue = (float)value / 20;
        switch (name) {
            case VolumeOptions.MasterVolume:
                _masterAudioSource.volume = _currentMasterVolume = floatValue;
                break;
            case VolumeOptions.UIVolume:
                _uiAudioSource.volume = _currentUIVolume = floatValue;
                break;
        }

        PlayerPrefs.SetInt(name.ToString(), value);
        LoadVolume();
    }

    void LoadVolume() {
        for (int i = 0; i < GetEnumLength<VolumeOptions>(); i++) {
            VolumeOptions currentVolume = (VolumeOptions)i;

            if (!PlayerPrefs.HasKey(currentVolume.ToString())) {
                SetVolume(currentVolume, 10);
            } else {
                float volumeValue = (float)GetVolume(currentVolume) / 20f;
                switch (currentVolume) {
                    case VolumeOptions.MasterVolume:
                        _masterAudioSource.volume = _currentMasterVolume = volumeValue;
                        break;
                    case VolumeOptions.UIVolume:
                        _uiAudioSource.volume = _currentUIVolume = volumeValue / 5;
                        break;
                }
            }
        }
    }

    public void PlayClip(AudioClip[] clips) {
        int index = Random.Range(0, clips.Length);
        AudioClip clip = clips[index];
        _masterAudioSource.clip = clip;
        _masterAudioSource.Play();
    }

    public void PlayButtonHover() {
        _uiAudioSource.clip = _buttonHoverClip;
        _uiAudioSource.PlayOneShot(_buttonHoverClip);
    }

    public void PlayButtonSelect() {
        _uiAudioSource.clip = _buttonSelectClip;
        _uiAudioSource.PlayOneShot(_buttonSelectClip);
    }
}
