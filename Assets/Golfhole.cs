using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golfhole : MonoBehaviour
{
    private Golfer cachedGolfer;
    private Golfball cachedGolfball;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void InitializeGolfhole(Golfball gb)
    {
        cachedGolfer = Golfer.Instance;
        cachedGolfball = gb;
    }
}
