using UnityEngine;

public class PlayerGun : MonoBehaviour {
    [SerializeField] Camera mainCamera;
    [SerializeField] int spreadCount;
    [SerializeField] float spreadAngle;

    [SerializeField] bool useAngleZ;
    [SerializeField] bool useAngleY;
    [SerializeField] bool useAngleX;
    [SerializeField] float angleZ;
    [SerializeField] float angleY;
    [SerializeField] float angleX;
    [SerializeField] float spreadRadius = 1.0f;


    [SerializeField] float bulletRange;
    [SerializeField] LayerMask targetLayer;

    [SerializeField] GameObject pelletPrefab;
    [SerializeField] float pelletSpeed = 10.0f;

    private Vector3 crosshairPosition;
    private LineRenderer lineRenderer;

    private void Start() {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 150;
        lineRenderer.startWidth = 0.01f;
        lineRenderer.endWidth = 0.01f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.red;
    }


    private void Update() {
        crosshairPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -5.0f));


        // Update the circle outline
        UpdateCircleOutline();

        if (Input.GetButtonDown("Fire1")) {
            Shoot();
            //FireShotgun();
        }
    }

    void FireShotgun() {
        for (int i = 0; i < spreadCount; i++) {

            angleX = useAngleX ? Random.Range(-spreadAngle, spreadAngle) : 0.0f;
            angleY = useAngleY ? Random.Range(-spreadAngle, spreadAngle) : 0.0f;
            angleZ = useAngleZ ? Random.Range(-spreadAngle, spreadAngle) : 0.0f;
            Vector3 direction = Quaternion.Euler(angleX, angleY, angleZ) * Vector3.forward;

            RaycastHit2D hit = Physics2D.Raycast(crosshairPosition, direction, bulletRange, targetLayer);

            Debug.DrawRay(crosshairPosition, direction * bulletRange, Color.red, 10.0f);


            if (hit) {
                if (((1 << hit.collider.gameObject.layer) & targetLayer) != 0) {
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }

    void UpdateCircleOutline() {
        float angleStep = 360f / lineRenderer.positionCount;
        for (int i = 0; i < lineRenderer.positionCount; i++) {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 point = new Vector3(crosshairPosition.x + Mathf.Cos(angle) * spreadRadius,
                                         crosshairPosition.y + Mathf.Sin(angle) * spreadRadius,
                                         0);
            lineRenderer.SetPosition(i, point);
        }
    }

    void Shoot() {
        for (int i = 0; i < spreadCount; i++) {
            // Generate a random position within a circle (spread)
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

            // Instantiate the pellet at the calculated position
            GameObject pellet = Instantiate(pelletPrefab, pelletPosition, Quaternion.identity);


            // Destroy the pellet after a certain time
            Destroy(pellet, 5.0f);
        }
    }

    private void Shoot1() {
        // Get the mouse position on the screen (2D)
        Vector3 mousePosition = Input.mousePosition;

        // Convert the mouse position to a world position where it hits the plane
        Vector3 worldTargetPosition = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, mainCamera.nearClipPlane));
        //Vector3 worldTargetPosition = mousePosition;

        // Origin point (Camera position)
        Vector3 shootOrigin = mainCamera.transform.position;

        // For each pellet, calculate a random direction within the spread
        for (int i = 0; i < spreadCount; i++) {
            // Calculate a random spread direction
            Vector3 spreadDirection = GetSpreadDirection(worldTargetPosition - shootOrigin);

            // Perform a raycast from the camera to the spread direction
            RaycastHit2D hit = Physics2D.Raycast(shootOrigin, spreadDirection, bulletRange, targetLayer);

            if (hit.collider != null) {
                Debug.Log("Hit: " + hit.collider.name);
                // Handle the hit logic (destroy the object, deal damage, etc.)
                Destroy(hit.collider.gameObject); // Example: Destroy the target (duck)
            }

            // Debug line to visualize the spray direction
            InstantiatePellet(shootOrigin, spreadDirection);
            Debug.DrawLine(shootOrigin, shootOrigin + spreadDirection * bulletRange, Color.red, 1f);
        }
    }

    // Generate a random direction within the spread angle
    Vector2 GetSpreadDirection(Vector3 baseDirection) {
        // Random angle deviation within the spread
        float randomAngleX = Random.Range(-spreadAngle / 2f, spreadAngle / 2f);
        float randomAngleY = Random.Range(-spreadAngle / 2f, spreadAngle / 2f);

        // Apply the random angle deviation to the base direction
        Quaternion spreadRotation = Quaternion.Euler(randomAngleX, randomAngleY, 0);

        // Return the spread direction
        return spreadRotation * baseDirection.normalized;
    }

    // Instantiate the pellet prefab to visualize the spread
    void InstantiatePellet(Vector3 origin, Vector3 direction) {
        // Instantiate the pellet prefab at the shoot origin
        GameObject pellet = Instantiate(pelletPrefab, origin, Quaternion.identity);

        // Add velocity to the pellet to make it move in the direction of the spread
        Rigidbody2D rb = pellet.GetComponent<Rigidbody2D>();
        if (rb != null) {
            rb.velocity = direction * pelletSpeed;
        }

        // Optionally, destroy the pellet after a certain time
        Destroy(pellet, 2f); // Destroy the pellet after 2 seconds
    }
}
