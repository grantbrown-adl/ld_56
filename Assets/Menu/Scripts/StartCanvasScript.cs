using UnityEngine;

public class StartCanvasScript : MonoBehaviour {
    private void Start() {
        VolumeLoad();
        //GameManager.IsPaused = true;        
    }

    public void Unpause() {
        //GameManager.IsPaused = false;
    }

    void VolumeLoad() {
        if (!PlayerPrefs.HasKey("volume")) {
            PlayerPrefs.SetFloat("volume", 0.5f);
            UpdateVolume();
        } else {
            UpdateVolume();
        }
    }

    public void UpdateVolume() {
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
    }
}
