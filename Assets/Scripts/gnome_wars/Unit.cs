using UnityEngine;

public class Unit : MonoBehaviour {
    [SerializeField] GameObject selectedOutline;
    [SerializeField] bool isSelected;

    [SerializeField] private string name;
    [SerializeField] private bool playerOwned = true;

    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private int damage;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float randomSpeed;
    [SerializeField] private float attackSpeed;

    [SerializeField] private Vector2 moveToPosition;
    [SerializeField] private Vector2 velocityVector;

    private float epsilon = 0.05f;

    public bool PlayerOwned { get => playerOwned; set => playerOwned = value; }
    public bool IsSelected { get => isSelected; set => isSelected = value; }

    private void Awake() {
        isSelected = false;
        currentHealth = maxHealth;
        moveToPosition = transform.position;
    }

    void Start() {
        selectedOutline.SetActive(isSelected);
    }

    private void Update() {
        if (moveToPosition != null && Vector2.Distance(transform.position, moveToPosition) > epsilon) {
            float moveStep = randomSpeed * Time.deltaTime;

            this.transform.position = Vector2.MoveTowards(transform.position, moveToPosition, moveStep);

            moveToPosition = Vector2.Distance(transform.position, moveToPosition) < epsilon ? transform.position : moveToPosition;
        }
    }

    public void Select() {
        isSelected = true;
        selectedOutline.SetActive(isSelected);
    }

    public void Deselect() {
        isSelected = false;
        selectedOutline.SetActive(isSelected);
    }

    public void SetMoveToPosition(Vector2 position) {
        randomSpeed = moveSpeed * Random.Range(0.9f, 1.0f);
        moveToPosition = position;
    }
}
