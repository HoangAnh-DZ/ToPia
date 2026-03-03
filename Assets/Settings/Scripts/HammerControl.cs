using UnityEngine;

public class HammerControl : MonoBehaviour
{
    [Header("Cấu hình xoay")]
    public float rotationSmoothing = 25f;
    public float dragKhiThaChuot = 30f;
    public float dragKhiNhanChuot = 10f;

    [Header("Xử lý Rung lắc")]
    public float deadzone = 0.5f;

    private Rigidbody2D rb;
    private Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        rb.centerOfMass = new Vector2(0, -0.5f);
    }

    void FixedUpdate()
    {
        // NẾU CHƯA CHO PHÉP DI CHUYỂN THÌ DỪNG BÚA LẠI
        if (!GameIntro.canMove)
        {
            rb.angularVelocity = 0;
            rb.velocity = Vector2.zero;
            return;
        }

        if (Input.GetMouseButton(0))
        {
            RotateTowardsMouse();
            rb.angularDrag = dragKhiNhanChuot;
        }
        else
        {
            rb.angularDrag = dragKhiThaChuot;
            rb.angularVelocity = Mathf.Lerp(rb.angularVelocity, 0, Time.fixedDeltaTime * 5f);
        }
    }

    void RotateTowardsMouse()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        Vector2 lookDirection = (Vector2)mouseWorldPos - rb.position;
        float distance = lookDirection.magnitude;

        if (distance > deadzone)
        {
            float targetAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            float smoothedAngle = Mathf.LerpAngle(rb.rotation, targetAngle, rotationSmoothing * Time.fixedDeltaTime);
            rb.MoveRotation(smoothedAngle);
        }
        else
        {
            rb.angularVelocity = 0f;
        }
    }
}