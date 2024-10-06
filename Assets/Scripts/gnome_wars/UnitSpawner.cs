using System.Collections;
using UnityEngine;

public class UnitSpawner : MonoBehaviour, IDamageable {
    [SerializeField] GameObject unitPrefab;
    [SerializeField] GameObject unitParent;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float spawnInterval = 5.0f;
    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;
    [SerializeField] bool shouldSpawn = true;
    [SerializeField] bool isPlayerBase;

    private Coroutine spawnerRunning;

    private void Awake() {
        currentHealth = maxHealth;
    }

    private void Start() {
        spawnerRunning = StartCoroutine(SpawnUnits());
    }

    private void Update() {
        if (currentHealth <= 0) {
            Destroy(gameObject);
        }
    }

    IEnumerator SpawnUnits() {
        while (shouldSpawn) {
            GameObject newUnit = Instantiate(unitPrefab, transform.position, Quaternion.identity, unitParent.transform);
            Unit unit = newUnit.GetComponent<Unit>();
            unit.SetMoveToPosition(spawnPoint.position);
            unit.PlayerOwned = isPlayerBase;

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void StopSpawning() {
        if (spawnerRunning == null) {
            return;
        }

        StopCoroutine(spawnerRunning);
        spawnerRunning = null;
    }

    public void TakeDamage(int damage, Unit attacker) {
        throw new System.NotImplementedException();
    }

    public bool IsAlive() {
        return currentHealth > 0;
    }
}
