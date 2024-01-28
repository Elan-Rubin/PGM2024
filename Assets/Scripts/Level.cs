using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private LevelElement element = LevelElement.Wind;
    public LevelElement Element { get { return element; } }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
public enum LevelElement
{
    Wind,
    Beeping,
    Exploding,
    StrongWind,
    Wizard,
    Streaker,
}
