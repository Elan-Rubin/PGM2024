using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image powerCircle;
    private float currentFill, targetFill;
    [SerializeField] private TextMeshProUGUI strokeText;
    private static UIManager instance;
    public static UIManager Instance { get { return instance; } }
    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
    }

    void Start()
    {
        currentFill = targetFill = 0;
    }

    void Update()
    {
        currentFill = powerCircle.fillAmount = Mathf.Lerp(currentFill, targetFill, Time.deltaTime * 15f);
    }

    public void UpdatePower(float value)
    {
        Debug.Log($"updatepower, {value}");
        //this is expensive
        targetFill = value;
    }

    public void UpdateStroke(int value)
    {
        strokeText.text = $"Stroke: {value:00}";
        strokeText.transform.DOPunchScale(Vector2.one * 0.1f, 0.1f);
    }
}
