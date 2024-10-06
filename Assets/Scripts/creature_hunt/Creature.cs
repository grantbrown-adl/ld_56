using System.Collections;
using UnityEngine;

public class Creature : MonoBehaviour {
    [SerializeField] bool changeDirection;

    [SerializeField] float moveSpeed;
    [SerializeField] float minMoveSpeed;
    [SerializeField] float maxMoveSpeed;
    [SerializeField] float minDirectionChangeInterval;
    [SerializeField] float maxDirectionChangeInterval;

    [SerializeField] Vector2 direction;

    private void Awake() {
        changeDirection = true;
    }

    private void Start() {
        //StartCoroutine(ChangeDirection());

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

}
