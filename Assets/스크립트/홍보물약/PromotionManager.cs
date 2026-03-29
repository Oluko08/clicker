using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PromotionManager : MonoBehaviour
{
    private SystemZamsu scoreSystem;

    [Header("🖼️ 연출 설정")]
    public Image borderImage; 

    // 테두리 우선순위 관리를 위한 변수
    private int currentBorderOwnerID = 0;

    private void Awake()
    {
        scoreSystem = FindAnyObjectByType<SystemZamsu>();
        if (borderImage != null) borderImage.gameObject.SetActive(false);
    }

    public void UsePromotion(PromotionData data, PromotionButtonUI caller)
    {
        if (scoreSystem == null) return;

        // 계열별 중복 체크
        if (data.type == PromotionData.PromotionType.Click && scoreSystem.isClickPromoting) return;
        if (data.type == PromotionData.PromotionType.PPS && scoreSystem.isPpsPromoting) return;

        bool canUse = false;
        if (data.multiplier <= 11f)
        {
            if (scoreSystem.item10Count > 0) { scoreSystem.item10Count--; canUse = true; }
        }
        else
        {
            if (scoreSystem.item100Count > 0) { scoreSystem.item100Count--; canUse = true; }
        }

        if (canUse)
        {
            scoreSystem.SaveGame();
            // 새로운 홍보가 시작될 때마다 ID를 증가시켜 "최신"임을 표시
            currentBorderOwnerID++;
            StartCoroutine(PromotionRoutine(data, caller, currentBorderOwnerID));
        }
    }

    private IEnumerator PromotionRoutine(PromotionData data, PromotionButtonUI caller, int myID)
    {
        bool isClick = (data.type == PromotionData.PromotionType.Click);

        // 배율 적용 및 상태 설정
        if (isClick) { scoreSystem.isClickPromoting = true; scoreSystem.clickMultiplier *= data.multiplier; }
        else { scoreSystem.isPpsPromoting = true; scoreSystem.ppsMultiplier *= data.multiplier; }

        // 타이머 활성화
        if (caller != null) caller.SetTimerActive(true);

        float timeLeft = data.duration;
        float animTimer = 0f;
        bool isSprite1 = true;

        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            animTimer += Time.deltaTime;

            if (caller != null) caller.UpdateTimerText(timeLeft);

            // [핵심 로직] 내 ID가 가장 최신 ID일 때만 테두리를 조작함
            if (myID == currentBorderOwnerID)
            {
                if (borderImage != null)
                {
                    if (!borderImage.gameObject.activeSelf) borderImage.gameObject.SetActive(true);
                    
                    if (animTimer >= data.animSpeed)
                    {
                        animTimer = 0f;
                        isSprite1 = !isSprite1;
                        borderImage.sprite = isSprite1 ? data.borderSprite1 : data.borderSprite2;
                    }
                }
            }

            yield return null;
        }

        // 상태 복구
        if (isClick) { scoreSystem.isClickPromoting = false; scoreSystem.clickMultiplier /= data.multiplier; }
        else { scoreSystem.isPpsPromoting = false; scoreSystem.ppsMultiplier /= data.multiplier; }

        if (caller != null) caller.SetTimerActive(false);

        // 내가 마지막 주인이었고, 이제 홍보가 끝났다면 테두리를 끔
        // (만약 다른 홍보가 아직 진행 중이라면 그쪽에서 다시 켤 것임)
        if (myID == currentBorderOwnerID)
        {
            if (borderImage != null) borderImage.gameObject.SetActive(false);
        }

        scoreSystem.UpdateUI();
    }
}