using UnityEngine;

public class Shooter : MonoBehaviour {
    private void OnMouseDown() {
        Destroy(gameObject);

        ScoreManager.Instance.CurrentScore = 10;
    }
}
