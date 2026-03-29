using UnityEngine;
using System.Collections.Generic;

public class YUBOG : MonoBehaviour
{
    [Header("📋 아이템 데이터 리스트")]
    public List<TrendItemData> itemList; 
    
    [Header("🏗️ 프리팹 및 생성 위치")]
    public GameObject itemPrefab;   
    public Transform contentParent; 

    [Header("🎬 씬에 배치된 캐릭터들 (순서대로)")]
    public List<GameObject> characterObjects; 

    void Start()
    {
        SpawnItems();
    }

    void SpawnItems()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            GameObject obj = Instantiate(itemPrefab, contentParent);
            YUBOGItemUI ui = obj.GetComponent<YUBOGItemUI>();

            // i번째 아이템에게 i번째 캐릭터 오브젝트를 넘겨줍니다.
            GameObject targetChar = (i < characterObjects.Count) ? characterObjects[i] : null;
            ui.Setup(itemList[i], targetChar);
        }
    }
}