using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;
    public Rigidbody2D targetRigidbody; // opcional, ayuda para el look-ahead

    [Header("Smoothing")]
    public float smoothTime = 0.12f; // tiempo para SmoothDamp
    private Vector3 smoothVelocity = Vector3.zero;

    [Header("Look Ahead")]
    public float lookAheadDistance = 2f; // distancia máxima en unidades mundo
    public float lookAheadReturnSpeed = 4f; // velocidad al volver a 0
    public float lookAheadMoveThreshold = 0.05f; // umbral mínimo de movimiento para activar look-ahead
    private float currentLookAheadX = 0f;
    private Vector3 lastTargetPosition;

    [Header("Bounds (opcional)")]
    public BoxCollider2D boundsBox; // si se asigna, usa sus bounds
    public Vector2 minBounds = new Vector2(-10f, -5f);
    public Vector2 maxBounds = new Vector2(10f, 5f);

    private Camera cam;
    private float halfHeight;
    private float halfWidth;

    void Start()
    {
        cam = GetComponent<Camera>();
        if (!cam.orthographic)
        {
            Debug.LogWarning("CameraFollow diseñado para cámara ortográfica. Ajusta la cámara a Orthographic.");
        }
        lastTargetPosition = target ? target.position : Vector3.zero;
        UpdateBoundsFromBox();
        RecalculateHalfExtents();
    }

    void OnValidate()
    {
        // mantener las extensiones actualizadas en edicion
        if (cam == null) cam = GetComponent<Camera>();
        RecalculateHalfExtents();
        UpdateBoundsFromBox();
    }

    private void UpdateBoundsFromBox()
    {
        if (boundsBox != null)
        {
            Bounds b = boundsBox.bounds;
            minBounds = b.min;
            maxBounds = b.max;
        }
    }

    private void RecalculateHalfExtents()
    {
        if (cam == null) cam = GetComponent<Camera>();
        halfHeight = cam.orthographicSize;
        halfWidth = halfHeight * cam.aspect;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // recalcula extents si cambia tamaño de ventana o cámara
        RecalculateHalfExtents();

        // Calcular look-ahead (basado en velocidad o en delta de posición)
        float xMoveDelta;
        if (targetRigidbody != null)
        {
            xMoveDelta = targetRigidbody.linearVelocity.x * Time.deltaTime;
        }
        else
        {
            xMoveDelta = (target.position - lastTargetPosition).x;
        }

        if (Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold)
        {
            currentLookAheadX = Mathf.Sign(xMoveDelta) * lookAheadDistance;
        }
        else
        {
            currentLookAheadX = Mathf.MoveTowards(currentLookAheadX, 0f, lookAheadReturnSpeed * Time.deltaTime);
        }

        Vector3 aheadTargetPos = target.position + new Vector3(currentLookAheadX, 0f, 0f);

        // Aplicar smooth damp
        Vector3 newPos = Vector3.SmoothDamp(transform.position, new Vector3(aheadTargetPos.x, aheadTargetPos.y, transform.position.z), ref smoothVelocity, smoothTime);

        // Clamp con bounds para que no se vea skybox
        float clampX = newPos.x;
        float clampY = newPos.y;

        float minX = minBounds.x + halfWidth;
        float maxX = maxBounds.x - halfWidth;
        float minY = minBounds.y + halfHeight;
        float maxY = maxBounds.y - halfHeight;

        // Si el mundo es más pequeño que la vista, centrar en el bounds
        if (minX > maxX) clampX = (minBounds.x + maxBounds.x) * 0.5f;
        else clampX = Mathf.Clamp(newPos.x, minX, maxX);

        if (minY > maxY) clampY = (minBounds.y + maxBounds.y) * 0.5f;
        else clampY = Mathf.Clamp(newPos.y, minY, maxY);

        transform.position = new Vector3(clampX, clampY, transform.position.z);

        lastTargetPosition = target.position;
    }
}