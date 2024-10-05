using System.Collections;
using UnityEngine;

public class UnitSpawner : MonoBehaviour {
    [SerializeField] GameObject unitPrefab;
    [SerializeField] GameObject unitParent;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float spawnInterval = 5.0f;
    [SerializeField] bool shouldSpawn = true;

    private Coroutine spawnerRunning;

    private void Start() {
        spawnerRunning = StartCoroutine(SpawnUnits());
    }

    IEnumerator SpawnUnits() {
        while (shouldSpawn) {
            Instantiate(unitPrefab, spawnPoint.position, Quaternion.identity, unitParent.transform);

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
}
