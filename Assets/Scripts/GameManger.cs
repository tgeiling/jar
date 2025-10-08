using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton so other scripts can access it easily
    
    [Header("Currency")]
    [SerializeField] private int coins = 0;
    [SerializeField] private int coinsPerFill = 500;
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI warningText; // New warning text
    
    void Awake()
    {
        // Singleton pattern - ensures only one GameManager exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        UpdateCoinUI();
        HideWarning(); // Hide warning at start
    }
    
    // Call this when jar gets filled
    public void OnJarFilled()
    {
        AddCoins(coinsPerFill);
        Debug.Log("Jar filled! Earned " + coinsPerFill + " coins");
        ShowWarning(); // Show warning when full
    }
    
    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateCoinUI();
    }
    
    public void SpendCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            UpdateCoinUI();
            return;
        }
        Debug.Log("Not enough coins!");
    }
    
    public int GetCoins()
    {
        return coins;
    }
    
    void UpdateCoinUI()
    {
        if (coinText != null)
        {
            coinText.text = "Coins: " + coins;
        }
    }
    
    public void ShowWarning()
    {
        if (warningText != null)
        {
            warningText.text = "Warning: Glass full!";
            warningText.gameObject.SetActive(true);
        }
    }
    
    public void HideWarning()
    {
        if (warningText != null)
        {
            warningText.gameObject.SetActive(false);
        }
    }
}