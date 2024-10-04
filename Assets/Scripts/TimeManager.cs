using UnityEngine;

public class TimeManager : MonoBehaviour {
    [SerializeField] private bool isPaused;
    private static TimeManager _instance;
    [SerializeField] GameObject _pausePanel;
    [SerializeField] GameObject _gameOverPanel;
    [SerializeField] private float _currentTimeScale;

    public bool IsPaused { get => isPaused; set => isPaused = value; }
    public static TimeManager Instance { get => _instance; set => _instance = value; }

    private void Awake() {
        if (_instance != null && _instance != this) Destroy(this);
        else _instance = this;
        // Always unpause on start
        _currentTimeScale = 1.0f;
        Time.timeScale = _currentTimeScale;
        if (IsPaused) IsPaused = false;
    }

    private void Start() {
        GameManager.Instance.IsGameOver = false;
    }
    private void Update() {
        Time.timeScale = _currentTimeScale;
        if (isPaused || GameManager.Instance.IsGameOver) {
            if (!GameManager.Instance.IsGameOver) _pausePanel.SetActive(true);
            else if (GameManager.Instance.IsGameOver) _gameOverPanel.SetActive(true);
            _currentTimeScale = 0.0f;
        } else {
            _gameOverPanel.SetActive(false);
            _pausePanel.SetActive(false);
            _currentTimeScale = 1.0f;
        }
        if (Input.GetKeyDown(KeyCode.Escape)) {
            isPaused = !isPaused;
        }
    }
}
