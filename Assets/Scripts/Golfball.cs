using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Golfball : MonoBehaviour
{
    private Golfer cachedGolfer;
    private Golfhole cachedGolfhole;
    private Vector2 golfHolePosition;
    private Vector2 golferPosition; //this will still need to update
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] LineRenderer lr;
    [SerializeField] private Material whiteMat;

    [Header("Attributes")]
    [SerializeField] private float maxPower = 10f;
    [SerializeField] private float power = 2f;
    [SerializeField] private float maxGoalSpeed = 4f;

    private bool isReady;
    private bool isDragging;
    private bool inHole;

    private int hitCounter;

    private float normalDrag;
    Material normalMat;

    void Start()
    {
        normalDrag = rb.drag;
        normalMat = transform.GetChild(0).GetComponent<SpriteRenderer>().material;

    }

    void Update()
    {
        PlayerInput();

        /*var dist = Vector2.Distance(transform.position, golfHolePosition);
        if (dist < 2f && rb.velocity.magnitude > 1f)
        {
            transform.position = Vector2.Lerp(transform.position, golfHolePosition, Time.deltaTime * 1f * (dist / 2f) * (rb.velocity.magnitude / 1f));
        }*/

        if(Vector2.Distance(transform.position, golfHolePosition) > 1)
        {
            var v = rb.velocity.magnitude;
            if (v < 0.4f)
                rb.velocity = Vector2.zero;
            else if (v < 2f)
            {
                rb.drag += Time.deltaTime * 2f;
            }
        }
        
        if (inHole)
        {
            transform.position = Vector2.Lerp(transform.position, golfHolePosition, Time.deltaTime * 10f);
        }
    }

    private bool IsReady()
    {
        if (rb.velocity.magnitude < 0.2f)
        {
            if (!isReady && hitCounter > 0)
            {
                if(inHole) SoundManager.Instance.PlaySoundEffect("golfHole");
                Golfer.Instance.MoveToBall();
                CameraManager.Instance.CenterCamera();
                
            }
            isReady = true;
            return true;
        }
        return false;
    }

    private void PlayerInput()
    {
        if (!IsReady()) return;

        Vector2 inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(transform.position, inputPos);

        if (Input.GetMouseButtonDown(0) && distance <= 0.5f) DragStart();
        if (Input.GetMouseButton(0) && isDragging) DragChange(inputPos);
        if (Input.GetMouseButtonUp(0) && isDragging) DragRelease(inputPos);
    }

    private void DragStart()
    {
        if (inHole) return;

        Golfer.Instance.StartRotating();

        rb.drag = normalDrag;

        isDragging = true;
        lr.positionCount = 2;

    }
    private void DragChange(Vector2 pos)
    {
        if (inHole) return;

        Vector2 dir = (Vector2)transform.position - pos;
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, (Vector2)transform.position + Vector2.ClampMagnitude(dir * power / 2, maxPower / 2));
        
        var pow = Vector2.ClampMagnitude(dir * power / 2, maxPower / 2).magnitude;
        var pow2 = pow * 2 / maxPower;
        UIManager.Instance.UpdatePower(pow2);

        Golfer.Instance.ChargeAnimation(pow2);

        var cam = CameraManager.Instance;
        cam.UpdateSize(pow2);
    }
    private void DragRelease(Vector2 pos)
    {
        UIManager.Instance.UpdateStroke(++hitCounter);
        UIManager.Instance.UpdatePower(0);
        UIManager.Instance.BounceGolfer();

        Golfer.Instance.ReleaseAnimation();

        SoundManager.Instance.PlaySoundEffect("golfHit");

        CameraManager.Instance.ResetSize();

        isReady = false;    

        Golfer.Instance.StopRotating();

        float distance = Vector2.Distance((Vector2)transform.position, pos);
        isDragging = false;
        lr.positionCount = 0;
        if (distance < 1f)
        {
            return;
        }
        Vector2 dir = (Vector2)transform.position - pos;
        rb.velocity = Vector2.ClampMagnitude(dir * power, maxPower);
    }

    public void InitializeGolfball(Golfhole gh)
    {
        cachedGolfer = Golfer.Instance;
        cachedGolfhole = gh;
        golfHolePosition = gh.transform.position;
    }

    private void CheckWinState()
    {
        if (inHole) return;

        if (rb.velocity.magnitude <= maxGoalSpeed)
        {
            inHole = true;
            rb.velocity = Vector2.zero;
            //gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SoundManager.Instance.PlaySoundEffect("golfBall");
        transform.GetChild(0).GetComponent<SpriteRenderer>().material = whiteMat;
        //optimize later
        transform.DOPunchScale(Vector2.one * 0.15f, 0.15f).OnComplete(() =>
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().material = normalMat;
        });
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //optimize later
        transform.DOPunchScale(Vector2.one * -0.15f, 0.15f);
        if (collision.tag.Equals("Golfhole")) CheckWinState();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //transform.DOPunchScale(Vector2.one * -0.15f, 0.15f);
        if (collision.tag.Equals("Golfhole")) CheckWinState();
    }
}
