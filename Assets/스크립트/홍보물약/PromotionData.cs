using UnityEngine;

[CreateAssetMenu(fileName = "NewPromotion", menuName = "TrendSystem/Promotion")]
public class PromotionData : ScriptableObject
{
    public string promotionName;   // 효과의 이름 (예: 클릭 10배 광고)
    
    [Header("📈 효과 설정")]
    public float multiplier = 10f; // 배율 (10 또는 100)
    public float duration = 30f;   // 지속 시간
    
    public enum PromotionType { Click, PPS }
    public PromotionType type;     // 이 효과가 클릭용인지 PPS용인지 구분

    [Header("✨ 테두리 애니메이션 설정")]
    public Sprite borderSprite1;   
    public Sprite borderSprite2;   
    public float animSpeed = 0.5f; 
}