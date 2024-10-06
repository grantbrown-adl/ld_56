using UnityEngine;

public class CrosshairTest : MonoBehaviour {
    public RectTransform crosshairRectTransform;

    void Update() {
        Vector3 mousePosition = Input.mousePosition;
        crosshairRectTransform.position = mousePosition;
    }
}
