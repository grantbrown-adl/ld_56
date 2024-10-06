using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {
    [SerializeField] float timeToLive;

    private void Awake() {
        Destroy(gameObject, timeToLive);
    }
}
