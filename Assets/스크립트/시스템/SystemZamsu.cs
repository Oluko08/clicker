using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class SystemZamsu : MonoBehaviour
{
    [Header("💰 핵심 경제 데이터")]
    public double popularity = 0;      
    public double clickPower = 1;      
    public double pps = 0;             

    [Header("🚀 홍보 배율 (도핑)")]
    public float clickMultiplier = 1f;
    public float ppsMultiplier = 1f;

    [Header("📦 공용 아이템 개수")]
    public int item10Count = 0;  
    public int item100Count = 0; 

    [Header("🔒 홍보 중 상태 (중복 방지)")]
    public bool isClickPromoting = false;
    public bool isPpsPromoting = false;

    [Header("🖥️ 메인 UI 연결")]
    public TextMeshProUGUI popularityText;
    public TextMeshProUGUI ppsText;
    public TextMeshProUGUI clickPowerText;
    public TextMeshProUGUI upgradeCostText;
    public Button upgradeButton; 

    [Header("📈 클릭 강화 설정")]
    public double baseClickCost = 10;     
    public float costMultiplier = 1.2f;   
    public int clickLevel = 1;            

    [Header("🕒 잠수 보상 설정")]
    public float maxOfflineHours = 2.0f; 

    private string[] units = { "", "만", "억", "조", "경", "해", "자", "양", "구", "간", "정", "재", "극" };

    private void Start()
    {
        LoadData();        
        HandleOffline();   
        InvokeRepeating(nameof(UpdateUI), 0f, 1f);
    }

    private void Update()
    {
        if (pps > 0)
        {
            popularity += (pps * ppsMultiplier) * Time.deltaTime;
        }

        if (upgradeButton != null)
        {
            upgradeButton.interactable = (popularity >= GetClickUpgradeCost());
        }
    }

    public void OnMainClick()
    {
        popularity += (clickPower * clickMultiplier);
        UpdateUI();
    }

    public void Debug_AddItems()
    {
        item10Count += 5;
        item100Count += 5;
    }

    public void UpgradeClick()
    {
        double cost = GetClickUpgradeCost();
        if (popularity >= cost)
        {
            popularity -= cost;
            clickLevel++;
            clickPower += 1; 
            UpdateUI();
            SaveGame(); 
        }
    }

    public double GetClickUpgradeCost()
    {
        return Math.Floor(baseClickCost * Math.Pow(costMultiplier, clickLevel - 1));
    }

    public void UpdateUI()
    {
        if (popularityText != null)
            popularityText.text = GetKoreanUnit(Math.Floor(popularity)) + " 인지도";
        
        if (ppsText != null)
            ppsText.text = "초당 상승량: " + GetKoreanUnit(Math.Floor(pps * ppsMultiplier));
        
        if (clickPowerText != null)
            clickPowerText.text = "클릭당: " + GetKoreanUnit(Math.Floor(clickPower * clickMultiplier));
        
        if (upgradeCostText != null)
            upgradeCostText.text = "강화 비용: " + GetKoreanUnit(GetClickUpgradeCost());
    }

    public string GetKoreanUnit(double value)
    {
        if (value < 10000) return value.ToString("N0");
        
        List<string> parts = new List<string>();
        double tempNumber = Math.Floor(value);

        for (int i = 0; i < units.Length; i++)
        {
            double remainder = tempNumber % 10000;
            if (remainder > 0)
                parts.Insert(0, ((long)remainder).ToString("N0") + units[i]);
            tempNumber = Math.Floor(tempNumber / 10000);
            if (tempNumber <= 0) break;
        }

        if (parts.Count > 2) return parts[0] + " " + parts[1];
        return string.Join(" ", parts);
    }

    public void SaveGame()
    {
        PlayerPrefs.SetString("Popularity", popularity.ToString());
        PlayerPrefs.SetString("SavedPPS", pps.ToString());
        PlayerPrefs.SetInt("ClickLevel", clickLevel);
        PlayerPrefs.SetInt("Item10", item10Count);      
        PlayerPrefs.SetInt("Item100", item100Count);    
        PlayerPrefs.SetString("LastExitTime", DateTime.Now.ToString());
        PlayerPrefs.Save();
    }

    private void LoadData()
    {
        if (PlayerPrefs.HasKey("Popularity"))
        {
            double.TryParse(PlayerPrefs.GetString("Popularity"), out popularity);
            if (PlayerPrefs.HasKey("SavedPPS"))
                double.TryParse(PlayerPrefs.GetString("SavedPPS"), out pps);
            clickLevel = PlayerPrefs.GetInt("ClickLevel", 1);
            item10Count = PlayerPrefs.GetInt("Item10", 0);
            item100Count = PlayerPrefs.GetInt("Item100", 0);
            clickPower = 1 + (clickLevel - 1); 
        }
    }

    private void HandleOffline()
    {
        if (PlayerPrefs.HasKey("LastExitTime"))
        {
            try {
                DateTime lastTime = DateTime.Parse(PlayerPrefs.GetString("LastExitTime"));
                TimeSpan ts = DateTime.Now - lastTime;
                double secondsAway = Math.Min(ts.TotalSeconds, maxOfflineHours * 3600);
                if (pps > 0) popularity += pps * secondsAway;
            } catch { }
        }
    }

    private void OnApplicationQuit() { SaveGame(); }
}