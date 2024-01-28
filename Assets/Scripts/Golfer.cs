using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UIElements;

public class Golfer : MonoBehaviour
{
   
    private Golfhole cachedGolfHole;
    private Golfball cachedGolfBall;

    private Vector2 positionBeforeRotating;
    private Transform rotatorChild;
    private Transform rotatorParent;
    private Vector2 cachedOffset;
    private SpriteRenderer golferSR;

    bool rotating = false;

    private int animationFrame;
    private int selectedAnimation;
    [SerializeField] private List<AnimationFrames> animations = new();

    private static Golfer instance;
    public static Golfer Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
    }
    void Start()
    {
        golferSR = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        golferSR.sprite = animations[selectedAnimation].Frames[animationFrame];

        golferSR.flipY = false;
        if (!rotating) return; //this is bad code
        golferSR.flipY = transform.position.x > cachedGolfBall.transform.position.x && !golferSR.flipX;

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);

        var dist = Vector2.Distance(mousePos, objectPos);
        if (dist < 3f) return; //this might be the line of interest

        mousePos.x -= objectPos.x;
        mousePos.y -= objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        var newquat = Quaternion.Euler(new Vector3(0, 0, angle + 180));

        //what if its between 2 and 4??
        float mult = 1;
        if (dist < 2f && dist > 4)
        {
            mult = .5f * (dist - 2);
        }
        rotatorParent.rotation = Quaternion.Lerp(rotatorParent.rotation, newquat, Time.deltaTime * 12f * mult);

    }

    public void ChargeAnimation(float value)
    {
        //value is from 0-1
        if (value > 0.15f) animationFrame = 1;
        if (value > 0.3f) animationFrame = 2;
        else animationFrame = 0;
        //yes, i know
    }

    public void ReleaseAnimation()
    {
        StartCoroutine(nameof(ReleaseAnimationCoroutine));
    }

    private IEnumerator ReleaseAnimationCoroutine()
    {
        animationFrame = 3;
        yield return new WaitForSeconds(0.1f);
        animationFrame = 4;
        yield return new WaitForSeconds(0.1f);
        animationFrame = 5;
        /*yield return new WaitForSeconds(0.1f);
        animationFrame = 0;*/
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
        var offset = 0.75f * (cachedGolfHole.transform.position.x > ballPos.x ? new Vector2(-1.83f, 0.74f) : new Vector2(1.83f, 0.74f));
        cachedOffset = offset;
        Debug.Log(offset);
        if (teleport) transform.position = (Vector2)ballPos + offset;
        else
        {
            transform.DOMove((Vector2)ballPos + offset, Vector2.Distance(ballPos, pos) / 7f).OnComplete(() =>
            {
                animationFrame = 0;
                golferSR.flipX = ballPos.x < transform.position.x;
            });
        }
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
        //maybe wait a second first??
        rotating = false;
        transform.parent = null;
        //transform.position = positionBeforeRotating;
        transform.DOMove(positionBeforeRotating, 0.25f);
        //transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.DORotate(new Vector3(0,0,0), 0.25f);
    }
    [System.Serializable]
    public struct AnimationFrames
    {
        public List<Sprite> Frames;
    }
}
