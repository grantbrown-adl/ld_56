using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderScript : MonoBehaviour {
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private VolumeOptions _volumeOption;

    private void Start() {
        _volumeSlider.value = SoundManager.Instance.GetVolume(_volumeOption);
    }

    public void UpdateVolume() {
        SoundManager.Instance.SetVolume(_volumeOption, (int)_volumeSlider.value);
    }
}
