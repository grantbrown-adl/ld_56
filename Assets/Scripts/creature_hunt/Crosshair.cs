using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour {
    [SerializeField] Image crosshairImage;

    private void Update() {
        //Vector2 mousePosition = GetMouseWorldPosition();
        Vector2 mousePosition = Input.mousePosition;
        Vector3 thissy = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
        crosshairImage.rectTransform.position = thissy;
    }

    public static Vector3 GetMouseWorldPosition() {
        Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }

    public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPosition, Camera worldCamera) {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}
