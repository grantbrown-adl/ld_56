using System.Collections;
using UnityEngine;

public class ButtonPressParticle : MonoBehaviour {
    private ParticleSystem _particleSystem;
    private void Awake() {
        _particleSystem = GetComponent<ParticleSystem>();
    }
    private void Start() {
        _particleSystem.Play();
        StartCoroutine(DestroyAfterTime());
    }

    IEnumerator DestroyAfterTime() {
        yield return new WaitForSecondsRealtime(5);
        Destroy(this.gameObject);
    }
}
