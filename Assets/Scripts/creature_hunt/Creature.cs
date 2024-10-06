using System.Collections;
using UnityEngine;

public class Creature : MonoBehaviour, IHealth {
    [SerializeField] bool changeDirection;

    [SerializeField] float moveSpeed;
    [SerializeField] float minMoveSpeed;
    [SerializeField] float maxMoveSpeed;
    [SerializeField] float minDirectionChangeInterval;
    [SerializeField] float maxDirectionChangeInterval;

    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;
    [SerializeField] bool isAlive;

    [SerializeField] int timeIncrement;
    [SerializeField] int scoreIncrement;

    [SerializeField] Vector2 direction;

    public int Health { get => currentHealth; set => currentHealth = value; }

    private void Awake() {
        changeDirection = true;
    }

    private void Start() {
        //StartCoroutine(ChangeDirection());
        currentHealth = maxHealth;
        isAlive = true;

        direction = transform.position.x < 0 ? Vector2.right : Vector2.left;
        GetComponent<SpriteRenderer>().flipX = transform.position.x > 0;

        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
    }

    private void Update() {
        transform.Translate(moveSpeed * Time.deltaTime * direction);

        if (transform.position.y > 6.0f || transform.position.x > 10.0f || transform.position.x < -10.0f) {
            Destroy(gameObject);
        }
    }

    IEnumerator ChangeDirection() {
        while (changeDirection) {
            float x = Random.Range(-1f, 1f);
            float y = Random.Range(0.0f, 1f);

            direction = new(x, y);

            float interval = Random.Range(minDirectionChangeInterval, maxDirectionChangeInterval);
            yield return new WaitForSeconds(interval);
        }
    }

    public void TakeDamage(int damage) {
        currentHealth -= damage;

        if (currentHealth <= 0f) {
            Die();
        }
    }

    public bool IsAlive() {
        return isAlive;
    }

    public void Die() {
        isAlive = false;
        GameManager.Instance.ModifyTime(timeIncrement);
        ScoreManager.Instance.CurrentScore = scoreIncrement;

        Destroy(gameObject);
    }
}
