using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class Golfhole : MonoBehaviour
{
    private Golfer cachedGolfer;
    private Golfball cachedGolfball;
    private Transform golfBallTransform;
    private GameObject flag;
    private SpriteRenderer flagSR;

    private Vector2 currentPos, targetPos;
    private Color currentColor, targetColor;
    void Start()
    {
        flag = transform.GetChild(1).gameObject;
        flagSR = flag.transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        var dist = Vector2.Distance(transform.position, golfBallTransform.position);
        if (dist < 3f)
        {
            targetPos = new Vector2(0, 0.5f);
        }
        else if(flag.transform.localPosition.y > 0.05)
        {
            targetPos = Vector2.zero;
        }
        if (dist < 1f)
        {
            targetColor = new Color(1,1,1,0);
        }
        else targetColor = Color.white;

        //this is expensive, should be optimized later
        currentColor = flagSR.color = Color.Lerp(currentColor, targetColor, Time.deltaTime * 8f);
        currentPos = flag.transform.localPosition = Vector2.Lerp(currentPos, targetPos, Time.deltaTime * 20f);

    }

    public void InitializeGolfhole(Golfball gb)
    {
        cachedGolfer = Golfer.Instance;
        cachedGolfball = gb;
        golfBallTransform = cachedGolfball.transform;
    }
}
