using UnityEngine;
using TMPro;

public class ItemInventoryUI : MonoBehaviour
{
    private SystemZamsu scoreSystem;

    [Header("📝 텍스트 UI 연결")]
    public TextMeshProUGUI item10Text;  // 10배권 개수 표시용
    public TextMeshProUGUI item100Text; // 100배권 개수 표시용

    private void Awake()
    {
        // 씬에 있는 SystemZamsu를 자동으로 찾습니다.
        scoreSystem = FindAnyObjectByType<SystemZamsu>();
    }

    private void Update()
    {
        if (scoreSystem != null)
        {
            // 실시간으로 텍스트 업데이트
            if (item10Text != null)
                item10Text.text = $"10배권: {scoreSystem.item10Count}개";

            if (item100Text != null)
                item100Text.text = $"100배권: {scoreSystem.item100Count}개";
        }
    }
}