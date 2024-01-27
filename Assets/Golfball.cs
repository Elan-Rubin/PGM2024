using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golfball : MonoBehaviour
{
    private Golfer cachedGolfer;
    private Golfhole cachedGolfhole;
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] LineRenderer lr;

    [Header("Attributes")]
    [SerializeField] private float maxPower = 10f;
    [SerializeField] private float power = 2f;
    [SerializeField] private float maxGoalSpeed = 4f;

    private bool isDragging;
    private bool inHole;

    void Start()
    {
        
    }

    void Update()
    {
        PlayerInput();   
    }

    private void PlayerInput()
    {
        Vector2 inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(transform.position, inputPos);

        if (UnityEngine.Input.GetMouseButtonDown(0) && distance <= 0.5f) DragStart();
        if (Input.GetMouseButton(0) && isDragging) DragChange();
        //CHANGE THIS DRAGRELEASE
        if (Input.GetMouseButtonUp(0) && isDragging) DragRelease(Vector2.zero);
    }

    private void DragStart() => isDragging = true;
    private void DragChange() { }
    private void DragRelease(Vector2 pos)
    {
        float distance = Vector2.Distance((Vector2)transform.position, pos);
        isDragging = false;
        if(distance < 1f)
        {
            return;
        }
        Vector2 dir = (Vector2)transform.position - pos;
        rb.velocity = Vector2.ClampMagnitude(dir * power, maxPower);
    }

    public void InitializeGolfball()
    {
        cachedGolfer = Golfer.Instance;
        cachedGolfhole = GameManager.Instance.GetCurrentLevel().Golfhole;
    }
}
