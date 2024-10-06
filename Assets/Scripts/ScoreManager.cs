using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour {
    #region Getters
    public static ScoreManager Instance { get => _instance; private set => _instance = value; }
    public float CurrentScore { get => currentScore; set => UpdateScore(value); }

    #endregion

    #region Singleton
    private static ScoreManager _instance;


    private void CreateSingleton() {
        if (_instance != null && _instance != this) Destroy(gameObject);
        else {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    [SerializeField] float currentScore;
    [SerializeField] TextMeshProUGUI scoreText;

    private void Awake() {
        CreateSingleton();
        UpdateScore(0);
    }


    private void Update() {
    }

    void UpdateScore(float score) {
        currentScore += score;
        scoreText.text = $"{currentScore:F0}";
    }
}
