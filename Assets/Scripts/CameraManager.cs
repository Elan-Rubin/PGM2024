using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Camera cam;
    private float camZ;
    private float defaultSize;
    private float currentSize, targetSize;
    [SerializeField] private float minSize = 3.5f, maxSize = 10;

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
        currentSize = targetSize = defaultSize = cam.orthographicSize;
        camZ = -10;
    }

    void Update()
    {
        currentSize = cam.orthographicSize = Mathf.Lerp(currentSize, targetSize, Time.deltaTime * 2f);
    }

    public void ResetSize() => targetSize = defaultSize;
    //this takes a value from 0 to 1
    public void UpdateSize(float value)
    {
        //value = Mathf.Log(value);
        targetSize = (maxSize - minSize) * value + minSize;
        //targetSize = (value - minSize) / (maxSize - minSize);
    }

    private void LateUpdate()
    {
        var pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y, -10);
    }

    public void CenterCamera()
    {
        var l = GameManager.Instance.GetCurrentLevel();
        var h = l.Golfhole.transform.position;
        var b = l.Golfball.transform.position;
        MoveCamera(Vector2.Lerp(h, b, 0.8f));
        //MoveCamera((h+b)/2f);
    }
    public void MoveCamera(Vector3 pos)
    {
        pos = new Vector3(pos.x, pos.y, camZ);
        //no idea why this line doesnt work
        transform.DOMove(pos, Vector2.Distance(transform.position, pos) / 4f).SetEase(Ease.Linear);
    }
}
