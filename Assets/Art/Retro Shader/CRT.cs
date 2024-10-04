using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CRT : MonoBehaviour {
    public Material baseMat;
    public Shader crtShader;
    public bool useCRTShader;

    [Range(1.0f, 10.0f)]
    public float curvature = 1.0f;

    [Range(1.0f, 500.0f)]
    public float vignetteWidth = 30.0f;

    private Material crtMat;

    void Start() {
        crtMat ??= new Material(crtShader);
        crtMat.hideFlags = HideFlags.HideAndDontSave;
    }

    public void ToggleCRT()
    {
        useCRTShader = !useCRTShader;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
            crtMat.SetFloat("_Curvature", curvature);
            crtMat.SetFloat("_VignetteWidth", vignetteWidth);
        if(useCRTShader)
        {
            Graphics.Blit(source, destination, crtMat);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }

    private void LateUpdate() {
        //if (Input.GetKeyDown(KeyCode.Space)) {
        //    RenderTexture rt = new RenderTexture(1920, 1080, 24);
        //    GetComponent<Camera>().targetTexture = rt;
        //    Texture2D screenshot = new Texture2D(1920, 1080, TextureFormat.RGB24, false);
        //    GetComponent<Camera>().Render();
        //    RenderTexture.active = rt;
        //    screenshot.ReadPixels(new Rect(0, 0, 1920, 1080), 0, 0);
        //    GetComponent<Camera>().targetTexture = null;
        //    RenderTexture.active = null;
        //    Destroy(rt);
        //    string filename = string.Format("{0}/../Recordings/snap_{1}.png", Application.dataPath, System.DateTime.Now.ToString("HH-mm-ss"));
        //    System.IO.File.WriteAllBytes(filename, screenshot.EncodeToPNG());
        //}
    }
}
