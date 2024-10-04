using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroTransitionScript : MonoBehaviour
{
    public VideoPlayer _videoPlayer;

    [SerializeField] private float introTime;
    private void Start()
    {
        if(_videoPlayer)
        {
            _videoPlayer.url = Application.streamingAssetsPath + "/" + "Splash_Animated.mp4";
            _videoPlayer.Prepare();
            _videoPlayer.Play();
        }

        StartCoroutine(LoadAfterWait());
    }


    IEnumerator LoadAfterWait()
    {
        yield return new WaitForSeconds(introTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
