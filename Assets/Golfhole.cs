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

    public void InitializeGolfhole()
    {
        cachedGolfer = Golfer.Instance;
        cachedGolfball = GameManager.Instance.GetCurrentLevel().Golfball;
    }
}
