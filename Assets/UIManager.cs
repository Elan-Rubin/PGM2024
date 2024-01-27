using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image powerCircle;
    public Image PowerCircle { get { return powerCircle; } }

    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }
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

    public void UpdatePower(float value)
    {
        powerCircle.fillAmount = value;
    }
}
