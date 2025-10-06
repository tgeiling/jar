using UnityEngine;

public class JarFiller : MonoBehaviour
{
    [Header("Liquid Settings")]
    [SerializeField] private Transform liquidObject; // Drag your liquid cylinder here
    [SerializeField] private float fillAmount = 0.1f; // How much to fill each tap
    [SerializeField] private float maxFillHeight = 2f; // Maximum height the liquid can reach
    [SerializeField] private float fillSpeed = 1f; // How fast the liquid rises
    
    private float currentFillHeight = 0.1f; // Start with a tiny bit of liquid visible
    private float targetFillHeight = 0.1f;
    private Vector3 initialScale;
    private Vector3 initialPosition;

    void Start()
    {
        if (liquidObject == null)
        {
            Debug.LogError("Please assign the liquid object in the inspector!");
            return;
        }

        // Store initial scale and position
        initialScale = liquidObject.localScale;
        initialPosition = liquidObject.localPosition;
        
        // Set initial liquid height
        UpdateLiquidVisual();
    }

    void Update()
    {
        // Check for touch input (mobile) or mouse click (testing in editor)
        bool inputDetected = false;

        // Mobile touch
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            inputDetected = true;
        }
        // Mouse click for testing in Unity Editor
        else if (Input.GetMouseButtonDown(0))
        {
            inputDetected = true;
        }

        if (inputDetected)
        {
            FillJar();
        }

        // Smoothly animate the liquid rising
        if (currentFillHeight < targetFillHeight)
        {
            currentFillHeight = Mathf.MoveTowards(currentFillHeight, targetFillHeight, fillSpeed * Time.deltaTime);
            UpdateLiquidVisual();
        }
    }

    void FillJar()
    {
        // Increase the target fill height
        targetFillHeight += fillAmount;
        
        // Clamp to max height
        targetFillHeight = Mathf.Min(targetFillHeight, maxFillHeight);
        
        // Optional: Add feedback
        if (targetFillHeight >= maxFillHeight)
        {
            Debug.Log("Jar is full!");
        }
    }

    void UpdateLiquidVisual()
    {
        // Scale the liquid on Y axis to show filling
        Vector3 newScale = initialScale;
        newScale.y = currentFillHeight;
        liquidObject.localScale = newScale;

        // Adjust position so liquid rises from the bottom
        Vector3 newPosition = initialPosition;
        newPosition.y = initialPosition.y + (currentFillHeight - initialScale.y) / 2f;
        liquidObject.localPosition = newPosition;
    }

    // Optional: Method to empty the jar
    public void EmptyJar()
    {
        targetFillHeight = 0.1f;
        currentFillHeight = 0.1f;
        UpdateLiquidVisual();
    }

    // Optional: Get fill percentage (useful for UI)
    public float GetFillPercentage()
    {
        return (currentFillHeight / maxFillHeight) * 100f;
    }
}