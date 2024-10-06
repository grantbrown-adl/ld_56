using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour {
    #region Getters
    public static CameraShake Instance { get => _instance; private set => _instance = value; }

    #endregion

    #region Singleton
    private static CameraShake _instance;


    private void CreateSingleton() {
        if (_instance != null && _instance != this) Destroy(gameObject);
        else {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion


    private void Awake() {
        CreateSingleton();
    }

    public IEnumerator PerformShake(float duration, float magnitude) {
        Vector3 initialPosition = transform.localPosition;

        float elapsedTime = 0.0f;
        while (elapsedTime < duration) {
            float x = Random.Range(-1.0f, 1.0f) * magnitude;
            float y = Random.Range(-1.0f, 1.0f) * magnitude;

            transform.localPosition = new Vector3(x, y, initialPosition.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = initialPosition;
    }
}
