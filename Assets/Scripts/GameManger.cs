using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Currency")]
    [SerializeField] private int coins = 0;
    [SerializeField] private int coinsPerFill = 500;
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI warningText;
    
    [Header("Managers")]
    [SerializeField] private AchievementsManager achievementsManager;
    
    private int jarsFilled = 0; // Track total jars filled
    
    void Awake()
    {
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
        HideWarning();
    }
    
    public void OnJarFilled()
    {
        AddCoins(coinsPerFill);
        jarsFilled++;
        
        Debug.Log("Jar filled! Earned " + coinsPerFill + " coins. Total fills: " + jarsFilled);
        ShowWarning();
        
        // Track achievements
        if (achievementsManager != null)
        {
            achievementsManager.IncrementAchievement("First Fill");
            achievementsManager.IncrementAchievement("Fill Master");
            achievementsManager.IncrementAchievement("Dedication");
        }
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
    
    public int GetJarsFilled()
    {
        return jarsFilled;
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