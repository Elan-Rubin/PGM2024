using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golfer : MonoBehaviour
{
    private static Golfer instance;
    public static Golfer Instance { get { return instance; } }

    private Golfhole cachedGolfHole;
    private Golfball cachedGolfBall;

    private Vector2 positionBeforeRotating;
    private Transform rotatorChild;
    private Transform rotatorParent;
    private Vector2 cachedOffset;

    bool rotating = false;

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
        if (!rotating) return;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        rotatorParent.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 180));
    }

    public void InitializeGolfer(Golfhole gh, Golfball gb)
    {
        cachedGolfHole = gh;
        cachedGolfBall = gb;
        TeleportToBall();
    }
    public void TeleportToBall() => MoveToBall(true);
    public void MoveToBall() => MoveToBall(false);
    public void MoveToBall(bool teleport)
    {
        var pos = transform.position;
        var ballPos = cachedGolfBall.transform.position;
        var offset = 0.75f * (ballPos.x > pos.x ? Vector2.left : Vector2.right);
        cachedOffset = offset;
        if (teleport) transform.position = (Vector2)ballPos + offset;
    }

    public void StartRotating()
    {
        if (rotating) return;

        rotating = true;
        positionBeforeRotating = transform.position;
        var parent = cachedGolfBall.transform.GetChild(1);
        rotatorParent = parent;
        transform.parent = parent;

        var child = parent.GetChild(0);
        child.localPosition = cachedOffset;
        rotatorChild = child;
    }

    public void StopRotating()
    {
        rotating = false;
        transform.parent = null;
        transform.position = positionBeforeRotating;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
