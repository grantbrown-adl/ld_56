using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadScript : MonoBehaviour
{
    [SerializeField] private bool destroyOnLoad;

    private void Awake()
    {
        if(!destroyOnLoad)
            DontDestroyOnLoad(this.gameObject);
    }
}
