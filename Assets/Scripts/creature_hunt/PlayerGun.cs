using UnityEngine;

public class PlayerGun : MonoBehaviour {
    [SerializeField] Camera mainCamera;
    [SerializeField] int spreadCount;

    [SerializeField] bool useAngleZ;
    [SerializeField] bool useAngleY;
    [SerializeField] bool useAngleX;
    [SerializeField] float angleZ;
    [SerializeField] float angleY;
    [SerializeField] float angleX;
    [SerializeField] float spreadRadius;


    [SerializeField] float bulletRange;
    [SerializeField] LayerMask targetLayer;

    [SerializeField] GameObject pelletPrefab;

    private Vector3 crosshairPosition;

    private void Start() {
        spreadRadius = GameManager.Instance.GunSpread;
    }


    private void Update() {
        crosshairPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -5.0f));

        if (Input.GetButtonDown("Fire1")) {
            Shoot();
        }
    }

    void Shoot() {
        spreadRadius = GameManager.Instance.GunSpread;

        for (int i = 0; i < spreadCount; i++) {
            Vector2 randomOffset = Random.insideUnitCircle * spreadRadius;
            Vector3 shotLocation = crosshairPosition + new Vector3(randomOffset.x, randomOffset.y, 0.0f);
            Vector3 pelletPosition = shotLocation + Vector3.forward * 16;

            Vector3 direction = Vector3.forward;

            RaycastHit2D hit = Physics2D.Raycast(shotLocation, direction, bulletRange, targetLayer);

            Debug.DrawRay(shotLocation, direction * bulletRange, Color.red, 10.0f);

            if (hit) {
                Debug.Log("Hit");
                if (((1 << hit.collider.gameObject.layer) & targetLayer) != 0) {
                    Debug.Log("Hit Enemy");
                    //Destroy(hit.collider.gameObject);
                }
            }

            GameObject pellet = Instantiate(pelletPrefab, pelletPosition, Quaternion.identity);

            Destroy(pellet, 5.0f);
        }
    }
}
