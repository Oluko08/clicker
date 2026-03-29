using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class YUBOGItemUI : MonoBehaviour
{
    private TrendItemData itemData;
    private SystemZamsu scoreSystem;
    private GameObject mainViewObject; // 매니저가 할당해줌

    [Header("🖼️ UI 요소")]
    public Image connectionBar;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public Image iconImage;
    public Button buyButton;

    private int level = 0;
    private bool isPurchased = false;

    // 초기화 시 캐릭터 오브젝트를 직접 전달받음
    public void Setup(TrendItemData data, GameObject characterObj)
    {
        itemData = data;
        mainViewObject = characterObj;
        scoreSystem = Object.FindAnyObjectByType<SystemZamsu>();

        if (mainViewObject != null && itemData != null)
        {
            var animator = mainViewObject.GetComponent<SimpleSpriteAnimator>();
            if (animator == null) animator = mainViewObject.AddComponent<SimpleSpriteAnimator>();
            animator.SetAnimationData(itemData.characterSprite1, itemData.characterSprite2, itemData.animationInterval);
        }

        LoadData();
        UpdateUI();
        if (isPurchased && mainViewObject != null) mainViewObject.SetActive(true);
    }

    private void Update()
    {
        if (scoreSystem == null || buyButton == null) return;
        buyButton.interactable = (scoreSystem.popularity >= GetCurrentCost());
    }

    public void OnBuyButtonClick()
    {
        double currentCost = GetCurrentCost();
        if (scoreSystem.popularity >= currentCost)
        {
            scoreSystem.popularity -= currentCost;
            if (!isPurchased)
            {
                isPurchased = true;
                if (mainViewObject != null) mainViewObject.SetActive(true);
            }
            level++;
            scoreSystem.clickPower += itemData.clickPowerAdd;
            scoreSystem.pps += itemData.perSecondAdd;
            scoreSystem.UpdateUI();
            UpdateUI();
            SaveData();
        }
    }

    public double GetCurrentCost() => !isPurchased ? itemData.price : itemData.price * System.Math.Pow(1.25f, level);

    public void UpdateUI()
    {
        if (nameText != null) nameText.text = isPurchased ? $"{itemData.itemName} (Lv.{level})" : itemData.itemName;
        if (priceText != null && scoreSystem != null) priceText.text = scoreSystem.GetKoreanUnit(GetCurrentCost());
        if (iconImage != null) iconImage.sprite = itemData.itemIcon;
        if (connectionBar != null) connectionBar.color = isPurchased ? new Color(1f, 0.5f, 0f) : new Color(0.3f, 0.3f, 0.3f);
    }

    private void SaveData() { PlayerPrefs.SetInt(itemData.itemName + "_Level", level); PlayerPrefs.SetInt(itemData.itemName + "_Purchased", isPurchased ? 1 : 0); }
    private void LoadData() { level = PlayerPrefs.GetInt(itemData.itemName + "_Level", 0); isPurchased = PlayerPrefs.GetInt(itemData.itemName + "_Purchased", 0) == 1; }
}