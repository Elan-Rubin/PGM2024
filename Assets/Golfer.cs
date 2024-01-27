using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golfer : MonoBehaviour
{
    private static Golfer instance;
    public static Golfer Instance { get { return instance; } }
    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void InitializeGolfer()
    {

    }
}
