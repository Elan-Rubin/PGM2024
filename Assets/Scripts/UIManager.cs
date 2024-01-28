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
    [SerializeField] private TextMeshProUGUI opponentStrokeText;
    [SerializeField] private Image golferImage;
    [SerializeField] private Image overlayImage;
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
        //this is expensive
        targetFill = value;
    }

    public void UpdateStroke(int value)
    {
        strokeText.text = $"Stroke: {value}";
        strokeText.transform.DOPunchScale(Vector2.one * 0.15f, 0.1f);
    }
    public void UpdateOpponentStroke(int value)
    {
        opponentStrokeText.text = $"Stroke: {value}";
        opponentStrokeText.transform.DOPunchScale(Vector2.one * 0.15f, 0.1f);
    }

    public void BounceGolfer()
    {
        golferImage.transform.DOPunchScale(Vector2.one * 0.15f, 0.1f);
        golferImage.transform.DOShakePosition(0.15f, 5).OnComplete(() =>
        {
            golferImage.transform.localPosition = Vector2.zero;
        });
    }

    public void FadeIn() => StartCoroutine(nameof(FadeInCoroutine));
    private IEnumerator FadeInCoroutine()
    {
        overlayImage.color = Color.black;
        yield return new WaitForSeconds(0.5f);
        overlayImage.DOFade(0, 0.25f);
    }
    public void FadeOut() => StartCoroutine(nameof(FadeOutCoroutine));
    private IEnumerator FadeOutCoroutine()
    {
        overlayImage.color = Color.clear;
        yield return new WaitForSeconds(0.5f);
        overlayImage.DOFade(1, 0.25f);
    }
}
