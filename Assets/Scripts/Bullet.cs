using UnityEngine;

public class Bullet : MonoBehaviour {
    [SerializeField] private float lifetime = 2f;
    private float elapsedTime = 0f;

    private float bulletScale;

    private void Awake() {
        bulletScale = GameManager.Instance.BulletSize;
    }

    void Update() {
        elapsedTime += Time.deltaTime;

        float scale = Mathf.Lerp(bulletScale, 0f, elapsedTime / lifetime);
        transform.localScale = new Vector3(scale, scale, 1f);

        if (elapsedTime >= lifetime || scale <= 0.0005f) {
            Destroy(gameObject);
        }
    }
}
