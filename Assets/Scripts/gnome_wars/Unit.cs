using UnityEngine;

public class Unit : MonoBehaviour {
    [SerializeField] GameObject selectedOutline;
    [SerializeField] bool isSelected;

    [SerializeField] private string name;
    [SerializeField] private bool playerOwned = true;

    public bool PlayerOwned { get => playerOwned; set => playerOwned = value; }
    public bool IsSelected { get => isSelected; set => isSelected = value; }

    private void Awake() {
        isSelected = false;
    }

    void Start() {
        selectedOutline.SetActive(isSelected);
    }

    public void Select() {
        isSelected = true;
        selectedOutline.SetActive(isSelected);
    }

    public void Deselect() {
        isSelected = false;
        selectedOutline.SetActive(isSelected);
    }
}
