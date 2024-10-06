using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour, IDamageable {
    private const float epsilon = 0.05f;

    [SerializeField] GameObject selectedOutline;
    [SerializeField] bool isSelected;
    [SerializeField] bool isAlive;

    [SerializeField] private string name;
    [SerializeField] private bool playerOwned = true;

    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;
    [SerializeField] private int defense;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float randomSpeed;

    [SerializeField] private Vector2 moveToPosition;
    [SerializeField] private Vector2 velocityVector;

    // Attack stuff
    [SerializeField] private int damageMin;
    [SerializeField] private int damageMax;
    [SerializeField] private float attackRange;
    [SerializeField] private float accuracy;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackTimer;

    // target
    [SerializeField] private GameObject currentTarget;

    public bool PlayerOwned { get => playerOwned; set => playerOwned = value; }
    public bool IsSelected { get => isSelected; set => isSelected = value; }

    private void Awake() {
        isSelected = false;
        currentHealth = maxHealth;
        moveToPosition = transform.position;
        currentTarget = null;
        attackTimer = 0.0f;
        isAlive = true;
    }

    void Start() {
        selectedOutline.SetActive(isSelected);
        StartCoroutine(SlowUpdate());
    }

    public void Attack(GameObject target) {
        IDamageable targetInterface = target.GetComponent<IDamageable>();
        Unit targetUnit = target.GetComponent<Unit>();

        if (!targetInterface.IsAlive()) {
            OnTargetKilled();
            return;
        }

        if (attackTimer <= 0f && Vector2.Distance(transform.position, target.transform.position) <= attackRange) {
            float attackRoll = Random.Range(0, 101);

            if (attackRoll <= accuracy) {
                int damageDealt = Random.Range(damageMin, damageMax) - targetUnit.defense;
                damageDealt = Mathf.Max(1, damageDealt);
                targetInterface.TakeDamage(damageDealt, this);
            }
            moveToPosition = transform.position;

            attackTimer = attackSpeed;
        }
    }

    public void TakeDamage(int damage, Unit attacker) {
        currentHealth -= damage;
        if (currentHealth <= 0) {
            Die(attacker);
        }
    }

    private void Die(Unit attacker) {
        isAlive = false;
        MouseController.Instance.RemoveUnit(this);
        // Death animation.
        attacker.OnTargetKilled();
        Destroy(gameObject);
    }

    IEnumerator SlowUpdate() {
        while (isAlive) {
            attackTimer = Mathf.Max(attackTimer - 0.1f, 0f);

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void OnTargetKilled() {
        moveToPosition = transform.position;
        currentTarget = null;
    }

    private void Update() {
        if (currentTarget != null) {
            Attack(currentTarget);
        }

        if (currentTarget != null && Vector2.Distance(transform.position, currentTarget.transform.position) <= attackRange) {
            return;
        }

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

    public void SetMoveToPosition(Vector2 position, GameObject target = null) {
        randomSpeed = moveSpeed * Random.Range(0.9f, 1.0f);
        moveToPosition = position;

        currentTarget = target;
    }

    public bool IsAlive() {
        return isAlive;
    }
}
