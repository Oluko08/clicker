using UnityEngine;

[CreateAssetMenu(fileName = "NewTrendItem", menuName = "TrendSystem/Item")]
public class TrendItemData : ScriptableObject
{
    public string itemName;
    public double price;
    
    [Header("📈 능력치")]
    public double clickPowerAdd;
    public double perSecondAdd;
    
    [Header("🖼️ UI 비주얼 (상점용)")]
    public Sprite itemIcon;

    [Header("💃 메인 화면 캐릭터 설정 (깜빡이)")]
    public Sprite characterSprite1;      
    public Sprite characterSprite2;      
    public float animationInterval = 0.5f;

    [HideInInspector] // ItemSpawnManager 에러 방지용 (사용 안 함)
    public GameObject animationPrefab; 
}