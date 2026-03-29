using UnityEngine;

public class List : MonoBehaviour
{
    [Header("📋 메인 카테고리 (버튼 2개가 있는 곳)")]
    public GameObject categoryPanel; 

    [Header("☝️ 클릭 강화용 방 (강화 버튼 + 뒤로가기)")]
    public GameObject clickUpgradePanel;

    [Header("🔥 유행 구매용 방 (리스트 + 뒤로가기)")]
    public GameObject trendShopPanel;

    private void Start()
    {
        // 처음 시작할 때는 '클릭 강화', '유행 구매' 버튼 2개만 보여야 함
        ShowCategory();
    }

    // [뒤로가기] 버튼에 이 함수를 연결하세요!
    public void ShowCategory()
    {
        categoryPanel.SetActive(true);      // 카테고리 버튼 2개 보이기
        clickUpgradePanel.SetActive(false); // 강화 방 숨기기
        trendShopPanel.SetActive(false);    // 유행 방 숨기기
    }

    // [클릭 강화] 버튼에 이 함수를 연결하세요!
    public void OpenClickUpgrade()
    {
        categoryPanel.SetActive(false);     // 카테고리 버튼 숨기기
        clickUpgradePanel.SetActive(true);  // 강화 방 보이기
        trendShopPanel.SetActive(false);
    }

    // [유행 구매] 버튼에 이 함수를 연결하세요!
    public void OpenTrendShop()
    {
        categoryPanel.SetActive(false);
        clickUpgradePanel.SetActive(false);
        trendShopPanel.SetActive(true);     // 유행 방 보이기
    }
}