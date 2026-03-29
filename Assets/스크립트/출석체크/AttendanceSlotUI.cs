using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class AttendanceSlotUI : MonoBehaviour
{
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI rewardText;
    public Image stampImage; // 도장 이미지 (평소엔 꺼져있음)
    public Button myButton;

    public void SetState(AttendanceData data, bool isToday, bool isReceived)
    {
        dayText.text = $"{data.dayIndex}일차";
        rewardText.text = data.rewardDisplayName;
        
        // 도장 표시 여부
        stampImage.gameObject.SetActive(isReceived);
        
        // 오늘 받을 차례면 버튼 활성화, 아니면 비활성화
        myButton.interactable = isToday;
    }

    public IEnumerator StampAnimation()
    {
        stampImage.gameObject.SetActive(true);
        RectTransform rect = stampImage.rectTransform;

        // 원근감 표현: 매우 큰 상태에서 원래 크기(1,1,1)로 작아지며 찍히기
        float duration = 0.4f;
        float elapsed = 0f;
        Vector3 startScale = new Vector3(5f, 5f, 5f);
        Vector3 endScale = Vector3.one;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            // 점점 작아지는 연출 (선입선출 느낌을 위해 커브 적용 가능)
            rect.localScale = Vector3.Lerp(startScale, endScale, t);
            yield return null;
        }
        
        rect.localScale = endScale;
        // 약간의 화면 흔들림 효과(선택사항) 등을 넣으면 더 좋습니다.
    }
}