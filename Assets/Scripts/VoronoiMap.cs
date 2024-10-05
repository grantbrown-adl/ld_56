using System.Collections.Generic;
using UnityEngine;

//public class VoronoiMap : MonoBehaviour {
//    public int seedCount = 15; // Number of regions
//    public Color[] regionColors; // Random colors for regions
//    private List<Vector2> seedPoints = new List<Vector2>();
//    private Texture2D voronoiTexture;
//    private Camera mainCamera;
//    public GameObject voronoiMap;

//    void Start() {
//        mainCamera = Camera.main;

//        regionColors = new Color[seedCount];
//        for (int i = 0; i < seedCount; i++) {
//            regionColors[i] = new Color(Random.value, Random.value, Random.value);
//        }

//        InitializeSeedPoints();
//        GenerateVoronoiMap();
//    }

//    void InitializeSeedPoints() {
//        for (int i = 0; i < seedCount; i++) {
//            // Randomly generate seed points within screen bounds
//            Vector2 randomPoint = new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height));
//            seedPoints.Add(randomPoint);
//        }
//    }

//    void GenerateVoronoiMap() {
//        voronoiTexture = new Texture2D(Screen.width, Screen.height);
//        for (int x = 0; x < voronoiTexture.width; x++) {
//            for (int y = 0; y < voronoiTexture.height; y++) {
//                Vector2 currentPoint = new Vector2(x, y);
//                int nearestSeedIndex = GetNearestSeedIndex(currentPoint);
//                voronoiTexture.SetPixel(x, y, regionColors[nearestSeedIndex]);
//            }
//        }

//        voronoiTexture.Apply();
//        //mainCamera.gameObject.GetComponent<Renderer>().material.mainTexture = voronoiTexture;
//        voronoiMap.GetComponent<Renderer>().material.mainTexture = voronoiTexture;
//    }

//    int GetNearestSeedIndex(Vector2 point) {
//        float minDistance = float.MaxValue;
//        int nearestSeedIndex = 0;

//        for (int i = 0; i < seedPoints.Count; i++) {
//            float distance = Vector2.Distance(point, seedPoints[i]);
//            if (distance < minDistance) {
//                minDistance = distance;
//                nearestSeedIndex = i;
//            }
//        }

//        return nearestSeedIndex;
//    }
//}
public class VoronoiMapWithBoundaries : MonoBehaviour {
    public int seedCount = 20; // Number of seeds
    public Color lineColor = Color.black; // Color for boundary lines
    public Color backgroundColor = Color.clear; // Color for non-boundary areas
    private List<Vector2> seedPoints = new List<Vector2>();
    private Texture2D voronoiTexture;

    void Start() {
        // Create and scale Quad to screen size
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.transform.localScale = new Vector3(Screen.width / 100f, Screen.height / 100f, 1f);

        // Create Material for Quad
        Material voronoiMaterial = new Material(Shader.Find("Unlit/Texture")); // Ensure we use an Unlit shader that supports textures
        quad.GetComponent<MeshRenderer>().material = voronoiMaterial;

        // Initialize seed points for Voronoi generation
        InitializeSeedPoints();

        // Generate the Voronoi texture
        voronoiTexture = GenerateVoronoiTexture(Screen.width, Screen.height);  // Pass correct width and height

        // Apply the texture to the Quad's material
        voronoiMaterial.mainTexture = voronoiTexture;
    }

    void InitializeSeedPoints() {
        for (int i = 0; i < seedCount; i++) {
            Vector2 randomPoint = new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height));
            seedPoints.Add(randomPoint);
        }
    }

    // Pass width and height as parameters for easier handling
    Texture2D GenerateVoronoiTexture(int textureWidth, int textureHeight) {
        // Create a new Texture2D with screen width and height
        Texture2D texture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);

        // For each pixel in the texture
        for (int x = 0; x < texture.width; x++) {
            for (int y = 0; y < texture.height; y++) {
                Vector2 currentPoint = new Vector2(x, y);
                int nearestSeedIndex = GetNearestSeedIndex(currentPoint);

                // Check if this pixel is a boundary pixel
                if (IsBoundaryPixel(x, y, nearestSeedIndex, textureWidth, textureHeight)) {
                    texture.SetPixel(x, y, lineColor); // Set boundary line color
                } else {
                    texture.SetPixel(x, y, Color.clear); // Set background color
                }
            }
        }

        texture.Apply();
        return texture;
    }

    int GetNearestSeedIndex(Vector2 point) {
        float minDistance = float.MaxValue;
        int nearestSeedIndex = 0;

        // Find the nearest seed for this pixel
        for (int i = 0; i < seedPoints.Count; i++) {
            float distance = Vector2.Distance(point, seedPoints[i]);
            if (distance < minDistance) {
                minDistance = distance;
                nearestSeedIndex = i;
            }
        }

        return nearestSeedIndex;
    }

    // Check if a pixel is a boundary by comparing its neighbors
    bool IsBoundaryPixel(int x, int y, int nearestSeedIndex, int textureWidth, int textureHeight) {
        // Ensure we're checking within bounds and avoid null references
        int left = Mathf.Max(0, x - 1);
        int right = Mathf.Min(textureWidth - 1, x + 1);
        int up = Mathf.Max(0, y + 1);
        int down = Mathf.Min(textureHeight - 1, y - 1);

        // Compare the current pixel's nearest seed with its neighbors
        if (GetNearestSeedIndex(new Vector2(left, y)) != nearestSeedIndex ||
            GetNearestSeedIndex(new Vector2(right, y)) != nearestSeedIndex ||
            GetNearestSeedIndex(new Vector2(x, up)) != nearestSeedIndex ||
            GetNearestSeedIndex(new Vector2(x, down)) != nearestSeedIndex) {
            return true; // Pixel is on the boundary
        }

        return false; // Not a boundary pixel
    }
}