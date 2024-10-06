using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {
    #region Getters
    public bool IsGameOver { get => _isGameOver; set => _isGameOver = value; }
    public static GameManager Instance { get => _instance; private set => _instance = value; }
    public float GunSpread { get => gunSpread; set => gunSpread = value; }

    #endregion

    #region Singleton
    private static GameManager _instance;


    private void CreateSingleton() {
        if (_instance != null && _instance != this) Destroy(gameObject);
        else {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    [Header("Game States")]
    [SerializeField] bool _isGameOver = false;

    [Header("Game Settings")]
    [SerializeField] float gameTime;
    [SerializeField] TextMeshProUGUI timeText;

    [Header("Gun Settings")]
    [SerializeField] float gunSpread;

    private void Awake() {
        CreateSingleton();
        _isGameOver = false;
        if (gameTime <= 0) { gameTime = 90.0f; }
    }

    private void Start() {
        StartCoroutine(UpdateTime());
    }

    private void Update() {
        if (_isGameOver) return;

        if (gameTime <= 0) { _isGameOver = true; }
    }

    IEnumerator UpdateTime() {
        while (!_isGameOver) {
            gameTime--;
            timeText.text = $"{gameTime:F0}";
            yield return new WaitForSeconds(1.0f);
        }
    }
}
