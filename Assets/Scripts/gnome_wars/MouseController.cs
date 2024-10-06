using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour {
    #region Getters
    public static MouseController Instance { get => _instance; private set => _instance = value; }

    #endregion

    #region Singleton
    private static MouseController _instance;


    private void CreateSingleton() {
        if (_instance != null && _instance != this) Destroy(gameObject);
        else {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    private Vector2 dragStart;
    private Vector2 dragEnd;
    private bool isDragging;

    [SerializeField] private List<Unit> selectedUnits;
    [SerializeField] private GameObject selectionArea;
    [SerializeField] private int scaleMultiplier;
    [SerializeField] private float formationDistance;

    [SerializeField] private float[] formationDistances;
    [SerializeField] private int[] formationAmounts;
    [SerializeField] private int formationRowLength;
    [SerializeField] private float formationSpacing;
    [SerializeField] private float noiseAmount;

    private void Awake() {
        CreateSingleton();
        isDragging = false;
        selectedUnits = new List<Unit>();
        selectionArea.SetActive(false);
    }

    void Update() {
        HandleMouseLeftClick();
        HandleMouseRightClick();
    }

    public void RemoveUnit(Unit unit) {
        if (selectedUnits.Contains(unit)) {
            selectedUnits.Remove(unit);
        }
    }

    private void HandleMouseLeftClick() {
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

    private void HandleMouseRightClick() {
        if (!Input.GetMouseButtonDown(1)) {
            return;
        }

        Vector2 mousePosition = GetMouseWorldPosition();
        Collider2D rightClickLocation = Physics2D.OverlapPoint(mousePosition);


        //Unit attackTarget = rightClickLocation != null ? rightClickLocation.GetComponent<Unit>() : null;
        GameObject attackTarget = (rightClickLocation != null) && (rightClickLocation.GetComponent<IDamageable>() != null) ? rightClickLocation.gameObject : null;

        //List<Vector2> formationPositions = GetFormationPosition(mousePosition, formationDistance, selectedUnits.Count);
        //List<Vector2> formationPositions = GetFormationPosition(mousePosition, formationDistances, formationAmounts);
        //int index = 0;

        List<Vector2> formationPositions = GetBoxFormationPositions(mousePosition, selectedUnits.Count, formationRowLength, formationSpacing);
        int index = 0;


        foreach (Unit unit in selectedUnits) {
            unit.SetMoveToPosition(formationPositions[index], attackTarget);
            index = (index + 1) % formationPositions.Count;
        }
    }

    private List<Vector2> GetBoxFormationPositions(Vector2 start, int totalUnits, int rowLength, float spacing) {
        List<Vector2> positions = new();

        // Calculate number of rows needed
        int rows = Mathf.CeilToInt((float)totalUnits / rowLength);

        // Position units in rows and columns
        for (int row = 0; row < rows; row++) {
            for (int col = 0; col < rowLength; col++) {
                int currentUnit = row * rowLength + col;

                // Stop if we've added all units
                if (currentUnit >= totalUnits) {
                    return positions;
                }

                float noiseX = Random.Range(-noiseAmount, noiseAmount);
                float noiseY = Random.Range(-noiseAmount, noiseAmount);

                // Calculate position of each unit based on row and column
                float xOffset = col * spacing + noiseX;
                float yOffset = row * spacing + noiseY;

                Vector2 position = new Vector2(start.x + xOffset, start.y - yOffset);
                positions.Add(position);
            }
        }

        return positions;
    }

    private List<Vector2> GetFormationPosition(Vector2 start, float[] distances, int[] amounts) {
        List<Vector2> positions = new();

        positions.Add(start);

        for (int i = 0; i < distances.Length; i++) {
            positions.AddRange(GetFormationPosition(start, distances[i], amounts[i]));
        }

        return positions;
    }

    private List<Vector2> GetFormationPosition(Vector2 start, float distance, int amount) {
        List<Vector2> positions = new();

        for (int i = 0; i < amount; i++) {
            float angle = i * (360.0f / amount);
            Vector2 direction = ApplyRotationToVector(new Vector2(1, 0), angle);
            Vector2 position = start + direction * distance;

            positions.Add(position);
        }

        return positions;
    }

    private Vector2 ApplyRotationToVector(Vector2 vector, float angle) {
        return Quaternion.Euler(0, 0, angle) * vector;
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
        if (unit.IsSelected && !ShiftPressed) {
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

            if (unit == null || !unit.PlayerOwned) {
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

