using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {
    private Vector2 dragStart;
    private Vector2 dragEnd;
    private bool isDragging;

    [SerializeField] private List<Unit> selectedUnits;
    [SerializeField] private GameObject selectionArea;
    [SerializeField] private int scaleMultiplier;

    private void Awake() {
        isDragging = false;
        selectedUnits = new List<Unit>();
        selectionArea.SetActive(false);
    }

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            StartDrag();
        }

        if (isDragging) {
            Drag();
        }

        if (Input.GetMouseButtonUp(0)) {
            EndDrag();
        }
    }
    private bool ShiftPressed => Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    private bool ControlPressed => Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl);

    private void DeselectAllUnits() {
        if (ShiftPressed || ControlPressed) {
            return;
        }

        foreach (Unit unit in selectedUnits) {
            unit.Deselect();
        }

        selectedUnits.Clear();
    }


    private void HandleUnitSelection(Unit unit) {
        if (unit.IsSelected) {
            unit.Deselect();
            selectedUnits.Remove(unit);
        } else {
            unit.Select();
            selectedUnits.Add(unit);
        }
    }

    private void StartDrag() {
        dragStart = GetMouseWorldPosition();
        selectionArea.SetActive(true);

        isDragging = true;
    }

    private void Drag() {
        Vector3 mousePosition = GetMouseWorldPosition();

        Vector3 lowerLeft = new(Mathf.Min(dragStart.x, mousePosition.x), Mathf.Min(dragStart.y, mousePosition.y));
        Vector3 upperRight = new(Mathf.Max(dragStart.x, mousePosition.x), Mathf.Max(dragStart.y, mousePosition.y));

        selectionArea.transform.position = lowerLeft;

        selectionArea.transform.localScale = upperRight - lowerLeft;
    }

    private void EndDrag() {
        dragEnd = GetMouseWorldPosition();
        isDragging = false;

        selectionArea.SetActive(false);

        Collider2D[] colliders = Physics2D.OverlapAreaAll(dragStart, dragEnd);

        DeselectAllUnits();

        foreach (var collider in colliders) {
            Unit unit = collider.GetComponent<Unit>();

            if (!unit.PlayerOwned || unit == null) {
                return;
            }

            HandleUnitSelection(unit);
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

