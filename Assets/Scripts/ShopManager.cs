using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    [System.Serializable]
    public class ShopItem
    {
        public string itemName;
        public string itemType; // "jar" or "figurine"
        public int cost;
        public Sprite itemSprite;
        public bool isPurchased = false;
        public GameObject prefab; // The actual jar or figurine prefab
    }

    [Header("Shop Settings")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject shopItemPrefab; // UI prefab for shop items
    [SerializeField] private Transform shopItemContainer; // Grid layout group container
    [SerializeField] private List<ShopItem> shopItems = new List<ShopItem>();

    [Header("Item Details Panel")]
    [SerializeField] private GameObject detailsPanel;
    [SerializeField] private TextMeshProUGUI detailsNameText;
    [SerializeField] private TextMeshProUGUI detailsCostText;
    [SerializeField] private Image detailsImage;
    [SerializeField] private Button purchaseButton;
    [SerializeField] private TextMeshProUGUI purchaseButtonText;

    private ShopItem selectedItem;

    void Start()
    {
        // Hide shop at start
        shopPanel.SetActive(false);
        detailsPanel.SetActive(false);

        // Setup purchase button
        if (purchaseButton != null)
        {
            purchaseButton.onClick.AddListener(PurchaseSelectedItem);
        }

        // Populate shop with items
        PopulateShop();
    }

    public void OpenShop()
    {
        shopPanel.SetActive(true);
        RefreshShop();
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        detailsPanel.SetActive(false);
    }

    void PopulateShop()
    {
        // Clear existing items
        foreach (Transform child in shopItemContainer)
        {
            Destroy(child.gameObject);
        }

        // Create UI elements for each shop item
        foreach (ShopItem item in shopItems)
        {
            GameObject itemUI = Instantiate(shopItemPrefab, shopItemContainer);
            
            // Setup the item UI
            Image itemImage = itemUI.transform.Find("ItemImage").GetComponent<Image>();
            TextMeshProUGUI itemNameText = itemUI.transform.Find("ItemName").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI itemCostText = itemUI.transform.Find("ItemCost").GetComponent<TextMeshProUGUI>();
            GameObject purchasedOverlay = itemUI.transform.Find("PurchasedOverlay").gameObject;

            if (itemImage != null) itemImage.sprite = item.itemSprite;
            if (itemNameText != null) itemNameText.text = item.itemName;
            if (itemCostText != null) itemCostText.text = item.cost + " coins";
            if (purchasedOverlay != null) purchasedOverlay.SetActive(item.isPurchased);

            // Add button functionality
            Button itemButton = itemUI.GetComponent<Button>();
            ShopItem currentItem = item; // Capture for closure
            itemButton.onClick.AddListener(() => SelectItem(currentItem));
        }
    }

    void RefreshShop()
    {
        PopulateShop();
    }

    void SelectItem(ShopItem item)
    {
        selectedItem = item;
        detailsPanel.SetActive(true);

        // Update details panel
        if (detailsNameText != null) detailsNameText.text = item.itemName;
        if (detailsCostText != null) detailsCostText.text = "Cost: " + item.cost + " coins";
        if (detailsImage != null) detailsImage.sprite = item.itemSprite;

        // Update purchase button
        if (item.isPurchased)
        {
            purchaseButtonText.text = "OWNED";
            purchaseButton.interactable = false;
        }
        else if (GameManager.Instance.GetCoins() >= item.cost)
        {
            purchaseButtonText.text = "BUY";
            purchaseButton.interactable = true;
        }
        else
        {
            purchaseButtonText.text = "NOT ENOUGH COINS";
            purchaseButton.interactable = false;
        }
    }

    void PurchaseSelectedItem()
    {
        if (selectedItem == null || selectedItem.isPurchased) return;

        if (GameManager.Instance.GetCoins() >= selectedItem.cost)
        {
            // Deduct coins
            GameManager.Instance.SpendCoins(selectedItem.cost);
            
            // Mark as purchased
            selectedItem.isPurchased = true;
            
            Debug.Log("Purchased: " + selectedItem.itemName);
            
            // Refresh UI
            RefreshShop();
            SelectItem(selectedItem); // Update details panel
            
            // TODO: Apply the item (swap jar or spawn figurine)
            ApplyPurchasedItem(selectedItem);
        }
    }

    void ApplyPurchasedItem(ShopItem item)
    {
        if (item.itemType == "jar")
        {
            // TODO: Replace current jar with new jar prefab
            Debug.Log("Equipped new jar: " + item.itemName);
        }
        else if (item.itemType == "figurine")
        {
            // TODO: Spawn figurine inside jar
            Debug.Log("Added figurine: " + item.itemName);
        }
    }
}