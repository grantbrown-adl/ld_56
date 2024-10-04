using UnityEngine;

public class GameManager : MonoBehaviour {
    #region Getters
    public bool IsGameOver { get => _isGameOver; set => _isGameOver = value; }
    public static GameManager Instance { get => _instance; private set => _instance = value; }

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


    private void Awake() {
        CreateSingleton();
        _isGameOver = false;
    }


    private void Update() {
        if (_isGameOver) return;
    }
}
