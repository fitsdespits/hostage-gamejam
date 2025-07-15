using UnityEngine;

public class Pawn : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 originalScale;
    private Vector3 targetScale;

    [Header("Scaling")]
    [SerializeField] private float scaleMultiplier = 1.2f;
    [SerializeField] private float scaleSpeed = 10f;

    [Header("Lag & Tilt")]
    [SerializeField] private float maxTiltAngle = 15f;
    [SerializeField] private float springStiffness = 40f;
    [SerializeField] private float springDamping = 8f;

    [Header("Lag Limits")]
    [SerializeField] private float maxLagDistance = 10f;
    [SerializeField] private float snapDistance = 3f;

    private Vector3 desiredTopPosition;      // where the mouse is (top, fixed)
    private Vector3 laggedBottomPosition;    // where the bottom is (lags)
    private Vector3 laggedBottomVelocity;    // velocity of the bottom point
    private float halfHeight;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
        halfHeight = GetHalfHeight();

        desiredTopPosition = GetTopPosition();
        laggedBottomPosition = GetBottomPosition(); // start at current bottom
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = transform.position.z;

            // Top of object is fixed to mouse
            desiredTopPosition = mousePos;
        }

        Vector3 desiredBottomPosition = desiredTopPosition - Vector3.up * (halfHeight * 2f);

        // Clamp max lag distance so lagged bottom doesn't fall too far behind
        Vector3 toTarget = desiredBottomPosition - laggedBottomPosition;
        if (toTarget.magnitude > maxLagDistance)
            toTarget = toTarget.normalized * maxLagDistance;

        // Spring physics for bottom
        Vector3 acceleration = toTarget * springStiffness - laggedBottomVelocity * springDamping;
        laggedBottomVelocity += acceleration * Time.deltaTime;
        laggedBottomPosition += laggedBottomVelocity * Time.deltaTime;

        // Hard snap correction if lag is too large
        if ((laggedBottomPosition - desiredBottomPosition).magnitude > snapDistance)
        {
            laggedBottomPosition = Vector3.Lerp(laggedBottomPosition, desiredBottomPosition, 0.5f);
            laggedBottomVelocity = Vector3.zero;
        }

        // Compute the vector from lagged bottom to fixed top
        Vector3 upVector = desiredTopPosition - laggedBottomPosition;
        float angle = Mathf.Atan2(upVector.y, upVector.x) * Mathf.Rad2Deg - 90f;

        // Dynamic max tilt angle reduces tilt if lag is large
        float dynamicMaxTilt = Mathf.Lerp(maxTiltAngle, 5f, toTarget.magnitude / maxLagDistance);
        angle = Mathf.Clamp(angle, -dynamicMaxTilt, dynamicMaxTilt);

        // Apply rotation
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Position the object so its top is at desiredTopPosition
        Vector3 rotatedOffset = transform.rotation * (Vector3.up * halfHeight);
        transform.position = desiredTopPosition - rotatedOffset;

        // Smooth scale
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
    }

    void OnMouseDown()
    {
        isDragging = true;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;

        desiredTopPosition = mousePos;
        laggedBottomPosition = GetBottomPosition();
        laggedBottomVelocity = Vector3.zero;

        targetScale = originalScale * scaleMultiplier;
    }

    void OnMouseUp()
    {
        isDragging = false;

        targetScale = originalScale;

        // Reset rotation
        transform.rotation = Quaternion.identity;
    }

    float GetHalfHeight()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            return sr.bounds.extents.y;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            return col.bounds.extents.y;

        return 0.5f; // fallback
    }

    Vector3 GetTopPosition()
    {
        return transform.position + Vector3.up * halfHeight;
    }

    Vector3 GetBottomPosition()
    {
        return transform.position - Vector3.up * halfHeight;
    }
}