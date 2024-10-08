using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerGun : MonoBehaviour {
    #region Getters
    public static PlayerGun Instance { get => _instance; private set => _instance = value; }
    public Vector3 SwayOffset { get => swayOffset; set => swayOffset = value; }
    public Vector3 CrosshairPosition { get => crosshairPosition; set => crosshairPosition = value; }

    #endregion

    #region Singleton
    private static PlayerGun _instance;


    private void CreateSingleton() {
        if (_instance != null && _instance != this) Destroy(gameObject);
        else {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    [SerializeField] Camera mainCamera;
    [SerializeField] int spreadCount;
    [SerializeField] int damage;

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

    [Header("Sway")]
    [SerializeField] float maxSwayX;
    [SerializeField] float maxSwayY;
    [SerializeField] float minSwayX;
    [SerializeField] float minSwayY;
    [SerializeField] float swaySpeed;
    [SerializeField] float minSwayInterval;
    [SerializeField] float maxSwayInterval;
    [SerializeField] Vector3 swayOffset;
    private Vector3 swayTarget;
    private float swayTimer;
    private float nextSwayChange;


    private Vector3 crosshairPosition;

    [SerializeField] float swayAmount = 0.1f;
    //[SerializeField] float swaySpeed = 2.0f;
    [SerializeField] float smoothFactor = 5.0f;
    private Vector2 currentSwayOffset;
    private Vector2 targetSwayOffset;

    [Header("Camera Shake")]
    [SerializeField] float shakeDuration;
    [SerializeField] float shakeMagnitude;

    [Header("Bullet stuff")]
    [SerializeField] int currentBullets;
    [SerializeField] int maxBullets;
    [SerializeField] float reloadTime;
    [SerializeField] bool isReloading;

    [Header("Bullet UI")]
    [SerializeField] Transform bulletHolder;
    [SerializeField] GameObject loadedBulletPrefab;
    [SerializeField] GameObject emptyBulletPrefab;
    [SerializeField] TextMeshProUGUI reloadText;

    private void Awake() {
        CreateSingleton();
    }

    private void Start() {
        spreadRadius = GameManager.Instance.GunSpread;
        if (maxBullets == 0) maxBullets = 6;
        currentBullets = maxBullets;
        isReloading = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        reloadText.gameObject.SetActive(false);


        SetNewTargetSway();
        InvokeRepeating(nameof(SetNewTargetSway), 0.0f, Random.Range(0.5f, 2.0f));
        UpdateBulletUI();
    }


    private void Update() {
        if (GameManager.Instance.IsGameOver || TimeManager.Instance.IsPaused) {
            return;
        }

        swayTimer += Time.deltaTime;
        swayOffset = Vector3.Lerp(Vector3.zero, swayTarget, swaySpeed * Time.deltaTime);

        // Calculate target sway offsets based on time
        //float targetSwayOffsetX = Mathf.Sin(Time.time * swaySpeed) * swayAmount;
        //float targetSwayOffsetY = Mathf.Cos(Time.time * swaySpeed) * swayAmount;

        // Smoothly interpolate towards the target sway offsets
        //currentSwayOffset.x = Mathf.Lerp(currentSwayOffset.x, targetSwayOffsetX, Time.deltaTime * smoothFactor);
        //currentSwayOffset.y = Mathf.Lerp(currentSwayOffset.y, targetSwayOffsetY, Time.deltaTime * smoothFactor);

        // Smoothly interpolate the current sway offset towards the random target sway offset
        currentSwayOffset.x = Mathf.Lerp(currentSwayOffset.x, targetSwayOffset.x, Time.deltaTime * smoothFactor);
        currentSwayOffset.y = Mathf.Lerp(currentSwayOffset.y, targetSwayOffset.y, Time.deltaTime * smoothFactor);

        // Update the final sway offset
        SwayOffset = currentSwayOffset;


        //crosshairPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -5.0f));
        crosshairPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -5.0f)) + swayOffset;


        CheckSway();

        if (Input.GetButtonDown("Fire1")) {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            if (isReloading) {
                StopCoroutine(Reload());
            } else {
                StartCoroutine(Reload());
            }
        }
    }

    IEnumerator Reload() {
        isReloading = true;
        while (currentBullets < maxBullets && isReloading) {
            currentBullets++;
            UpdateBulletUI();
            // play reload sound
            yield return new WaitForSeconds(reloadTime);
        }
        isReloading = false;
    }

    private void CheckSway() {
        if (swayTimer >= nextSwayChange) {
            SetNewSwayTarget();
            swayTimer = 0f;
            nextSwayChange = Random.Range(minSwayInterval, maxSwayInterval);
        }
    }

    private void SetNewTargetSway() {
        targetSwayOffset.x = Random.Range(-swayAmount, swayAmount);  // New random X target
        targetSwayOffset.y = Random.Range(-swayAmount, swayAmount);  // New random Y target
    }

    private void SetNewSwayTarget() {
        float newSwayX = Random.Range(-maxSwayX, maxSwayX);
        float newSwayY = Random.Range(-maxSwayY, maxSwayY);
        swayTarget = new Vector3(newSwayX, newSwayY, 0.0f);
    }


    void Shoot() {
        isReloading = false;

        if (currentBullets <= 0) {
            // Handle bullet click here
            return;
        }

        StartCoroutine(CameraShake.Instance.PerformShake(shakeDuration, shakeMagnitude));

        spreadRadius = GameManager.Instance.GunSpread;

        float boxSizing = GameManager.Instance.BulletSize;

        for (int i = 0; i < spreadCount; i++) {
            Vector2 randomOffset = Random.insideUnitCircle * spreadRadius;
            Vector3 shotLocation = crosshairPosition + new Vector3(randomOffset.x, randomOffset.y, 0.0f);
            Vector3 pelletPosition = shotLocation + Vector3.forward * 16;

            Vector3 direction = Vector3.forward;

            RaycastHit2D hit = Physics2D.BoxCast(shotLocation, new Vector2(boxSizing, boxSizing), 0.0f, direction, bulletRange, targetLayer);

            //RaycastHit2D hit = Physics2D.Raycast(shotLocation, direction, bulletRange, targetLayer);

            Debug.DrawRay(shotLocation, direction * bulletRange, Color.red, 10.0f);

            if (hit && hit.collider.GetComponent<IHealth>() != null) {
                ScoreManager.Instance.Hits++;
                hit.collider.GetComponent<IHealth>().TakeDamage(damage);
                //Debug.Log("Hit");
                //if (((1 << hit.collider.gameObject.layer) & targetLayer) != 0) {
                //    Debug.Log("Hit Enemy");
                //    //Destroy(hit.collider.gameObject);
                //}
            }

            Instantiate(pelletPrefab, pelletPosition, Quaternion.identity);
        }

        ScoreManager.Instance.Shots++;
        currentBullets--;
        UpdateBulletUI();
    }

    private void UpdateBulletUI() {
        reloadText.gameObject.SetActive(currentBullets <= 0);

        foreach (Transform child in bulletHolder) {
            Destroy(child.gameObject);
        }

        // Display the current bullet count
        for (int i = 0; i < maxBullets; i++) {
            GameObject bulletSprite = i < currentBullets ? loadedBulletPrefab : emptyBulletPrefab;
            Instantiate(bulletSprite, bulletHolder);
        }
    }
}
