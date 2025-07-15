using UnityEngine;

public class Pawn : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 originalScale;
    private Vector3 targetScale;

    [SerializeField] private float scaleMultiplier = 1.2f;
    [SerializeField] private float scaleSpeed = 10f;
    [SerializeField] private float lagSpeed = 5f;
    [SerializeField] private float maxTiltAngle = 15f;

    private Vector3 desiredTopPosition;    // where the mouse is
    private Vector3 centerPosition;        // where the center currently is

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
        centerPosition = transform.position;
        desiredTopPosition = GetTopPosition();
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = transform.position.z;

            // top of object is fixed to mouse
            desiredTopPosition = mousePos;
        }

        Vector3 halfHeightOffset = new Vector3(0, GetHalfHeight(), 0);

        // desired center position to keep top fixed
        Vector3 desiredCenterPosition = desiredTopPosition - halfHeightOffset;

        // smooth lagging of the center *only*
        centerPosition = Vector3.Lerp(centerPosition, desiredCenterPosition, Time.deltaTime * lagSpeed);

        transform.position = centerPosition;

        // Optional tilt effect based on lag
        Vector3 lagDirection = desiredCenterPosition - centerPosition;
        float angle = Mathf.Clamp(-lagDirection.x * 10f, -maxTiltAngle, maxTiltAngle);
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Smooth scale
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * scaleSpeed);
    }

    void OnMouseDown()
    {
        isDragging = true;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = transform.position.z;

        desiredTopPosition = mousePos;

        targetScale = originalScale * scaleMultiplier;
    }

    void OnMouseUp()
    {
        isDragging = false;

        targetScale = originalScale;

        // reset rotation
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
        return transform.position + new Vector3(0, GetHalfHeight(), 0);
    }
}