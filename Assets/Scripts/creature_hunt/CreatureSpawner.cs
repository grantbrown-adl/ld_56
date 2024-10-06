using System.Collections;
using UnityEngine;

public class CreatureSpawner : MonoBehaviour {
    [SerializeField] bool shouldSpawn;

    [SerializeField] GameObject[] creaturePrefabs;
    [SerializeField] Transform spawnerParent;
    [SerializeField] float spawnInterval;
    [SerializeField] float increaseDifficultyAfter = 10.0f;
    [SerializeField] float difficultyMultiplier = 0.9f;

    private void Awake() {
        shouldSpawn = true;
    }

    private void Start() {
        StartCoroutine(SpawnCreature());
        StartCoroutine(IncreaseDifficulty());
    }

    IEnumerator SpawnCreature() {
        while (shouldSpawn) {
            float y = Random.Range(-4.0f, 4.0f);

            Vector2 spawnPosition = new(Random.value > 0.5f ? -10.0f : 10.0f, y);

            Instantiate(creaturePrefabs[0], spawnPosition, Quaternion.identity, spawnerParent);

            yield return new WaitForSeconds(spawnInterval);
        }
    }
    IEnumerator IncreaseDifficulty() {
        while (shouldSpawn) {
            yield return new WaitForSeconds(increaseDifficultyAfter);
            spawnInterval *= difficultyMultiplier;
            spawnInterval = Mathf.Max(spawnInterval, 0.5f);
        }
    }
}
