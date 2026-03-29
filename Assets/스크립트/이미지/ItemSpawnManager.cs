using UnityEngine;
using System.Collections.Generic;

public class ItemSpawnManager : MonoBehaviour
{
    public static ItemSpawnManager instance; // 어디서든 부를 수 있게 싱글톤 설정

    [Header("아이템이 나타날 부모 오브젝트 (UI 혹은 빈 오브젝트)")]
    public Transform spawnParent; 

    // 이미 구매해서 화면에 떠 있는 아이템들을 관리하는 리스트 (중복 생성 방지용)
    private Dictionary<string, GameObject> spawnedItems = new Dictionary<string, GameObject>();

    private void Awake()
    {
        instance = this;
    }

    public void SpawnItem(TrendItemData data)
    {
        // 이미 화면에 있는 아이템이면 또 만들지 않음
        if (spawnedItems.ContainsKey(data.itemName)) return;

        if (data.animationPrefab != null)
        {
            // 프리팹 생성
            GameObject newItem = Instantiate(data.animationPrefab, spawnParent);
            
            // 데이터에 저장된 위치나 기본 위치 설정 (필요시 수정)
            newItem.transform.localPosition = Vector3.zero; 

            // 리스트에 추가
            spawnedItems.Add(data.itemName, newItem);
            Debug.Log($"{data.itemName} 애니메이션 생성 완료!");
        }
    }
}