using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Camera cam;
    private float defaultSize;

    private static CameraManager instance;
    public static CameraManager Instance { get { return instance; } }
    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
    }
    void Start()
    {
        cam = GetComponent<Camera>();
        defaultSize = cam.orthographicSize;
    }

    void Update()
    {
        
    }

    public void ResetSize() => UpdateSize(defaultSize);
    public void UpdateSize(float value)
    {
        cam.orthographicSize = value;
    }
}
