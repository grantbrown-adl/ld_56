using UnityEditor;
using UnityEngine;

public class FolderCreation : MonoBehaviour {
    [MenuItem("GameObject/Create Basic Folder Structure")]
    static void CreateFolders() {
        AssetDatabase.CreateFolder("Assets", "Scripts");
        AssetDatabase.CreateFolder("Assets", "Sounds");
        AssetDatabase.CreateFolder("Assets", "Prefabs");
        AssetDatabase.CreateFolder("Assets", "Materials");
        AssetDatabase.CreateFolder("Assets", "Art");
        AssetDatabase.CreateFolder("Assets", "Animation");
    }
}
