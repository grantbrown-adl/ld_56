using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour {
    [SerializeField] Image crosshairImage;
    [SerializeField] float spreadRadius;
    [SerializeField] float spreadMultiplier; //2.2 is the sweet spot

    private void Start() {
        StartCoroutine(SlowUpdate());
    }

    private void Update() {
        Vector2 mousePosition = Input.mousePosition;
        crosshairImage.transform.position = mousePosition;
        float imageSize = spreadRadius * spreadMultiplier;

        //crosshairImage.sizeDelta = new Vector2(imageSize, imageSize);
        crosshairImage.transform.localScale = new Vector2(imageSize, imageSize);
    }

    IEnumerator SlowUpdate() {
        while (true) {
            spreadRadius = GameManager.Instance.GunSpread;
            yield return new WaitForSeconds(0.25f);
        }
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
