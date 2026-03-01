using UnityEngine;

public class HammerControl : MonoBehaviour
{
    [Header("Cấu hình xoay")]
    public float rotationSmoothing = 25f; // Tăng một chút cho mượt
    public float dragKhiThaChuot = 30f;
    public float dragKhiNhanChuot = 10f;

    [Header("Xử lý Rung lắc")]
    public float deadzone = 0.5f; // Khoảng cách an toàn để búa không bị giật

    private Rigidbody2D rb;
    private Camera mainCamera;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        // Trọng tâm búa (Nếu vẫn rung hãy thử comment dòng này lại)
        rb.centerOfMass = new Vector2(0, -0.5f);
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            RotateTowardsMouse();
            rb.angularDrag = dragKhiNhanChuot;
        }
        else
        {
            rb.angularDrag = dragKhiThaChuot;
            // Khi không bấm chuột, triệt tiêu lực xoay dư thừa
            rb.angularVelocity = Mathf.Lerp(rb.angularVelocity, 0, Time.fixedDeltaTime * 5f);
        }
    }

    void RotateTowardsMouse()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        // Tính toán vector từ búa tới chuột
        Vector2 lookDirection = (Vector2)mouseWorldPos - rb.position;
        float distance = lookDirection.magnitude;

        // CHỐNG RUNG: Chỉ xoay khi chuột đủ xa tâm búa
        if (distance > deadzone)
        {
            float targetAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            float smoothedAngle = Mathf.LerpAngle(rb.rotation, targetAngle, rotationSmoothing * Time.fixedDeltaTime);

            rb.MoveRotation(smoothedAngle);
        }
        else
        {
            // CHỐNG XOAY TRÒN: Cưỡng chế búa đứng im khi chuột quá gần
            rb.angularVelocity = 0f;
        }
    }
}