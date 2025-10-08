using UnityEngine;

public class JarFiller : MonoBehaviour
{
    [Header("Liquid Settings")]
    [SerializeField] private Transform liquidObject; // Drag your liquid cylinder here
    [SerializeField] private float fillAmount = 0.1f; // How much to fill each tap
    [SerializeField] private float maxFillHeight = 2f; // Maximum height the liquid can reach
    [SerializeField] private float fillSpeed = 1f; // How fast the liquid rises
    
    [Header("Pour Effect")]
    [SerializeField] private ParticleSystem pourEffect; // Drag your particle system here
    [SerializeField] private float pourDuration = 0.5f; // How long liquid pours
    
    private bool isFull = false; // Track if jar is full
    private bool coinsAwarded = false; // Track if coins were already given
    
    private float currentFillHeight = 0.1f; // Start with a tiny bit of liquid visible
    private float targetFillHeight = 0.1f;
    private float initialScaleY;
    private Vector3 initialPosition;

    void Start()
    {
        if (liquidObject == null)
        {
            Debug.LogError("Please assign the liquid object in the inspector!");
            return;
        }

        // Store initial scale Y and position
        initialScaleY = liquidObject.localScale.y;
        initialPosition = liquidObject.localPosition;
        
        // Set initial liquid height
        UpdateLiquidVisual();
    }

    void Update()
    {
        // Don't allow input if jar is already full
        if (isFull)
        {
            return;
        }
        
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
            Debug.Log("Click detected! Filling jar...");
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
        
        // Trigger pour effect
        if (pourEffect != null)
        {
            pourEffect.Play();
            Invoke("StopPourEffect", pourDuration);
        }
        
        // Check if jar just became full
        if (targetFillHeight >= maxFillHeight && !coinsAwarded)
        {
            isFull = true;
            coinsAwarded = true; // Prevent multiple coin awards
            Debug.Log("Jar is full!");
            
            // Award coins only once
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnJarFilled();
            }
            
            // Don't reset the jar anymore - it stays full
        }
    }
    
    void StopPourEffect()
    {
        if (pourEffect != null)
        {
            pourEffect.Stop();
        }
    }

    void UpdateLiquidVisual()
    {
        // Scale the liquid on Y axis to show filling
        Vector3 newScale = liquidObject.localScale;
        newScale.y = currentFillHeight;
        liquidObject.localScale = newScale;

        // Adjust position so liquid rises from the bottom
        Vector3 newPosition = initialPosition;
        newPosition.y = initialPosition.y + (currentFillHeight - initialScaleY) / 2f;
        liquidObject.localPosition = newPosition;
    }

    // Optional: Method to empty the jar (can be called when you want to restart)
    public void EmptyJar()
    {
        targetFillHeight = 0.1f;
        currentFillHeight = 0.1f;
        isFull = false;
        coinsAwarded = false;
        UpdateLiquidVisual();
        
        // Hide warning when emptying
        if (GameManager.Instance != null)
        {
            GameManager.Instance.HideWarning();
        }
    }

    // Optional: Get fill percentage (useful for UI)
    public float GetFillPercentage()
    {
        return (currentFillHeight / maxFillHeight) * 100f;
    }
}