using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PromotionButtonUI : MonoBehaviour
{
    public PromotionData data; 
    public TextMeshProUGUI countText;   // 보유 개수 텍스트
    public TextMeshProUGUI timerText;   // 버튼 중앙 타이머 텍스트
    public Button actionButton;        

    private SystemZamsu scoreSystem;
    private PromotionManager manager;

    private void Start()
    {
        scoreSystem = FindAnyObjectByType<SystemZamsu>();
        manager = FindAnyObjectByType<PromotionManager>();
        
        if (actionButton != null)
        {
            actionButton.onClick.RemoveAllListeners();
            actionButton.onClick.AddListener(OnButtonClick);
        }

        if (timerText != null) timerText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (scoreSystem != null && actionButton != null && data != null)
        {
            // 1. 아이템 개수 체크
            int currentCount = (data.multiplier <= 11f) ? scoreSystem.item10Count : scoreSystem.item100Count;
            
            // 2. 해당 계열이 홍보 중인지 체크
            bool isSameTypePromoting = (data.type == PromotionData.PromotionType.Click) ? scoreSystem.isClickPromoting : scoreSystem.isPpsPromoting;

            // 아이템이 있고 + 해당 계열 전체가 홍보 중이 아닐 때만 버튼 활성화 (중복 클릭 방지)
            actionButton.interactable = (currentCount > 0 && !isSameTypePromoting);
            
            if (countText != null) countText.text = $"{currentCount}개";
        }
    }

    public void OnButtonClick()
    {
        if (manager != null && data != null)
        {
            // [수정] 매니저에게 '나(this)'를 함께 보냄
            manager.UsePromotion(data, this);
        }
    }

    public void SetTimerActive(bool isActive)
    {
        if (timerText != null) timerText.gameObject.SetActive(isActive);
    }

    public void UpdateTimerText(float time)
    {
        if (timerText != null) timerText.text = $"{time:F1}s";
    }
}