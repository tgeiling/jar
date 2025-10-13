using UnityEngine;

public class IsometricCameraController : MonoBehaviour
{
    [Header("Target")]
    public Transform jar; // Assign your jar GameObject here
    
    [Header("Camera Settings")]
    public float orbitSpeed = 100f;
    public float zoomSpeed = 10f;
    public float minDistance = 3f;
    public float maxDistance = 15f;
    
    [Header("Angle Constraints")]
    public float minAngle = 30f; // Minimum angle from horizontal
    public float maxAngle = 70f; // Maximum angle from horizontal
    public float minHorizontalAngle = -45f; // Left wall limit
    public float maxHorizontalAngle = 45f; // Right wall limit
    
    private float currentDistance;
    private float currentVerticalAngle;
    private float currentHorizontalAngle;
    
    void Start()
    {
        if (jar == null)
        {
            Debug.LogError("Jar target not assigned!");
            return;
        }
        
        Debug.Log("Jar position: " + jar.position);
        
        // Set default viewing position
        currentDistance = (minDistance + maxDistance) / 2f;
        currentVerticalAngle = 30f;
        currentHorizontalAngle = 0f;
        
        // Immediately position camera correctly
        UpdateCameraPosition();
    }
    
    void LateUpdate()
    {
        if (jar == null) return;
        
        HandleInput();
        UpdateCameraPosition();
    }
    
    void HandleInput()
    {
        // Mobile touch input
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Moved)
            {
                float h = touch.deltaPosition.x * orbitSpeed * 0.1f * Time.deltaTime;
                float v = -touch.deltaPosition.y * orbitSpeed * 0.1f * Time.deltaTime;
                
                currentHorizontalAngle += h;
                currentVerticalAngle += v;
                
                // Clamp angles to prevent going behind walls
                currentHorizontalAngle = Mathf.Clamp(currentHorizontalAngle, minHorizontalAngle, maxHorizontalAngle);
                currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, minAngle, maxAngle);
            }
        }
        // Pinch to zoom on mobile
        else if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);
            
            Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            
            float prevMagnitude = (touch0PrevPos - touch1PrevPos).magnitude;
            float currentMagnitude = (touch0.position - touch1.position).magnitude;
            
            float difference = prevMagnitude - currentMagnitude;
            
            currentDistance += difference * zoomSpeed * 0.01f;
            currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
        }
        
        // PC debug controls - right mouse button to rotate
        if (Input.GetMouseButton(1))
        {
            float h = Input.GetAxis("Mouse X") * orbitSpeed * Time.deltaTime;
            float v = -Input.GetAxis("Mouse Y") * orbitSpeed * Time.deltaTime;
            
            currentHorizontalAngle += h;
            currentVerticalAngle += v;
            
            // Clamp angles to prevent going behind walls
            currentHorizontalAngle = Mathf.Clamp(currentHorizontalAngle, minHorizontalAngle, maxHorizontalAngle);
            currentVerticalAngle = Mathf.Clamp(currentVerticalAngle, minAngle, maxAngle);
        }
        
        // PC debug controls - mouse wheel to zoom
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            currentDistance -= scroll * zoomSpeed;
            currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
        }
    }
    
    void UpdateCameraPosition()
    {
        // Calculate new camera position
        float radVertical = currentVerticalAngle * Mathf.Deg2Rad;
        float radHorizontal = currentHorizontalAngle * Mathf.Deg2Rad;
        
        Vector3 offset = new Vector3(
            Mathf.Sin(radHorizontal) * Mathf.Cos(radVertical),
            Mathf.Sin(radVertical),
            Mathf.Cos(radHorizontal) * Mathf.Cos(radVertical)
        ) * currentDistance;
        
        transform.position = jar.position + offset;
        transform.LookAt(jar.position);
    }
}