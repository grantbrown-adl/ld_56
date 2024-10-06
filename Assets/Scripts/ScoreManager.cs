using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour {
    #region Getters
    public static ScoreManager Instance { get => _instance; private set => _instance = value; }
    public float CurrentScore { get => currentScore; set => UpdateScore(value); }
    public int Kills { get => kills; set => IncrementKills(); }
    public int Shots { get => shots; set => IncrementShots(); }
    public int Hits { get => hits; set => IncrementHits(); }

    public int TimeElapsed { set => survivalText.text = $"{value:f0}"; }

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
    [SerializeField] int kills;
    [SerializeField] int shots;
    [SerializeField] int hits;
    [SerializeField] float accuracy;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] TextMeshProUGUI killText;
    [SerializeField] TextMeshProUGUI shotText;
    [SerializeField] TextMeshProUGUI hitText;
    [SerializeField] TextMeshProUGUI accuracyText;
    [SerializeField] TextMeshProUGUI survivalText;

    private void Awake() {
        CreateSingleton();
        UpdateScore(0);
    }

    void UpdateScore(float score) {
        currentScore += score;
        scoreText.text = $"{currentScore:F0}";
        finalScoreText.text = $"{currentScore:F0}";
    }

    void IncrementHits() {
        hits++;
        hitText.text = $"{hits:F0}";
        UpdateAccuracy();
    }

    void IncrementShots() {
        shots++;
        shotText.text = $"{shots:F0}";
        UpdateAccuracy();
    }

    void IncrementKills() {
        kills++;
        killText.text = $"{kills:F0}";
    }

    void UpdateAccuracy() {
        accuracy = ((float)hits / (shots != 0 ? shots : 1)) * 100;
        accuracyText.text = $"{accuracy:F2}";
    }
}
