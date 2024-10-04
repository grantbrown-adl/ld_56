using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeStart : MonoBehaviour
{
    private void Start()
    {
        VolumeLoad();
    }

    void VolumeLoad()
    {
        if(!PlayerPrefs.HasKey("volume"))
        {
            PlayerPrefs.SetFloat("volume", 0.5f);
            UpdateVolume();
        }
        else
        {
            UpdateVolume();
        }
    }

    public void UpdateVolume()
    {
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
    }
}
