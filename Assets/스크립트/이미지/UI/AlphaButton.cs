using UnityEngine;
using UnityEngine.UI;

public class AlphaButton : MonoBehaviour
{
    private void Awake()
    {
        Image img = GetComponent<Image>();
        if (img != null)
        {
            // 투명도가 10% 미만인 곳은 클릭을 무시하도록 설정
            img.alphaHitTestMinimumThreshold = 0.1f;
        }
    }
}